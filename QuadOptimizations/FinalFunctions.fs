﻿module FinalFunctions
open System.Collections.Generic
open QuadSupport
open FinalSupport
open Types
open Error
open Symbol

let stringRegistry = new Dictionary<string,string list*string> ()
let cnt=ref 1
let inline addString (lst,s) =
    if (stringRegistry.ContainsKey s) then
        stringRegistry.[s] |> snd
    else
        let str = "@str_"+ (string !cnt)
        stringRegistry.[s] <- (lst,str); incr cnt
        str

let labelRegistry = new HashSet<int> ()

let localFunctionRegistry = new Stack<string>()

let usedLibraryFunctionRegistry = new HashSet<string>() 

let inline getNestingDiff e = e.usageNest - e.entry.entry_scope.sco_nesting

let inline localSize (e:entry) =
    match e.entry_info with
    |ENTRY_function f -> f.function_negoffs |>int16
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "not a function\n"
let inline parameterSize (e:entry) =
    match e.entry_info with
    |ENTRY_function f -> f.function_posoffs |>int16
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "not a function\n"
let inline getAR (n:int)=
    (Mov (Register SI, Indirect (BP,"word",4)))::(List.replicate (n-1) (Mov (Register SI, Indirect (SI,"word",4))))

let inline updateAL n=
    match n with
    |_ when n<0 ->
       [Push (Register BP)]
    |_ when n=0 ->
       [Push (Indirect (BP,"word",4))]
    |_ ->
        (getAR n)@[Push (Indirect (SI,"word",2))]    
let rec load R a = 
    match a with
    |QNone -> []
    |Int(c) -> 
        [Mov (Register R, Const c)]
    |Byte(c) ->
        [Mov (Register R, Const (int16 c))]
    |String(lst,str)->    
        internal_error (__SOURCE_FILE__,__LINE__) "Cannot Load a String in a register"
    |Entry e -> 
        let (size, offset, mode) = entry.GetSizeOffsetMode e.entry
        match (getNestingDiff e, mode) with
        |(0,PASS_BY_REFERENCE)|(0,RET) ->
            [
                Mov (Register SI,Indirect (BP,"word",offset));
                Mov (Register R, Indirect (SI,size,0))
            ]
        |(0,PASS_BY_VALUE) ->
            [
                Mov (Register R,Indirect (BP,size,offset))
            ]
        |(n,PASS_BY_REFERENCE)|(n,RET) ->
            (getAR <| getNestingDiff e) @ [
                                            Mov (Register SI, Indirect (SI,"word",offset));
                                            Mov (Register R, Indirect (SI,size,0))
                                        ]
        |(n, PASS_BY_VALUE) ->
            (getAR <| getNestingDiff e) @ [ 
                                            Mov (Register R, Indirect (SI,size,offset))
                                        ]
    |Valof e ->
        let size = sizeof e.entryType
        List.concat [  
                        load DI (Entry e);
                        [Mov (Register R, Indirect (DI,size,0))]
                    ]

let inline loadAddr R a =
    match a with
    |Entry(e) ->
        let (size, offset, mode) = entry.GetSizeOffsetMode e.entry
        match (getNestingDiff e, mode) with
        |(0,PASS_BY_REFERENCE)|(0,RET) ->
            [Mov (Register R,Indirect (BP,"word",offset))]
        |(0,PASS_BY_VALUE) ->
            [Lea (Register R, Indirect (BP,size,offset))]
        |(n,PASS_BY_REFERENCE)|(n,RET) ->
            (getAR <| getNestingDiff e)@[Mov (Register R, Indirect (SI,size,offset))]
        |(n, PASS_BY_VALUE) ->
            (getAR <| getNestingDiff e)@[Lea (Register R, Indirect (SI,"word",offset))]
    |Valof e ->
        load R (Entry e)
    |String (l,s) ->
        [Lea (Register R,Identifier ("byte ptr " + addString (l,s)))]
    | _ -> internal_error (__SOURCE_FILE__,__LINE__) "invalid a"

