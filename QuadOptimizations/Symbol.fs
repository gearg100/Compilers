module Symbol
open Types
open Error
open Identifier
open System.Collections.Generic

type pass_mode = 
    |PASS_BY_VALUE 
    |PASS_BY_REFERENCE
    |RET
    static member print = function
        |PASS_BY_VALUE -> "V"
        |PASS_BY_REFERENCE -> "R"
        |RET -> "RET"
type param_status = PARDEF_COMPLETE | PARDEF_DEFINE | PARDEF_CHECK
type scope = {
    sco_parent : scope option
    sco_nesting : int
    sco_function: entry option
    mutable sco_entries : entry list
    mutable sco_negofs : int
}

and variable_info = {
    variable_type   : typ
    mutable variable_offset : int
}

and function_info = {
    function_unique_name : string
    mutable function_isForward : bool
    mutable function_paramlist : entry list
    mutable function_redeflist : entry list
    mutable function_result    : typ
    mutable function_pstatus   : param_status
    mutable function_initquad  : int
    mutable function_negoffs   : int
    mutable function_posoffs   : int
    mutable function_mutatesForeignBytes : bool
    mutable function_mutatesForeignInts : bool
}

and parameter_info = {
    parameter_type           : typ
    mutable parameter_offset : int
    parameter_mode           : pass_mode
}

and temporary_info = {
    temporary_type   : typ
    mutable temporary_offset : int
}

and entry_info = 
    | ENTRY_none
    | ENTRY_variable of variable_info
    | ENTRY_function of function_info
    | ENTRY_parameter of parameter_info
    | ENTRY_temporary of temporary_info

and [<ReferenceEquality>] entry =
    {
    entry_id    : id
    entry_scope : scope
    entry_info  : entry_info       
    }   
    static member GetType = function 
        | {entry_info=ENTRY_none} -> 
            TYPE_none
        | {entry_info=ENTRY_variable v}-> 
            v.variable_type
        | {entry_info=ENTRY_function f}-> 
            f.function_result
        | {entry_info=ENTRY_parameter p} -> 
            p.parameter_type
        | {entry_info=ENTRY_temporary t} -> 
            t.temporary_type
    static member GetKind = function
        | {entry_info=ENTRY_none} -> 
            "nothing"
        | {entry_info=ENTRY_variable v}-> 
            "variable"
        | {entry_info=ENTRY_function f}-> 
            "function"
        | {entry_info=ENTRY_parameter p} -> 
            "parameter"
        | {entry_info=ENTRY_temporary t} -> 
            "temporary"
    override x.ToString() = 
        "{ id = (" + x.entry_id.node + "," + x.entry_id.tag.ToString() +
        "), kind = " + entry.GetKind x +
        ", type = " + sprintf "%A" (entry.GetType x) +
        ", nesting level = " + x.entry_scope.sco_nesting.ToString() + " }"

and entryWithTypeAndNesting =
    {
    entry : entry
    usageNest :int
    entryType : typ
    }
    override x.ToString() =
       x.entry.ToString()

type lookup_type = LOOKUP_CURRENT_SCOPE | LOOKUP_ALL_SCOPES

let start_positive_offset = 8
let start_negative_offset = 0

let the_outer_scope = 
    {
        sco_parent   = None
        sco_nesting  = System.Int32.MaxValue
        sco_function = None
        sco_entries  = []
        sco_negofs   = start_negative_offset
    }

let no_entry id = 
    {
        entry_id    = id
        entry_scope = the_outer_scope
        entry_info  = ENTRY_none
    }

let currentScope = ref the_outer_scope
let quadNext     = ref 1
let tempNumber   = ref 1 
let name =
    let counter= ref 0
    fun nameMode (p:id)->
        if nameMode then 
            let name = sprintf "%s_%d" (id_name p) !counter
            incr counter
            name
        else
            let name = sprintf "%s" (id_name p)
            name

let tab = ref (new HashMultiMap<id,entry> (0,HashIdentity.Structural))

let initSymbolTable (size:int) =
    tab := new HashMultiMap<id,entry> (size,HashIdentity.Structural)
    currentScope := the_outer_scope

let openScope f =
    let sco = 
        {
            sco_parent   = Some !currentScope
            sco_nesting  = if (!currentScope).sco_nesting=System.Int32.MaxValue then 1 else (!currentScope).sco_nesting + 1
            sco_function = f
            sco_entries  = []
            sco_negofs   = start_negative_offset
        }
    currentScope := sco

let closeScope () =
    let sco = !currentScope
    let manyentry (e:entry) = (!tab).Remove e.entry_id
    List.iter manyentry sco.sco_entries
    match sco.sco_parent with
    | Some scp ->
        match sco.sco_function.Value.entry_info with
        |ENTRY_function f ->f.function_negoffs <- -sco.sco_negofs
        | _ -> internal_error (__SOURCE_FILE__,__LINE__) "not a function!\n"
        currentScope := scp
    | None ->
        internal_error (__SOURCE_FILE__,__LINE__) "cannot close the outer scope! \n"
exception Failure_NewEntry of entry
exception Exit
let newEntry (id:id) inf err =
    try
        if err then
            match (!tab).TryFind id with
            |Some e -> if (e.entry_scope.sco_nesting = (!currentScope).sco_nesting) then
                        raise (Failure_NewEntry e)
            |None -> ()
        let e = {
                    entry_id    = id
                    entry_scope = !currentScope
                    entry_info  = inf
                } 
        (!tab).Add (id, e)
        (!currentScope).sco_entries <- e :: (!currentScope).sco_entries
        e
    with Failure_NewEntry e ->
        error "duplicate identifier %a \n" pretty_id id
        e

