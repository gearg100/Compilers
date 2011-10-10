﻿module AuxFunctions

open Types
open Symbol
open Error
open Identifier
open ParserTypes
open QuadSupport
open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing

#nowarn "25"
let inline lexeme () = LexBuffer<char>.LexemeString
let inline FindPosition (state:IParseState) = (state.InputStartPosition(1).Line,state.InputStartPosition(1).Column)
let inline EHandler (ctxt: ParseErrorContext<_>) = 
    let p=(ctxt.ParseState.InputStartPosition(1).Line,ctxt.ParseState.InputStartPosition(1).Column)
    (error "Syntax Error at %A: Unrecognized Syntax Error\n" p )
let inline getScopeFunction () = (!currentScope).sco_function.Value
let inline setScopeFunctionsByteMutationFlag () = 
    let (ENTRY_function finfo) = getScopeFunction().entry_info
    finfo.function_mutatesForeignBytes <- true
let inline setScopeFunctionsIntMutationFlag () = 
    let (ENTRY_function finfo) = getScopeFunction().entry_info
    finfo.function_mutatesForeignInts <- true
let inline getScopeNesting () = (!currentScope).sco_nesting
let inline getEntryType e = entry.GetType e
let inline getReturnType f =
    match f.entry_info with
    |ENTRY_function x -> 
        x.function_result
    |_ ->
        internal_error (__SOURCE_FILE__,__LINE__) "Not A Function"
        raise Terminate
let inline checkCondSemantics x1 x2 (state:IParseState) txt =
    if (x1=x2) then true
               else let (ps,pe)=state.ResultRange
                    error txt (ps.Line,ps.Column) (pe.Line,pe.Column) 
                    false

let inline syntaxError (state:IParseState) txt ret =
    let (ps,pe)=state.ResultRange
    error txt (ps.Line,ps.Column) (pe.Line,pe.Column)
    |>ignore
    ret

let setInitialPos (lexbuf:LexBuffer<char>) filename = 
       lexbuf.EndPos <- { pos_bol = 0;
                          pos_fname=filename; 
                          pos_cnum=0;
                          pos_lnum=1 }