let inline store R a =
    match a with
    |Entry(e) ->
        let (size, offset, mode) = entry.GetSizeOffsetMode e.entry
        match (getNestingDiff e, mode) with
        |(0,PASS_BY_REFERENCE)|(0,RET) ->
            [   
                Mov (Register SI,Indirect (BP,"word",offset));
                Mov (Indirect (SI,size,0), Register R)  
            ]
        |(0,PASS_BY_VALUE) ->
            [   
                Mov (Indirect (BP,size,offset),Register R)
            ]
        |(n,PASS_BY_REFERENCE)|(n,RET) ->
            (getAR <| getNestingDiff e) @  [ 
                                Mov (Register SI, Indirect (SI,"word",offset));
                                Mov (Indirect (SI,size,0),Register R)
                            ]
        |(n, PASS_BY_VALUE) ->
            (getAR <| getNestingDiff e) @  [ 
                                Mov (Indirect (SI,size,offset),Register R)
                            ]
    |Valof e ->
        let size = sizeof e.entryType
        List.concat [  
                        load DI (Entry e);
                        [Mov (Indirect (DI,size,0), Register R)]
                    ]
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "cannot store to a constant"
   
let inline QuadtoFinal (item:quadWithIndexType) (labelRegistry:HashSet<int>) = 
    let inline chooseRegister a b =
        match quadElementType.GetType a with
        |TYPE_int ->(AX,CX)
        |TYPE_byte ->(AL,CL)
        |_ -> internal_error (__SOURCE_FILE__,__LINE__) "Neither int nor byte"
    let final=
        match item.quad with
        |QuadNone -> [ Label "; optimized and removed" ]
        |QuadUnit s ->
            let uid = "_" + entry.GetUniqueID s
            localFunctionRegistry.Push uid
            [   Proc uid;
                Push (Register BP);
                Mov (Register BP,Register SP);
                Sub (Register SP,Const (localSize s))]
        |QuadEndUnit s->
            let uid = entry.GetUniqueID s
            [   Label (sprintf "@%s :" uid);
                Mov (Register SP,Register BP);
                Pop (BP);
                Ret;
                Endp (sprintf "_%s" uid)]
        |QuadBinOperation(op,a,b,e) ->
            match op with
            |OpAdd ->
                let ar,br = chooseRegister a b
                List.concat [
                                load ar a ; load br b;
                                Add (Register ar, Register br)::(store ar (Entry e))
                            ]
            |OpSub ->
                let ar,br = chooseRegister a b
                List.concat [
                                load ar a ; load br b;
                                Sub (Register ar, Register br)::(store ar (Entry e))
                            ]
            |OpMult ->
                    let ar,br = chooseRegister a b
                (*let inline isPowerOfTwo x zero one = (x > one) && (x &&& (x - one)) = zero
                let inline findPower x zero one =
                    let rec power x acc = 
                        let x' = x >>> 1
                        if x' = one then acc else power x' (acc + 1)
                    power x 1
                let ar,br = chooseRegister a b
                match a, b with
                |Int x, c | c, Int x when x |> isPowerOfTwo 0s 1s -> 
                    findPower x 0s 1s
                    List.concat [
                                    load AX c ;
                                    Shl(AX,) ::(store ar (Entry e))
                                ]
                |Byte x, _ | _, Byte x when x |> isPowerOfTwo 0uy 1uy->
                    findPower x 0uy 1uy
                    List.concat [
                                    load ar a ; load br b;
                                    IMul br ::(store ar (Entry e))
                                ]
                | _ ->*)                    
                    List.concat [
                                    load ar a ; load br b;
                                    IMul br ::(store ar (Entry e))
                                ]
            |OpDiv ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    List.concat [
                                    load AX a ; Cwd ::(load CX b);
                                    IDiv CX ::(store AX (Entry e))
                                ]
                |TYPE_byte ->
                     List.concat [
                                    load AL a ; Cbw ::(load CL b);
                                    IDiv CL ::(store AL (Entry e))
                                ]
                |_ -> internal_error (__SOURCE_FILE__,__LINE__) "Neither int nor byte"
            |OpMod ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    List.concat [
                                    load AX a ; Cwd ::(load CX b);
                                    IDiv CX ::(store DX (Entry e))
                                ]
                |TYPE_byte ->
                     List.concat [
                                    load AL a ; Cbw ::(load CL b);
                                    IDiv CL ::(store AH (Entry e))
                                ]
                |_ -> internal_error (__SOURCE_FILE__,__LINE__) "Neither int nor byte"
        |QuadUnOperation(op,a,e) ->
            let ar,_ = chooseRegister a QNone
            match op with
            |OpPos ->
                (load ar a)@(Pos ar ::(store ar (Entry e)))
            |OpNeg ->
                (load ar a)@(Neg ar ::(store ar (Entry e)))
        |QuadAssign(a,e) ->
            let ar,_ = chooseRegister a QNone
            (load ar a)@(store ar e)
        |QuadArray(e1, a, e2) ->
            let size =
                match e1.entryType with
                |TYPE_array (t,s) -> sizeOfType t
                |_ -> internal_error (__SOURCE_FILE__,__LINE__) "not an array"
            List.concat [
                            load AX (a);
                            [Mov (Register CX, Const size);IMul CX];
                            loadAddr CX (Entry(e1));
                            [Add (Register AX,Register CX)];
                            store AX (Entry (e2))
                        ]
        |QuadComparison(comp,a,b,i1,i2) ->
            let ar,br = chooseRegister a b
            List.concat [
                                load ar a ; load br b;
                                [Cmp (ar, br); Cond (finalCompType.ofQuadCompType comp, sprintf "@label%d" !i1)]
                                [Jump (sprintf "@label%d" !i2)]
                        ]
        |QuadJump i ->
            [Jump (sprintf "@label%d" !i)]
        |QuadCall (f,_) ->
            let fn = "_" + entry.GetUniqueID f.entry
            if localFunctionRegistry.Contains fn |> not then usedLibraryFunctionRegistry.Add fn |>ignore
            let temp=
                (updateAL <| getNestingDiff f) @
                [Call fn; Add (Register SP, Const  (parameterSize f.entry + 4s))]
            if f.entryType =TYPE_proc then (Sub (Register SP,Const 2s))::temp else temp
        |QuadPar(a, mode) ->
            match (quadElementType.GetType a) with
            |TYPE_int when mode = PASS_BY_VALUE ->
                (load AX a) @ [ Push (Register AX) ]
            |TYPE_byte when mode = PASS_BY_VALUE ->
                (load AL a) @ [ Sub (Register SP, Const 1s);
                                Mov (Register SI, Register SP);
                                Mov (Indirect(SI,"byte",0), Register AL)]
            | _ when (mode = PASS_BY_REFERENCE || mode=RET)  ->
                (loadAddr SI a) @ [Push (Register SI)]
            | _ ->
                internal_error (__SOURCE_FILE__,__LINE__) "invalid parameter"
        |QuadRet f -> 
            [Jump ("@" + entry.GetUniqueID f)]
    if labelRegistry.Contains (item.index) then (Label (sprintf "@label%d :" (item.index))::final) else final
    |>fun x ->
        Label (quadType.print item.quad|>sprintf ";@%d:\t\"%s\"" (item.index) ) :: x