let lookupEntry id how err =
    let scc = !currentScope
    let lookup () =
        match how, (!tab).TryFind id with
        | LOOKUP_CURRENT_SCOPE, Some e when e.entry_scope.sco_nesting <> scc.sco_nesting -> None
        | _, x -> x
    if err then
        match lookup () with
        | Some e as x-> x
        | None ->
            error "unknown identifier %a (first occurrence) \n" pretty_id id
            (* put it in, so we don't see more errors *)
            (!tab).Add (id, (no_entry id))
            raise Exit
    else
        lookup()

let newVariable id typ err =
    (!currentScope).sco_negofs <- (!currentScope).sco_negofs - (sizeOfType typ|> int)
    let inf = {
                variable_type   = typ
                variable_offset = (!currentScope).sco_negofs
            }
    newEntry id (ENTRY_variable inf) err

let newFunction id err nameMode=
    match lookupEntry id LOOKUP_CURRENT_SCOPE false with
    | Some e -> 
        match e.entry_info with
        | ENTRY_function inf when inf.function_isForward ->
            inf.function_isForward <- false
            inf.function_pstatus <- PARDEF_CHECK
            inf.function_redeflist <- inf.function_paramlist
            e
        | _ ->
            if err then
                error "duplicate identifier: %a \n" pretty_id id
            raise Exit
    | None -> 
        let inf =   {
                    function_unique_name = id |> name nameMode
                    function_isForward = false
                    function_paramlist = []
                    function_redeflist = []
                    function_result    = TYPE_none
                    function_pstatus   = PARDEF_DEFINE
                    function_initquad  = 0
                    function_posoffs   = 0
                    function_negoffs   = 0
                    function_mutatesForeignBytes = false
                    function_mutatesForeignInts = false
                } 
        newEntry id (ENTRY_function inf) false

let newParameter id typ mode f err =
    match f.entry_info with
    | ENTRY_function inf -> 
        match inf.function_pstatus with
        | PARDEF_DEFINE ->
            let inf_p = 
                {
                    parameter_type   = typ
                    parameter_offset = 0
                    parameter_mode   = mode
                } 
            let e = newEntry id (ENTRY_parameter inf_p) err
            inf.function_paramlist <- e :: inf.function_paramlist
            e
        | PARDEF_CHECK -> 
            match inf.function_redeflist with
            | p :: ps -> 
                inf.function_redeflist <- ps
                match p.entry_info with
                | ENTRY_parameter inf ->
                    if not (equalType inf.parameter_type typ) then
                        error "Parameter type mismatch in redeclaration of function %a \n" pretty_id f.entry_id
                    elif inf.parameter_mode <> mode then
                        error "Parameter passing mode mismatch in redeclaration of function %a \n" pretty_id f.entry_id
                    elif p.entry_id <> id then
                        error "Parameter name mismatch in redeclaration of function %a \n" pretty_id f.entry_id
                    else
                    (!tab).Add (id, p)
                    p
                | _ ->
                    internal_error (__SOURCE_FILE__,__LINE__) "I found a parameter that is not a parameter! \n"
                    raise Exit
            | [] ->
                error "More parameters than expected in redeclaration of function %a \n" pretty_id f.entry_id
                raise Exit
        | PARDEF_COMPLETE ->
            internal_error (__SOURCE_FILE__,__LINE__) "Cannot add a parameter to an already defined function \n"
            raise Exit
    | _ ->
        internal_error (__SOURCE_FILE__,__LINE__) "Cannot add a parameter to a non-function \n"
        raise Exit

let newTemporary typ =
    let id = id_make ("$" + (!tempNumber).ToString())
    (!currentScope).sco_negofs <- (!currentScope).sco_negofs - (sizeOfType typ |>int)
    let inf = 
        {
            temporary_type = typ
            temporary_offset = (!currentScope).sco_negofs
        }
    incr tempNumber
    newEntry id (ENTRY_temporary inf) false

let forwardFunction e =
    match e.entry_info with
    | ENTRY_function inf ->
        inf.function_isForward <- true
    | _ ->
        internal_error (__SOURCE_FILE__,__LINE__) "Cannot make a non-function forward \n"

let endFunctionHeader e typ =
    match e.entry_info with
    | ENTRY_function inf ->
        match inf.function_pstatus with
        | PARDEF_COMPLETE -> 
            internal_error (__SOURCE_FILE__,__LINE__) "Cannot end parameters in an already defined function \n"
        | PARDEF_DEFINE ->
            inf.function_result <- typ
            let offset =
                if typ = TYPE_proc 
                then
                    ref start_positive_offset
                else 
                    newParameter (id_make "$$") typ RET e true |> ignore
                    ref (start_positive_offset - 2)
            let fix_offset e =
                match e.entry_info with
                | ENTRY_parameter inf ->
                    inf.parameter_offset <- !offset
                    let size =
                        match inf.parameter_mode with
                        | PASS_BY_VALUE -> sizeOfType inf.parameter_type
                        | PASS_BY_REFERENCE|RET -> 2s                
                    offset := !offset + (size |>int)
                | _ ->
                    internal_error (__SOURCE_FILE__,__LINE__) "Cannot fix offset to a non parameter \n"
            List.iter fix_offset inf.function_paramlist
            inf.function_paramlist <- inf.function_paramlist
            inf.function_posoffs <- !offset - start_positive_offset
        | PARDEF_CHECK ->
            if inf.function_redeflist <> [] then
                error "Fewer parameters than expected in redeclaration of function %a \n" pretty_id e.entry_id
            if not (equalType inf.function_result typ) then
                error "Result type mismatch in redeclaration of function %a \n"
                    pretty_id e.entry_id
        inf.function_pstatus <- PARDEF_COMPLETE
    | _ ->
        internal_error (__SOURCE_FILE__,__LINE__) "Cannot end parameters in a non-function \n"