let LibraryFunctions = [
    ("writeInteger",[("win" ,TYPE_int                 ,PASS_BY_VALUE    )],TYPE_proc);
    ("writeByte"   ,[("wby" ,TYPE_byte                ,PASS_BY_VALUE    )],TYPE_proc);
    ("writeChar"   ,[("wch" ,TYPE_byte                ,PASS_BY_VALUE    )],TYPE_proc);
    ("writeString" ,[("wst" ,TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_proc);
    ("readInteger" ,[]                                                    ,TYPE_int );
    ("readByte"    ,[]                                                    ,TYPE_byte);
    ("readChar"    ,[]                                                    ,TYPE_byte);
    ("readString"  ,[("rst1",TYPE_int                 ,PASS_BY_VALUE    )
                    ;("rst2",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_proc);
    ("extend"      ,[("ext" ,TYPE_byte                ,PASS_BY_VALUE    )],TYPE_int);
    ("shrink"      ,[("scr" ,TYPE_int                 ,PASS_BY_VALUE    )],TYPE_byte);
    ("strlen"      ,[("stl" ,TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_int );
    ("strcmp"      ,[("cmp1",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)
                    ;("cmp2",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_int );
    ("strcpy"      ,[("cpyd",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)
                    ;("cpys",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_proc);
    ("strcat"      ,[("catd",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)
                    ;("cats",TYPE_array (TYPE_byte,-1s),PASS_BY_REFERENCE)],TYPE_proc);
                       ]

let inline bulkLoad (fName,parameterList,fType) = 
    let f = newFunction (id_make fName) true false
    openScope (Some f)
    List.iter (fun (pName,pType,pPass) -> newParameter (id_make pName) pType pPass f true |> ignore ) parameterList
    endFunctionHeader f fType
    closeScope()

let inline FindPosAndEntry (state:IParseState) id =
    (FindPosition state,  lookupEntry (id_make id) LOOKUP_ALL_SCOPES true)

//Definitions
let inline processFunctionDefinition (state:IParseState) f localDefinitions body=
    let cleanedLocalDefinitions = 
        localDefinitions
        |> List.fold (fun acc curr -> if curr=[] then acc else curr::acc) []
        |> List.rev
    (QuadEndUnit(f) :: body @ [QuadUnit(f)]) :: (cleanedLocalDefinitions)

//Statements
let inline checkStmtSemantics (state:IParseState) x1 x2 txt =
    if (x1=x2) then 
        true 
    else 
        let (ps,pe)=state.ResultRange
        error txt (ps.Line,ps.Column) (pe.Line,pe.Column) false
let inline checkReturnSemantics (state:IParseState) typ  =
    let t = getScopeFunction >> getReturnType <| ()
    if (t = typ) then 
        true 
    else 
        let (ps,pe)=state.ResultRange
        error "Semantic Error at %A -%A: \n\tType Mismatch: Return type must be %A but is %A\n" (ps.Line,ps.Column) (pe.Line,pe.Column) t typ
        false

let inline processAssignment (state:IParseState) (l:expressionType) (e:expressionType) =
    let t1 = (l.Place |> quadElementType.GetType)
    let t2 = (e.Place |> quadElementType.GetType)
    if (checkStmtSemantics state t1 t2 "Semantic Error at %A - %A: Type Mismatch1\n")
    then 
        match l.Place with
        |Entry ent |Valof ent ->
            if ent.entry.entry_scope.sco_nesting <> getScopeNesting () then
                if ent.entryType = TYPE_int then setScopeFunctionsIntMutationFlag()
                elif ent.entryType = TYPE_byte then setScopeFunctionsByteMutationFlag()
            QuadAssign(e.Place,l.Place)::l.Code@e.Code
        |_ -> 
            internal_error (__SOURCE_FILE__,__LINE__) "Not an Entry"
            raise Terminate
    else 
        []

let inline processFunctionCallStmt (state:IParseState) (func:expressionType) = 
    match func.Place with
    |QNone -> 
        func.Code
    |_ ->
        let (ps,pe)=state.ResultRange
        error "Semantic Error at %A - %A: Type Mismatch: Function Return Type is %A instead of proc\n" (ps.Line,ps.Column) (pe.Line,pe.Column)  (func.Place |> quadElementType.GetType) false
        []

let inline processIfStmt (state:IParseState) (condition:conditionType) (statement:quadType list) =
    List.iter (fun x -> x := !x + statement.Length ) condition.False
    statement @ condition.Code

let inline processIfElseStmt (state:IParseState) (condition:conditionType) (thenStatement:quadType list) (elseStatement:quadType list) =
    match thenStatement with
    |[] ->
        List.iter (fun x -> incr x) condition.False
        elseStatement @ ( (QuadJump(ref (elseStatement.Length+1)) ) :: (condition.Code) )
    |(QuadRet(_)::_) ->
        List.iter (fun x -> x := !x + thenStatement.Length ) condition.False
        elseStatement @ ( thenStatement@condition.Code )
    |_ ->
        List.iter (fun x -> x := !x + thenStatement.Length + 1) condition.False
        elseStatement @ ( (QuadJump(ref (elseStatement.Length+1)) ) :: (thenStatement@condition.Code) )

let inline processWhileStmt (state:IParseState) (condition:conditionType) (statement:quadType list) =
    List.iter (fun x -> x := !x + statement.Length + 1) condition.False
    QuadJump(ref -(statement.Length + condition.Code.Length)) :: (statement @ condition.Code )

let inline processReturnExpressionStmt (state:IParseState) (expression:expressionType) =
    if ((expression.Place |> quadElementType.GetType)
        |> checkReturnSemantics state)
    then 
        match (lookupEntry (id_make "$$") LOOKUP_CURRENT_SCOPE true) with
        |Some ent ->
            let e = { entry = ent; entryType = getEntryType ent; usageNest = getScopeNesting () }
            (QuadRet (!currentScope).sco_function.Value)::QuadAssign(expression.Place,Entry e)::expression.Code
        |None ->
            []
    else
        []

let inline processReturnStmt (state:IParseState) =
    if (checkReturnSemantics state TYPE_proc)
    then
        [QuadRet (!currentScope).sco_function.Value]
    else
        []

//FunctionCall
let inline getParameterTypeAndPassMode e = 
    match e.entry_info with
    |ENTRY_parameter p -> 
        (p.parameter_type,p.parameter_mode)
    |_ -> 
        internal_error (__SOURCE_FILE__,__LINE__) "Not a Parameter"
        raise Terminate

let inline checkParameters flag e p = 
    match e, p with
    |(t1,p1),(t2,p2) when t1==t2 && (if p1=PASS_BY_REFERENCE then (p1=p2) else true)->flag
    |_ ->false  

let inline processFunctionCall (state:IParseState) id (paramList:parameterListnew) =
    let p,e = FindPosAndEntry state id
    let paramSet = new System.Collections.Generic.HashSet<entryWithTypeAndNesting>()
    let inline CheckAndCreateParameterCode (actualParList:parameterListnew) (parList:entry list) resultType func=
        let rec createParameterCodeHelper actual expected acc =
            match actual, expected with 
            |[],[] ->
                Some acc
            |((expression,mode)::t1),(expectedParameter::t2) ->
                let actualType, actualMode = quadElementType.GetType expression.Place, mode
                let (ENTRY_parameter p) = expectedParameter.entry_info
                let expectedType, expectedMode = p.parameter_type, p.parameter_mode
                if actualType == expectedType && (expectedMode = PASS_BY_VALUE || actualMode = expectedMode)
                then
                    if expectedMode = PASS_BY_REFERENCE then
                        match expression.Place with
                        | Entry e -> paramSet.Add e |>ignore
                        | _ -> ()
                    createParameterCodeHelper t1 t2 (acc @ (QuadPar(expression.Place,expectedMode) :: (expression.Code)))
                else
                    None
            | _,_ -> 
                None
        let codeOption = 
            match parList with 
            |e::_ when let (ENTRY_parameter p) = e.entry_info in p.parameter_mode = RET && resultType == p.parameter_type ->
                createParameterCodeHelper actualParList parList.Tail []
            | _ -> 
                createParameterCodeHelper actualParList parList []
        match codeOption with
        |Some code ->
            func code
        |None ->
            let inline getActualTypeAndMode (parList:parameterListnew) resultType =
                let rec getActualTypeAndModeHelper rest acc=
                    match rest with
                    |[] -> 
                        acc
                    |(expr,mode)::t ->
                        getActualTypeAndModeHelper t ((quadElementType.GetType expr.Place, mode) :: acc)
                getActualTypeAndModeHelper parList (if resultType = TYPE_proc then [] else [resultType,RET])
            let inline getExpectedTypeAndMode (parList:entry list) =
                let rec getExpectedTypeAndModeHelper rest acc=
                    match rest with
                    |[] ->
                        acc
                    |param :: t ->
                        match param.entry_info with
                        |ENTRY_parameter p -> 
                            getExpectedTypeAndModeHelper t ((p.parameter_type,p.parameter_mode)::acc)
                        |_ -> 
                            internal_error (__SOURCE_FILE__,__LINE__) "Not a Parameter"
                            raise Terminate                        
                getExpectedTypeAndModeHelper parList []
            let inline errorcase exp act = 
                if paramList =[] then 
                    error "Semantic Error at %A: Parameter Mismatch at function %s:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p id exp
                else
                    error "Semantic Error at %A: Parameter Mismatch at function %s:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p id exp act
                printf "ERROR"; voidExpression
            let actualTypeAndMode = 
                getActualTypeAndMode paramList resultType
            let expectedTypeAndMode = 
                getExpectedTypeAndMode parList
            errorcase expectedTypeAndMode actualTypeAndMode
    match e with
    |Some entry->
        match entry.entry_info with
        |ENTRY_function f ->
            if f.function_mutatesForeignBytes then setScopeFunctionsByteMutationFlag()
            elif f.function_mutatesForeignBytes then setScopeFunctionsIntMutationFlag()
            match f.function_result with
            |TYPE_proc ->
                CheckAndCreateParameterCode paramList f.function_paramlist f.function_result
                    (fun code ->
                        let fn = { entry = entry; entryType = TYPE_proc; usageNest = getScopeNesting () - 1 }
                        {
                            Code = QuadCall(fn, paramSet)::code
                            Place = QNone
                        })
            |TYPE_int|TYPE_byte as resultType->
                CheckAndCreateParameterCode paramList f.function_paramlist f.function_result
                    (fun code ->
                        let temp = newTemporary resultType
                        let fn = { entry = entry; entryType = resultType; usageNest = getScopeNesting () - 1 }
                        let tmp = { entry = temp; entryType = resultType; usageNest = getScopeNesting () }
                        {
                            Code = QuadCall(fn,paramSet)::QuadPar ( Entry(tmp) , RET)::code
                            Place = Entry(tmp)
                        })
            |_ -> error "Semantic Error at %A: wtf\n" p
        |_ ->
             error "Semantic Error at %A: Given name is not a Function\n" p
             printf "ERROR"; voidExpression
    |None -> 
        error "undeclared Identifier"
        printf "ERROR"; voidExpression 

//LValue
let inline getVariableOrParameterType (entry:entry) p=
    match entry.entry_info with
    |ENTRY_variable v ->  
        v.variable_type
    |ENTRY_parameter p ->  
        p.parameter_type
    |_ ->
        error "Semantic Error at %A: The give name is neither a variable nor a parameter name\n" p 

let inline processLValue1 (state:IParseState) id = 
    let p,e = FindPosAndEntry state id
    match e with
    |Some entry->
        let e = { entry = entry; entryType = getVariableOrParameterType entry p ;usageNest = getScopeNesting () }
        {
            Code = []
            Place = Entry(e)
        }
    |None ->
        error "undeclared variable" |> ignore;
        printf "ERROR"; voidExpression

let inline processLValue2 (state:IParseState) id =
    let p,e = FindPosAndEntry state id
    match e with
    |Some entry->
        match getVariableOrParameterType entry p with
        |TYPE_int|TYPE_byte as typ->
            let e = { entry = entry; entryType = typ ;usageNest = getScopeNesting () }
            {
                Code = []
                Place = Entry(e)
            }
        |_ ->
            error "Semantic Error at %A: type mismatch: LValue neither Int nor Byte\n" p
            printf "ERROR"; voidExpression
    |None ->
        error "undeclared variable" |>ignore
        printf "ERROR"; voidExpression

let inline processLValue3 (state:IParseState) id (index : expressionType)=
    let p,e = FindPosAndEntry state id
    let tOfIndex = index.Place |> quadElementType.GetType
    match e with
    |Some entry->
        match getVariableOrParameterType entry p with
        |TYPE_array (typ,_) as arrayType when tOfIndex=TYPE_int -> 
            let temp = newTemporary TYPE_int
            let ar = { entry = entry; entryType = arrayType ;usageNest = getScopeNesting () }
            let tmp = { entry = temp; entryType = typ; usageNest = getScopeNesting () }
            {
                Code = (QuadArray(ar, index.Place, tmp) :: index.Code)
                Place = Valof(tmp)
            }
        |TYPE_array _ as arrayType->
            error "Semantic Error at %A: type mismatch: Invalid Array Index type = %A\n" p arrayType
            printf "ERROR"; voidExpression
        |_ ->
            error "Semantic Error at %A: type mismatch: not an Array\n" p 
            printf "ERROR"; voidExpression
    |None -> 
        error "Semantic Error at %A: undeclared Array\n" p 
        printf "ERROR"; voidExpression

//Expressions
let inline checkExprSemantics op x1 x2 (state:IParseState) txt =
    match op x1 x2 with
    |true ->
        true
    |_ -> 
        let (ps,pe)=state.ResultRange
        error txt (ps.Line,ps.Column) (pe.Line,pe.Column)
        false

let inline processLValueExpression (state:IParseState) (l:expressionType) =
    l

let inline processFunctionCallExpression (state:IParseState) f =
    let t = (f.Place |> quadElementType.GetType)
    if (checkExprSemantics (<>) t TYPE_proc state "Semantic Error at %A -%A: void Function call\n") 
    then f else printf "ERROR"; voidExpression

let inline processUnExpression (state:IParseState) (op:unOpType) (e:expressionType) =
    let typ = (e.Place |> quadElementType.GetType)
    if (checkExprSemantics (=) typ TYPE_int state "Semantic Error at %A - %A: Invalid use of Unary Operator\n") 
    then
        let temp = newTemporary typ
        let tmp = { entry = temp; entryType = typ; usageNest = getScopeNesting () }
        { 
            Code = (QuadUnOperation(op,e.Place, tmp) :: e.Code)
            Place = Entry(tmp)
        }
    else
        printf "ERROR"; voidExpression

let inline processBinExpression (state:IParseState) (op:binOpType) (e1:expressionType) (e2:expressionType) =
    let t1 = (e1.Place |> quadElementType.GetType)
    let t2 = (e2.Place |> quadElementType.GetType)
    if (checkExprSemantics (=) t2 t1 state "Semantic Error at %A - %A: Type Mismatch3\n") 
    then 
        let temp = newTemporary t1
        let tmp = { entry = temp; entryType = t1; usageNest = getScopeNesting () }
        { 
            Code = (QuadBinOperation(op,e1.Place, e2.Place, tmp) :: (e2.Code @ e1.Code))
            Place = Entry(tmp)
        }
    else
        printf "ERROR"; voidExpression

//Conditions
let inline processComparison (state:IParseState) (op:compType) (e1:expressionType) (e2:expressionType)=
    if (checkCondSemantics (e1.Place |> quadElementType.GetType) (e2.Place |> quadElementType.GetType) 
            state "Semantic Error at %A - %A: Type Mismatch4\n")
    then
        let True, False = ref 1, ref 1
        {
            Code    = QuadComparison(op,e1.Place, e2.Place, True, False)::e2.Code@e1.Code
            True    = [True]
            False   = [False]
        }
    else
        voidCondition

let inline processAndCondition (state:IParseState) (condition1:conditionType) (condition2:conditionType)=
    List.iter (fun x -> x := !x + condition2.Code.Length) condition1.False
    { 
        Code    = condition2.Code @ condition1.Code
        True    = condition2.True
        False   = condition1.False @ condition2.False
    }

let inline processOrCondition (state:IParseState) (condition1:conditionType) (condition2:conditionType)=
    List.iter (fun x -> x := !x + condition2.Code.Length) condition1.True
    { 
        Code    = condition2.Code @ condition1.Code
        True    = condition1.True @ condition2.True
        False   = condition2.False
    }

let inline processTrue () =
    let True =  ref 1
    {
        Code    = [QuadJump(True)]
        True    = [True]
        False   = []
    }

let inline processFalse () =
    let False =  ref 1
    {
        Code    = [QuadJump(False)]
        True    = []
        False   = [False]
    }