let inline codeHead ()=
    "xseg segment public 'code'" + System.Environment.NewLine
  + "     assume cs : xseg, ds : xseg, ss : xseg" + System.Environment.NewLine
  + "     org 100h" + System.Environment.NewLine
  + "main proc near" + System.Environment.NewLine
  + "     call near ptr "+ localFunctionRegistry.Pop () + System.Environment.NewLine 
  + "     mov ax, 4C00h" + System.Environment.NewLine
  + "     int 21h" + System.Environment.NewLine
  + "main endp" + System.Environment.NewLine 

let inline codeTail () =
    let inline aux registry func= [for i in registry -> func i]
    let externalFunctions = aux usedLibraryFunctionRegistry (fun str -> sprintf "extrn %s : proc" str)
    let strings = aux stringRegistry (fun pair -> 
                                        let s = (new System.Text.StringBuilder("; ",40)).AppendLine(pair.Key)
                                                                                        .Append(snd pair.Value)
                                                                                        .Append(" db ").Append((fst pair.Value)
                                                                                        .Head.ToString ())
                                        List.fold (fun (acc : System.Text.StringBuilder) cur -> acc.Append(", ")
                                                                                                   .Append(cur.ToString())) s (fst pair.Value).Tail
                                        |>fun x -> x.ToString () + ", 0" + System.Environment.NewLine )
    List.concat [externalFunctions ; strings ; ["xseg ends\n     end  main\n"]]

