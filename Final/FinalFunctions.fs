module FinalFunctions
open System.Collections.Generic
open QuadSupport
open FinalSupport
open Types
open Error
open Symbol

let stringRegistry = new Dictionary<string,string list*string> ()
let cnt=ref 0
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

let inline localSize (e:entry) =
    match e.entry_info with
    |ENTRY_function f -> f.function_negoffs
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "not a function\n"
let inline parameterSize (e:entry) =
    match e.entry_info with
    |ENTRY_function f -> f.function_posoffs
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
        [Mov (Register R, Const (int c))]
    |String(lst,str)->    
        internal_error (__SOURCE_FILE__,__LINE__) "Cannot Load a String in a register"
    |Entry(ent,typ,nest) -> 
        let (size, offset, mode) = entry.GetSizeOffsetMode ent
        match (nest, mode) with
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
            (getAR nest) @  [
                                Mov (Register SI, Indirect (SI,"word",offset));
                                Mov (Register R, Indirect (SI,size,0))
                            ]
        |(n, PASS_BY_VALUE) ->
            (getAR nest) @  [ 
                                Mov (Register R, Indirect (SI,size,offset))
                            ]
    |Valof (ent,typ,nest) ->
        let size = sizeof typ
        List.concat [  
                        load DI (Entry (ent,typ,nest));
                        [Mov (Register R, Indirect (DI,size,0))]
                    ]

let inline loadAddr R a =
    match a with
    |Entry(ent,typ,nest) ->
        let (size, offset, mode) = entry.GetSizeOffsetMode ent
        match (nest, mode) with
        |(0,PASS_BY_REFERENCE)|(0,RET) ->
            [Mov (Register R,Indirect (BP,"word",offset))]
        |(0,PASS_BY_VALUE) ->
            [Lea (Register R, Indirect (BP,size,offset))]
        |(n,PASS_BY_REFERENCE)|(n,RET) ->
            (getAR nest)@[Mov (Register R, Indirect (SI,size,offset))]
        |(n, PASS_BY_VALUE) ->
            (getAR nest)@[Lea (Register R, Indirect (SI,"word",offset))]
    |Valof (e,typ,nest) ->
        load R (Entry (e,typ,nest))
    |String (l,s) ->
        [Lea (Register R,Identifier ("byte ptr " + addString (l,s)))]
    | _ -> internal_error (__SOURCE_FILE__,__LINE__) "invalid a"

let inline store R a =
    match a with
    |Entry(ent,typ,nest) ->
        let (size, offset, mode) = entry.GetSizeOffsetMode ent
        match (nest, mode) with
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
            (getAR nest) @  [ 
                                Mov (Register SI, Indirect (SI,"word",offset));
                                Mov (Indirect (SI,size,0),Register R)
                            ]
        |(n, PASS_BY_VALUE) ->
            (getAR nest) @  [ 
                                Mov (Indirect (SI,size,offset),Register R)
                            ]
    |Valof (ent,typ,nest) ->
        let size = sizeof typ
        List.concat [  
                        load DI (Entry (ent,typ,nest));
                        [Mov (Indirect (DI,size,0), Register R)]
                    ]
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "cannot store to a constant"
   
let inline QuadtoFinal (i:int,quad:quadType) (labelRegistry:HashSet<int>) = 
    //(quadType.print quad|>printfn "\r@%d: \"%s\":" i )
    let inline chooseRegister a b =
        match quadElementType.GetType a with
        |TYPE_int ->(AX,CX)
        |TYPE_byte ->(AL,CL)
        |_ -> internal_error (__SOURCE_FILE__,__LINE__) "Neither int nor byte"
    let inline Condition a b i action =
        let ar,br = chooseRegister a b
        List.concat [
                            load ar a ; load br b;
                            [Cmp (ar, br); Cond (action, sprintf "@label%d" !i)]
                    ]
    let final=
        match quad with
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
        |QuadAdd(a,b,e) ->
            let ar,br = chooseRegister a b
            List.concat [
                            load ar a ; load br b;
                            Add (Register ar, Register br)::(store ar (Entry e))
                        ]
        |QuadSub(a,b,e) ->
            let ar,br = chooseRegister a b
            List.concat [
                            load ar a ; load br b;
                            Sub (Register ar, Register br)::(store ar (Entry e))
                        ]
        |QuadNeg(a,e) ->
            let ar,_ = chooseRegister a QNone
            (load ar a)@(Neg ar ::(store ar (Entry e)))
        |QuadMult(a,b,e) ->
            let ar,br = chooseRegister a b
            List.concat [
                            load ar a ; load br b;
                            IMul br ::(store ar (Entry e))
                        ]
        |QuadDiv(a,b,e) ->
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
        |QuadMod(a,b,e) ->
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
        |QuadAssign(a,e) ->
            let ar,_ = chooseRegister a QNone
            (load ar a)@(store ar e)
        |QuadArray((e1,t1,n1), a, (e2,t2,n2)) ->
            let size =
                match t1 with
                |TYPE_array (t,s) -> sizeOfType t
                |_ -> internal_error (__SOURCE_FILE__,__LINE__) "not an array"
            List.concat [
                            load AX (a);
                            [Mov (Register CX, Const size);IMul CX];
                            loadAddr CX (Entry(e1,t1,n1));
                            [Add (Register AX,Register CX)];
                            store AX (Entry (e2,t2,n2))
                        ]
        |QuadEQ(a,b,i) ->
            Condition a b i "je "
        |QuadNE(a,b,i) ->
            Condition a b i "jne "
        |QuadLT(a,b,i) ->
            Condition a b i "jl "
        |QuadGT(a,b,i) ->
            Condition a b i "jg "
        |QuadGE(a,b,i) ->
            Condition a b i "jge "
        |QuadLE(a,b,i) ->
            Condition a b i "jle "
        |QuadJump i ->
            [Jump (sprintf "@label%d" !i)]
        |QuadCall (e,t,n) ->
            let fn = "_" + entry.GetUniqueID e
            if localFunctionRegistry.Contains fn |> not then usedLibraryFunctionRegistry.Add fn |>ignore
            let temp=
                (updateAL n) @
                [Call fn; Add (Register SP, Const  (parameterSize e + 4))]
            if t =TYPE_proc then (Sub (Register SP,Const 2))::temp else temp
        |QuadPar(a, mode) ->
            match (quadElementType.GetType a) with
            |TYPE_int when mode = PASS_BY_VALUE ->
                (load AX a) @ [ Push (Register AX) ]
            |TYPE_byte when mode = PASS_BY_VALUE ->
                (load AL a) @ [ Sub (Register SP, Const 1);
                                Mov (Register SI, Register SP);
                                Mov (Indirect(SI,"byte",0), Register AL)]
            | _ when (mode = PASS_BY_REFERENCE || mode=RET)  ->
                (loadAddr SI a) @ [Push (Register SI)]
            | _ ->
                internal_error (__SOURCE_FILE__,__LINE__) "invalid parameter"
        |QuadRet f -> 
            [Jump ("@" + entry.GetUniqueID f)]

    if labelRegistry.Contains i then (Label (sprintf "@label%d :" (i))::final) else final
    |>fun x ->
        Label (quadType.print (i,quad)|>sprintf ";@%d: \"%s\"" (i) ) :: x

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

