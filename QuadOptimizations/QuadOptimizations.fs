module QuadOptimizations
open System.Collections.Generic
open Symbol
open Types
open QuadSupport
open FinalSupport
open FinalFunctions

module UnitList = 
    let inline map f (unitList: 'a list list) =
        List.map (List.map f) unitList
    let inline mapi initValue f (unitList: 'a list list) =
        let i = ref (initValue-1)
        List.map (List.map (fun quad -> incr i; f !i quad)) unitList
    let inline iter f (unitList: 'a list list) =
        List.iter (List.iter f) unitList
    let inline iteri initValue f (unitList: 'a list list) =
        let i = ref (initValue-1)
        List.iter (List.iter (fun quad -> incr i; f !i quad)) unitList
    let inline flatten f (unitList: 'a list) =
        let rec flattenHelper nodeList currentNode acc =
            match currentNode with
            |[] -> 
                match nodeList with
                |[] ->
                    List.rev acc
                |h::t ->
                    flattenHelper t (f h) acc
            |h::t ->
                flattenHelper nodeList t (h::acc)
        flattenHelper unitList [] []

module Block =
    type blockType =
        {
            startingIndex:int;
            endingIndex:int;
            mutable quads:quadWithIndexType list;
        }
    type blockWithIdType =
        {
            id:int;
            name:int;
            block:blockType
        }
        override block.ToString() =
            "BlockName "+ block.name.ToString() + " , Id = " + block.id.ToString() 
            :: "Starts at @" + block.block.startingIndex.ToString() + " and ends at @" + block.block.endingIndex.ToString() 
            :: List.map quadWithIndexType.print block.block.quads @ [""]
            |> List.fold (fun acc str -> str + System.Environment.NewLine + acc) ""  

    type controlFlowGraphNodeType =
        {
            id:int;
            predecessors:int list;
            successors:int list;
            block:blockType;
        }
        override graphNode.ToString() =
            "GraphNodeId "+ graphNode.id.ToString()
            :: sprintf "Predecessors = %A" graphNode.predecessors
            :: sprintf "Successors = %A" graphNode.successors
            :: "Code :"
            :: List.map quadWithIndexType.print graphNode.block.quads
            |> List.fold (fun acc str -> str + System.Environment.NewLine + acc) "" 

    let blockNameHash = new Dictionary<int,int>()

    let inline makeBasicBlocksOfUnit (unit:quadWithIndexType list) =
        let blockCounter = ref -1
        let inline makeBlock (code:quadWithIndexType list) =
            incr blockCounter
            let _end = code.Head.index
            let _code = List.rev code
            let _start = _code.Head.index
            let _block =
                {
                    startingIndex = _start
                    endingIndex = _end
                    quads = _code
                }
            blockNameHash.[_start] <- !blockCounter
            {
                name = !blockCounter 
                id = _start
                block = _block
            }
        let rec makeBasicBlocksOfUnitHelper (source:quadWithIndexType list) 
            (blockAcc:quadWithIndexType list) (unitAcc:blockWithIdType list) =  
            match source,blockAcc with
            |[], _-> 
                failwith "empty source???"//List.rev unitAcc
            |[enduQuad],[] ->
                let fblock =
                    makeBlock [enduQuad]
                List.rev (fblock::unitAcc)
            |[enduQuad],_ ->
                let pblock =
                    makeBlock blockAcc
                let fblock =
                    makeBlock [enduQuad]
                List.rev (fblock::pblock::unitAcc)
            |h::t,[] ->
                makeBasicBlocksOfUnitHelper t [h] unitAcc
            |h::t,acc ->
                match h.quad with
                |_ when (labelRegistry.Contains h.index) ->
                    makeBasicBlocksOfUnitHelper t [h] (makeBlock acc :: unitAcc)
                |QuadJump _ |QuadRet _ ->
                    makeBasicBlocksOfUnitHelper t [] (makeBlock (h :: acc) :: unitAcc)
                //|QuadCall (f,lst) ->
                //    makeBasicBlocksOfUnitHelper t [] (makeBlock (h :: acc) :: unitAcc)
                |_ ->
                    makeBasicBlocksOfUnitHelper t (h::acc) unitAcc
        makeBasicBlocksOfUnitHelper unit.Tail [] [makeBlock [unit.Head]]

    let inline flattenGraphOfUnit graph =
        UnitList.flatten (fun node -> node.block.quads) graph

module List =
    let inline take n lst =
        Seq.take n lst |> Seq.toList

let inline codeCleanUp (code:quadType list list) =
    List.map (List.filter (fun quad -> 
                                match quad with
                                |QuadNone -> 
                                    false
                                |_ ->
                                    true)) code

//Some Simple Optimizations
let inline simpleBackwardPropagation (unit:quadWithIndexType list) =
    let rec simpleBackwardPropagationHelper acc (quadList:quadWithIndexType list) =
        match quadList with
        |[]->
            List.rev acc
        |[h] ->
            List.rev (h::acc)
        |h1::h2::t ->
            let inline checkAndContinue op q1 q2 e=
                match h2.quad with
                |QuadAssign(Entry e1,Entry e2) when e.Equals(e1)->
                    simpleBackwardPropagationHelper ({ index = h2.index-1; quad = op(q1,q2,e2) }::acc) t
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t) 
            match h1.quad with
            |QuadBinOperation(op,q1,q2,e) ->
                match h2.quad with
                |QuadAssign(Entry e1,Entry e2) when e.Equals(e1)->
                    simpleBackwardPropagationHelper ({ index = h2.index-1; quad = QuadBinOperation(op,q1,q2,e2) }::acc) t
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t) 
            |QuadUnOperation(op,q1,e) ->
                match h2.quad with
                |QuadAssign(Entry e1,Entry e2) when e=e1->
                    simpleBackwardPropagationHelper ({ index = h2.index-1; quad = QuadUnOperation(op,q1,e2) }::acc) t 
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t) 
            |QuadAssign(e11,e21) ->
                match h2.quad with
                |QuadAssign(e12,e22) when e21=e12->
                    simpleBackwardPropagationHelper (h1::acc) ({ index = h2.index; quad = QuadAssign(e11,e22) }::t)
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t)
            |_ ->
                simpleBackwardPropagationHelper (h1::acc) (h2::t) 
    simpleBackwardPropagationHelper [] unit

open Block
type intOpType = System.Int16->System.Int16->System.Int16
type byteOpType = System.Byte->System.Byte->System.Byte
type intCmpType = System.Int16->System.Int16->bool
type byteCmpType = System.Byte->System.Byte->bool
let constantFolding (block:blockWithIdType) =
    let byteHash = new Dictionary<entryWithTypeAndNesting,System.Byte>()
    let intHash = new Dictionary<entryWithTypeAndNesting,System.Int16>()
    let rec chooser (quads:quadWithIndexType list) acc =
        let inline tryGetIntConstant (elem:quadElementType) = 
            match elem with
            |Entry e when intHash.ContainsKey(e) ->
                Some intHash.[e]
            |Int i ->
                Some i
            |_ -> 
                None
        let inline tryGetByteConstant (elem:quadElementType) = 
            match elem with
            |Entry e when byteHash.ContainsKey(e) ->
                Some byteHash.[e]
            |Byte b ->
                Some b
            |_ -> 
                None
        match quads with
        |[] -> 
            List.rev acc
        |h::t ->
            match h.quad with
            |QuadBinOperation(op,a,b,s) ->
                match s.entryType with
                | TYPE_int ->
                    match tryGetIntConstant a, tryGetIntConstant b with
                    |Some c1,Some c2 ->
                        if (op = OpDiv || op = OpMod) && c2 = 0s
                        then 
                            h::acc
                        else 
                            let result =
                                (c1, c2) ||> 
                                (match op with
                                |OpAdd -> (+)
                                |OpSub -> (-)
                                |OpMult -> (*)
                                |OpDiv -> (/)
                                |OpMod  -> (%) )
                            intHash.[s] <- result                    
                            match s.entry.entry_info with
                            |ENTRY_variable _ |ENTRY_parameter _ ->
                                h.quad <- QuadAssign(Int result, Entry s)
                                h :: acc
                            |ENTRY_temporary _ ->
                                acc
                            | _ -> 
                                failwith "wtf"
                    |Some c, None ->
                        h.quad <- QuadBinOperation(op,Int c, b, s)
                        h :: acc
                    |None, Some c ->
                        h.quad <- QuadBinOperation(op,a, Int c, s)
                        h :: acc
                    |None, None ->
                        h ::acc
                | TYPE_byte ->
                    match tryGetByteConstant a, tryGetByteConstant b with
                    |Some c1,Some c2 ->
                        if (op = OpDiv || op = OpMod) && c2 = 0uy
                        then 
                            h::acc
                        else 
                            let result = (c1, c2) ||>  (match op with
                                                        |OpAdd -> (+)
                                                        |OpSub -> (-)
                                                        |OpMult -> (*)
                                                        |OpDiv -> (/)
                                                        |OpMod  -> (%))
                            byteHash.[s] <- result                    
                            match s.entry.entry_info with
                            |ENTRY_variable _ |ENTRY_parameter _ ->
                                h.quad <- QuadAssign(Byte result, Entry s)
                                h :: acc
                            |ENTRY_temporary _ ->
                                acc
                            | _ -> 
                                failwith "wtf"
                    |Some c, None ->
                        h.quad <- QuadBinOperation(op,Byte c, b, s)
                        h :: acc
                    |None, Some c ->
                        h.quad <- QuadBinOperation(op,a, Byte c, s)
                        h :: acc
                    |None, None ->
                        h ::acc
                | _ -> failwith "wtf"
            |QuadComparison(comp,a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    match tryGetIntConstant a, tryGetIntConstant b with
                    |Some c1,Some c2 ->
                        if (match comp with
                            |CompEQ -> (=)
                            |CompNE -> (<>)
                            |CompLT -> (<)
                            |CompGT -> (>)
                            |CompLE -> (<=)
                            |CompGE -> (>=)) c1 c2
                        then
                            acc
                        else
                            h.quad <- QuadJump(i2) 
                            h :: acc
                    |Some c, None ->
                        h.quad <- QuadComparison(comp, Int c, b, i1, i2)
                        h :: acc
                    |None, Some c ->
                        h.quad <- QuadComparison(comp, a, Int c, i1, i2)
                        h :: acc
                    |None, None ->
                        h ::acc
                |TYPE_byte ->
                    match tryGetByteConstant a, tryGetByteConstant b with
                    |Some c1,Some c2 ->
                        if (match comp with
                            |CompEQ -> (=)
                            |CompNE -> (<>)
                            |CompLT -> (<)
                            |CompGT -> (>)
                            |CompLE -> (<=)
                            |CompGE -> (>=)) c1 c2
                        then
                            acc
                        else
                            h.quad <- QuadJump(i2) 
                            h :: acc
                    |Some c, None ->
                        h.quad <- QuadComparison(comp, Byte c, b, i1, i2)
                        h :: acc
                    |None, Some c ->
                        h.quad <- QuadComparison(comp, a, Byte c, i1, i2)
                        h :: acc
                    |None, None ->
                        h ::acc
                |_ -> failwith "wtf"
            |QuadArray (ar,index,s) ->
                match tryGetIntConstant index with
                |Some c ->
                    h.quad <- QuadArray(ar,Int c,s)
                |None ->
                    ()
                h :: acc
            |QuadAssign (a,s) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    match tryGetIntConstant a with
                    |Some c ->
                        h.quad <- QuadAssign(Int c,s)
                    |None ->
                        ()
                |TYPE_byte ->
                    match tryGetByteConstant a with
                    |Some c ->
                        h.quad <- QuadAssign(Byte c,s)
                    |None ->
                        ()
                |_-> failwith "wtf"
                h :: acc
            |QuadPar(e,m) ->
                if m=PASS_BY_VALUE then
                    match quadElementType.GetType e with
                    |TYPE_int ->
                        match tryGetIntConstant e with
                        |Some c ->
                            h.quad <- QuadPar(Int c,m)
                        |None ->
                            ()
                    |TYPE_byte ->
                        match tryGetByteConstant e with
                        |Some c ->
                            h.quad <- QuadPar(Byte c,m)
                        |None ->
                            ()
                    |_ -> ()
                h :: acc
            |QuadCall(f,pars)->
                let (ENTRY_function finf) = f.entry.entry_info
                if finf.function_mutatesForeignBytes then
                    byteHash.Clear()
                elif finf.function_mutatesForeignInts then
                    intHash.Clear()
                else Seq.iter (fun (it:entryWithTypeAndNesting) ->
                                if it.entryType = TYPE_byte then byteHash.Remove it |>ignore
                                elif it.entryType = TYPE_int then intHash.Remove it |>ignore) pars
                h :: acc
            | _ ->
                h :: acc
            |> chooser t
    block.block.quads <- chooser block.block.quads []
    block

let shortenJumpPaths (unitList: blockWithIdType list) =    
    let unitArray = List.toArray unitList
    let rec dfs node acc=
        let fstQuad =unitArray.[node].block.quads.Head
        match fstQuad.quad with
        |QuadJump x ->
            dfs blockNameHash.[!x] (x::acc)
        |_ ->
            List.iter (fun l -> l:= fstQuad.index) acc
    List.iter (fun (block:blockWithIdType) -> 
                let quads = block.block.quads
                match quads.[quads.Length - 1].quad with
                |QuadJump x ->
                    dfs blockNameHash.[!x] [x]
                |QuadComparison(_,_,_,l1,l2) ->
                    dfs blockNameHash.[!l1] [l1]
                    dfs blockNameHash.[!l2] [l2]
                | _ ->
                    ()) unitList
    unitList

//Control Flow Analysis
let makeControlFlowGraphOfUnit (unit:blockWithIdType list) =
    let childrenArray = Array.create unit.Length []
    let parentsArray = Array.create unit.Length []
    let inline func (code:blockWithIdType) =  
        let index = code.name
        let inline getChildren q =
            if parentsArray.[index]<>[] then 
                match q.quad with
                |QuadComparison(_,_,_,l1,l2) ->
                    let childIndex1 = blockNameHash.[!l1]
                    let childIndex2 = blockNameHash.[!l2]
                    childrenArray.[index] <- [childIndex1; childIndex2] 
                    parentsArray.[childIndex1] <- index :: parentsArray.[childIndex1]
                    parentsArray.[childIndex2] <- index :: parentsArray.[childIndex2]
                |QuadJump(l) ->
                    let childIndex = blockNameHash.[!l]
                    childrenArray.[index] <- childIndex :: childrenArray.[index]
                    parentsArray.[childIndex] <- index :: parentsArray.[childIndex]
    //Optimize junk block elimination
                |QuadRet _ -> 
                    childrenArray.[index] <- unit.Length - 1 :: childrenArray.[index]
                    parentsArray.[unit.Length - 1] <- index :: parentsArray.[unit.Length - 1]
                |QuadEndUnit _ -> 
                    childrenArray.[index] <- [-1]
                |_ ->
                    childrenArray.[index] <- index + 1 :: childrenArray.[index]
                    parentsArray.[index+1] <- index :: parentsArray.[index+1]
        match List.rev code.block.quads with
        |[] ->
            failwith "empty block???"
        |h::t ->
            getChildren h
    parentsArray.[0] <- [-1]
    List.iter func unit
    let rec helper i (cBlock:blockWithIdType list) acc =
        match cBlock with
        |[] ->
            List.rev acc
        |h::t ->
//Junk block elimination optimizing version
            if parentsArray.[h.name] <> [] then
                //printfn "id=%d, parents = %A, children = %A" i parentsArray.[i] childrenArray.[i]
                match acc with
                | hacc::tacc ->
                    let previousCode = hacc.block.quads |> List.toArray
                    if previousCode.[previousCode.Length - 1].quad = QuadJump (ref h.id) then
                        hacc.block.quads <- [for i=0 to (previousCode.Length - 2) do yield previousCode.[i]]
                | _ -> ()
                helper (i+1) t ({ 
                                    id = i; 
                                    predecessors = parentsArray.[i]; 
                                    successors = childrenArray.[i]; 
                                    block = h.block 
                                }::acc)
            else
                helper (i+1) t acc
    helper 0 unit []

let optimizeIntermediate = 
    List.map ( simpleBackwardPropagation 
            >> makeBasicBlocksOfUnit
            >> List.map (constantFolding) 
            >> shortenJumpPaths 
            >> makeControlFlowGraphOfUnit 
            >> flattenGraphOfUnit)
