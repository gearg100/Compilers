﻿module AuxFunctions

open Types
open Symbol
open Error
open Identifier
open QuadSupport
open ParserTypes
open Microsoft.FSharp.Text.Lexing
open Microsoft.FSharp.Text.Parsing

let inline lexeme () = LexBuffer<char>.LexemeString
let inline FindPosition (state:IParseState) = (state.InputStartPosition(1).Line,state.InputStartPosition(1).Column)
let inline EHandler (ctxt: ParseErrorContext<_>) = 
    let p=(ctxt.ParseState.InputStartPosition(1).Line,ctxt.ParseState.InputStartPosition(1).Column)
    (error "Syntax Error at %A: Unrecognized Syntax Error\n" p )
let inline getScopeFunction () = (!currentScope).sco_function.Value
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
    ("writeString" ,[("wst" ,TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_proc);
    ("readInteger" ,[]                                                    ,TYPE_int );
    ("readByte"    ,[]                                                    ,TYPE_byte);
    ("readChar"    ,[]                                                    ,TYPE_byte);
    ("readString"  ,[("rst1",TYPE_int                 ,PASS_BY_VALUE    )
                    ;("rst2",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_proc);
    ("extend"      ,[("ext" ,TYPE_byte                ,PASS_BY_VALUE    )],TYPE_proc);
    ("shrink"      ,[("scr" ,TYPE_int                 ,PASS_BY_VALUE    )],TYPE_proc);
    ("strlen"      ,[("stl" ,TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_int );
    ("strcmp"      ,[("cmp1",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)
                    ;("cmp2",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_int );
    ("strcpy"      ,[("cpyd",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)
                    ;("cpys",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_proc);
    ("strcat"      ,[("catd",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)
                    ;("cats",TYPE_array (TYPE_byte,-1),PASS_BY_REFERENCE)],TYPE_proc);
                       ]

let inline bulkLoad (fName,parameterList,fType) = 
    let f = newFunction (id_make fName) true
    openScope (Some f)
    List.iter (fun (pName,pType,pPass) -> newParameter (id_make pName) pType pPass f true |> ignore ) parameterList
    endFunctionHeader f fType
    closeScope()

let inline FindPosAndEntry (state:IParseState) id =
    (FindPosition state,  lookupEntry (id_make id) LOOKUP_ALL_SCOPES true)

//Definitions
let inline processFunctionDefinition (state:IParseState) f localDefinitions body=
    QuadEndUnit(f) :: (body@ [QuadUnit(f)]@ (localDefinitions ))
//Statements

let inline checkStmtSemantics (state:IParseState) x1 x2 txt =
    if (x1=x2) then 
        true 
    else 
        let (ps,pe)=state.ResultRange
        error txt (ps.Line,ps.Column) (pe.Line,pe.Column) false
let inline checkReturnSemantics (state:IParseState) typ  =
    let t = () |> getScopeFunction |> getReturnType
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
        |Entry entry -> 
            (QuadAssign(e.Place,entry))::l.Code@e.Code
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
    QuadJump(ref -(statement.Length + 2)) :: (statement @ condition.Code )

let inline processReturnExpressionStmt (state:IParseState) (expression:expressionType) =
    if ((expression.Place |> quadElementType.GetType)
        |> checkReturnSemantics state)
    then 
        match (lookupEntry (id_make "$$") LOOKUP_CURRENT_SCOPE true) with
        |Some entry ->
            QuadRet::QuadAssign(expression.Place,entry)::expression.Code
        |None ->
            []
    else
        []

let inline processReturnStmt (state:IParseState) =
    if (checkReturnSemantics state TYPE_proc)
    then
        [QuadRet]
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

let inline processFunctionCall (state:IParseState) id (expressionList:expressionType list, parameterList) =
    let p,e = FindPosAndEntry state id
    let inline createParameterCode acc expression (_,passMode) =
        (QuadPar(expression.Place,passMode)) :: (expression.Code @ acc)
    match e with
    |Some entry->
        match entry.entry_info with
        |ENTRY_function f ->
            match f.function_result with
            |TYPE_proc when f.function_paramlist.Length=expressionList.Length ->
                let actualTypeAndMode = 
                    List.map2 (fun quad mode -> (quad.Place |> quadElementType.GetType , mode)) (List.rev expressionList) (List.rev parameterList)
                let expectedTypeAndMode = 
                    List.map getParameterTypeAndPassMode f.function_paramlist
                if(List.fold2 checkParameters true expectedTypeAndMode actualTypeAndMode)
                then
                    let code = List.fold2 createParameterCode [] (List.rev expressionList) expectedTypeAndMode
                    let z = 
                        {
                            Code = QuadCall(entry)::code
                            Place = QNone
                        }
                    //printfn "%A" z
                    z
                else
                    if expressionList =[] then 
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p expectedTypeAndMode
                    else
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p expectedTypeAndMode actualTypeAndMode
                    printf "ERROR"; voidExpression
            |TYPE_proc ->
                let actualTypeAndMode = List.map2 (fun quad mode -> (quad.Place |> quadElementType.GetType , mode)) (expressionList) (parameterList)|>List.rev
                let expectedTypeAndMode = List.map getParameterTypeAndPassMode f.function_paramlist
                if expressionList =[] then 
                    error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p expectedTypeAndMode
                else
                    error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p expectedTypeAndMode actualTypeAndMode
                printf "ERROR"; voidExpression
            |TYPE_int when f.function_paramlist.Length=expressionList.Length+1 ->
                let actualTypeAndMode = 
                    (TYPE_int, RET)::(List.map2 (fun quad mode -> (quad.Place |> quadElementType.GetType , mode)) (expressionList) (parameterList))
                let expectedTypeAndMode = 
                    List.map getParameterTypeAndPassMode f.function_paramlist
                //printfn "%A\n%A" actualTypeAndMode expectedTypeAndMode
                if(List.fold2 checkParameters true expectedTypeAndMode actualTypeAndMode)
                then
                    let code = List.fold2 createParameterCode [] (List.rev expressionList) (expectedTypeAndMode
                                                                                           |>Seq.take expressionList.Length
                                                                                           |>List.ofSeq)
                    let temp = newTemporary f.function_result
                    let z = 
                        {
                            Code = QuadCall(entry)::QuadPar ( Entry(temp) , RET)::code
                            Place = Entry(temp)
                        }
                    //printfn "2 %A" z
                    z
                else
                    if expressionList =[] then 
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p expectedTypeAndMode
                    else
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p expectedTypeAndMode actualTypeAndMode
                    printf "ERROR"; voidExpression
                
            |TYPE_byte when f.function_paramlist.Length=expressionList.Length+1 ->
                let actualTypeAndMode = 
                    (TYPE_byte, RET)::(List.map2 (fun quad mode -> (quad.Place |> quadElementType.GetType , mode)) (expressionList) (parameterList))
                let expectedTypeAndMode = 
                    List.map getParameterTypeAndPassMode f.function_paramlist
                    |>Seq.take expressionList.Length
                    |>List.ofSeq
                if(List.fold2 checkParameters true expectedTypeAndMode actualTypeAndMode)
                then
                    let code = List.fold2 createParameterCode [] (List.rev expressionList) (expectedTypeAndMode
                                                                                           |>Seq.take expressionList.Length
                                                                                           |>List.ofSeq)
                    let temp = newTemporary f.function_result
                    let z = 
                        {
                            Code = QuadCall(entry)::QuadPar ( Entry(temp) , RET)::code
                            Place = Entry(temp)
                        }
                    //printfn "2 %A" z
                    z
                else
                    if expressionList =[] then 
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p expectedTypeAndMode
                    else
                        error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p expectedTypeAndMode actualTypeAndMode
                    printf "ERROR"; voidExpression  
            |TYPE_int|TYPE_byte ->
                let actualTypeAndMode = 
                    (f.function_result,RET)::(List.map2 (fun quad mode -> (quad.Place |> quadElementType.GetType , mode)) (expressionList) (parameterList))|>List.rev
                let expectedTypeAndMode = 
                    List.map getParameterTypeAndPassMode f.function_paramlist
                if expressionList =[] then 
                    error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List=%A \n\t Given empty Parameter List\n" p expectedTypeAndMode
                else
                    error "Semantic Error at %A: Parameter Mismatch:\n\t Expected Parameter List = %A \n\t Actual Parameter List = %A\n" p expectedTypeAndMode actualTypeAndMode
                printf "ERROR"; voidExpression
            |_ ->               
                error "Semantic Error at %A: Not Valid Function Type\n" p
                printf "ERROR"; voidExpression
        |_ ->
             error "Semantic Error at %A: Given name is not a Function\n" p
             printf "ERROR"; voidExpression
    |None -> 
        error "undeclared Identifier"
        printf "ERROR"; voidExpression 

//LValue
let inline GetVariableOrParameterType (entry:entry) p=
    match entry.entry_info with
    |ENTRY_variable v ->  
        v.variable_type
    |ENTRY_parameter p ->  
        p.parameter_type
    |_ ->
        error "Semantic Error at %A: The give name is neither a variable nor a parameter name\n" p 

let inline ProcessLValue1 (state:IParseState) id = 
    let p,e = FindPosAndEntry state id
    match e with
    |Some entry->
        let t = GetVariableOrParameterType entry p
        {
            Code = []
            Place = Entry(entry)
        }
    |None ->
        error "undeclared variable" |> ignore;
        printf "ERROR"; voidExpression

let inline ProcessLValue2 (state:IParseState) id =
    let p,e = FindPosAndEntry state id
    match e with
    |Some entry->
        let t = GetVariableOrParameterType entry p     
        match t with
        |TYPE_int|TYPE_byte ->
            {
                Code = []
                Place = Entry(entry)
            }
        |_ ->
            error "Semantic Error at %A: type mismatch: LValue neither Int nor Byte\n" p
            printf "ERROR"; voidExpression
    |None ->
        error "undeclared variable" |>ignore
        printf "ERROR"; voidExpression

let inline ProcessLValue3 (state:IParseState) id (index : expressionType)=
    let p,e = FindPosAndEntry state id
    let tOfIndex = (index.Place |> quadElementType.GetType)
    match e with
    |Some entry->
        let t = GetVariableOrParameterType entry p
        match t with
        |TYPE_array (typ,_) when t=TYPE_int -> 
            let temp = newTemporary typ
            {
                Code = (QuadArray(entry, index.Place, temp) :: index.Code)
                Place = Entry(temp)
            }
        |TYPE_array _ ->
            error "Semantic Error at %A: type mismatch: Invalid Array Index" p 
            printf "ERROR"; voidExpression
        |_ ->
            error "Semantic Error at %A: type mismatch: not an Array " p 
            printf "ERROR"; voidExpression
    |None -> 
        error "Semantic Error at %A: undeclared Array" p 
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
    match l.Code with
    |[] -> l
    |QuadArray(_,_,p)::_ ->
        { l with Place = Valof(p)}
    | _ -> l

let inline processFunctionCallExpression (state:IParseState) f =
    let t = (f.Place |> quadElementType.GetType)
    if (checkExprSemantics (<>) t TYPE_proc state "Semantic Error at %A -%A: void Function call\n") 
    then f else printf "ERROR"; voidExpression


let inline processExpression (state:IParseState) (op) (e1:expressionType) (e2Option:expressionType option) =
    let t1 = (e1.Place |> quadElementType.GetType)
    match e2Option with
    | None ->
        if (checkExprSemantics (=) t1 TYPE_int state "Semantic Error at %A - %A: Invalid use of Unary Operator\n") 
        then
            let temp = newTemporary t1
            { 
                Code = (op(Int(0), e1.Place, temp ) :: e1.Code)
                Place = Entry(temp)
            }
        else
            printf "ERROR"; voidExpression
    |Some e2 ->
        let t2 = (e2.Place |> quadElementType.GetType)
        if (checkExprSemantics (=) t2 t1 state "Semantic Error at %A - %A: Type Mismatch3\n") 
        then 
            let temp = newTemporary t1
            { 
                Code = (op(e1.Place, e2.Place, temp) :: (e2.Code @ e1.Code))
                Place = Entry(temp)
            }
        else
            printf "ERROR"; voidExpression

//Conditions
let inline functionize token =
    match token with
    |"EQ" -> QuadEQ
    |"NE" -> QuadNE
    |"LT" -> QuadLT
    |"GT" -> QuadGT
    |"LE" -> QuadLE
    |"GE" -> QuadGE
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "Not a valid Comparison Operand"

let inline processComparison (state:IParseState) (op) (e1:expressionType) (e2:expressionType)=
    if (checkCondSemantics (e1.Place |> quadElementType.GetType) (e2.Place |> quadElementType.GetType) 
            state "Semantic Error at %A - %A: Type Mismatch4\n")
    then
        let True, False = ref 2, ref 1
        {
            Code    = (QuadJump (False))::(op(e1.Place, e2.Place, True))::e2.Code@e1.Code
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