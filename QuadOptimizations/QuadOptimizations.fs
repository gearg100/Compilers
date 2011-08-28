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
type controlFlowGraphNodeType =
    {
        id:int;
        predecessors:int list;
        successors:int list;
        block:blockType;
    }

let inline codeCleanUp (code:quadType list list) =
    List.map (List.filter (fun quad -> 
                                match quad with
                                |QuadNone -> 
                                    false
                                |_ ->
                                    true)) code

let blockNameHash = new Dictionary<int,int>();
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
            |QuadJump _ |QuadRet _ ->
                makeBasicBlocksOfUnitHelper t [] (makeBlock (h::acc) :: unitAcc)
            |QuadCall _ ->
                makeBasicBlocksOfUnitHelper t [] (makeBlock (h :: acc) :: unitAcc)
            |_ when (labelRegistry.Contains h.index) ->
                makeBasicBlocksOfUnitHelper t [h] (makeBlock acc :: unitAcc)
            |_ ->
                makeBasicBlocksOfUnitHelper t (h::acc) unitAcc
    makeBasicBlocksOfUnitHelper unit.Tail [] [makeBlock [unit.Head]]

let inline printBlock (block:blockWithIdType) =
   (fun (block:blockWithIdType) -> 
                    "BlockName "+ block.name.ToString() + " , Id = " + block.id.ToString() 
                    :: "Starts at @" + block.block.startingIndex.ToString() + " and ends at @" + block.block.endingIndex.ToString() 
                    :: List.map quadWithIndexType.print block.block.quads @ [""])
                block
//Some Simple Optimizations
let inline simpleBackwardPropagation (code:quadWithIndexType list list) =
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
            |QuadAdd(q1,q2,e) ->
                checkAndContinue QuadSub q1 q2 e
            |QuadSub(q1,q2,e) ->
                checkAndContinue QuadSub q1 q2 e
            |QuadMult(q1,q2,e) ->
                checkAndContinue QuadMult q1 q2 e
            |QuadDiv(q1,q2,e) ->
                checkAndContinue QuadDiv q1 q2 e
            |QuadMod(q1,q2,e) ->
                checkAndContinue QuadMod q1 q2 e
            |QuadNeg(q1,e) ->
                match h2.quad with
                |QuadAssign(Entry e1,Entry e2) when e=e1->
                    simpleBackwardPropagationHelper ({ index = h2.index-1; quad = QuadNeg(q1,e2) }::acc) t 
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t) 
            |QuadAssign(e11,e21) ->
                match h2.quad with
                |QuadAssign(e12,e22) when e21=e12->
                    simpleBackwardPropagationHelper ({ index = h2.index-1; quad = QuadAssign(e11,e22) }::acc) t
                |_ -> 
                    simpleBackwardPropagationHelper (h1::acc) (h2::t) 
            |_ ->
                simpleBackwardPropagationHelper (h1::acc) (h2::t) 
    List.map (simpleBackwardPropagationHelper []) code

type intOpType = System.Int16->System.Int16->System.Int16
type byteOpType = System.Byte->System.Byte->System.Byte
type intCmpType = System.Int16->System.Int16->bool
type byteCmpType = System.Byte->System.Byte->bool
let constantFolding (block:blockWithIdType) =
    let byteHash = new Dictionary<Hashcons.hash_consed<string>,System.Byte>()
    let intHash = new Dictionary<Hashcons.hash_consed<string>,System.Int16>()
    let rec chooser (quads:quadWithIndexType list) acc =
        let inline tryGetIntConstant (elem:quadElementType) = 
            match elem with
            |Entry (e,_,_) when intHash.ContainsKey(e.entry_id) ->
                Some intHash.[e.entry_id]
            |Int i ->
                Some i
            |_ -> 
                None
        let inline tryGetByteConstant (elem:quadElementType) = 
            match elem with
            |Entry (e,_,_) when byteHash.ContainsKey(e.entry_id) ->
                Some byteHash.[e.entry_id]
            |Byte b ->
                Some b
            |_ -> 
                None
        match quads with
        |[] -> 
            List.rev acc
        |h::t ->
            let inline actOp constr operand (a:quadElementType) (b:quadElementType) (s:entryWithTypeAndNesting) elemConstr tryGetConstant (hashTable:Dictionary<_,_>) =
                match tryGetConstant a, tryGetConstant b with
                |Some c1,Some c2 ->
                    let result = operand c1 c2
                    let (e,_,_) = s
                    hashTable.[e.entry_id] <- result                    
                    match e.entry_info with
                    |ENTRY_variable _ |ENTRY_parameter _ ->
                        h.quad <- QuadAssign(elemConstr result, Entry s)
                        h :: acc
                    |ENTRY_temporary _ ->
                        acc
                    | _ -> 
                        failwith "wtf"
                |Some c, None ->
                    h.quad <-constr(elemConstr c, b, s)
                    h :: acc
                |None, Some c ->
                    h.quad <- constr(a, elemConstr c, s)
                    h :: acc
                |None, None ->
                    h ::acc
            let inline actDivMod constr operand (a:quadElementType) (b:quadElementType) (s:entryWithTypeAndNesting) elemConstr
                tryGetConstant (hashTable:Dictionary<_,_>) zero=
                match tryGetConstant a, tryGetConstant b with
                |_ ,Some x when x = zero ->
                    h ::acc
                |Some c1,Some c2 ->
                    let result = operand c1 c2
                    let (e,_,_) = s
                    hashTable.[e.entry_id] <- result                    
                    match e.entry_info with
                    |ENTRY_variable _ |ENTRY_parameter _ ->
                        h.quad <- QuadAssign(elemConstr result, Entry s)
                        h :: acc
                    |ENTRY_temporary _ ->
                        acc
                    | _ -> 
                        failwith "wtf"
                |Some c, None ->
                    h.quad <- constr(elemConstr c, b, s)
                    h :: acc
                |None, Some c ->
                    h.quad <- constr(a, elemConstr c, s)
                    h :: acc
                |None, None ->
                    h :: acc
            let inline actCmp constr operand (a:quadElementType) (b:quadElementType) (i1:int ref) (i2:int ref) elemConstr
                tryGetConstant =
                match tryGetConstant a, tryGetConstant b with
                |Some c1,Some c2 ->
                    if operand c1 c2 
                    then
                        h.quad <- QuadJump(i1) 
                        h :: acc
                    else
                        acc
                |Some c, None ->
                    h.quad <- constr(elemConstr c, b, i1, i2)
                    h :: acc
                |None, Some c ->
                    h.quad <- constr(a, elemConstr c, i1, i2)
                    h :: acc
                |None, None ->
                    h ::acc
            match h.quad with
            |QuadAdd(a,b,s) -> 
                let (_,t,_) = s
                match t with
                | TYPE_int ->
                    actOp QuadAdd ((+):intOpType) a b s Int tryGetIntConstant intHash 
                | TYPE_byte ->
                    actOp QuadAdd ((+):byteOpType) a b s Byte tryGetByteConstant byteHash
                |_-> failwith "wtf" 
            |QuadSub(a,b,s) ->
                let (_,t,_) = s
                match t with
                |TYPE_int ->
                    actOp QuadAdd ((-):intOpType) a b s Int tryGetIntConstant intHash 
                |TYPE_byte ->
                    actOp QuadAdd ((-):byteOpType) a b s Byte tryGetByteConstant byteHash
                |_-> failwith "wtf" 
            |QuadMult(a,b,s) ->
                let (_,t,_) = s
                match t with
                |TYPE_int ->
                    actOp QuadAdd ((-):intOpType) a b s Int tryGetIntConstant intHash 
                |TYPE_byte ->
                    actOp QuadAdd ((-):byteOpType) a b s Byte tryGetByteConstant byteHash
                |_-> failwith "wtf" 
            |QuadDiv(a,b,s) ->
                let (_,t,_) = s
                match t with
                |TYPE_int ->
                    actDivMod QuadAdd ((-):intOpType) a b s Int tryGetIntConstant intHash 0s
                |TYPE_byte ->
                    actDivMod QuadAdd ((-):byteOpType) a b s Byte tryGetByteConstant byteHash 0uy
                |_-> failwith "wtf" 
            |QuadMod(a,b,s) ->
                let (_,t,_) = s
                match t with
                |TYPE_int ->
                    actDivMod QuadAdd ((-):intOpType) a b s Int tryGetIntConstant intHash 0s
                |TYPE_byte ->
                    actDivMod QuadAdd ((-):byteOpType) a b s Byte tryGetByteConstant byteHash 0uy
                |_-> failwith "wtf" 
            |QuadEQ(a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadEQ ((=):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadEQ ((=):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadNE (a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadNE ((<>):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadNE ((<>):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadGE (a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadNE ((>=):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadNE ((>=):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadLE (a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadNE ((<=):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadNE ((<=):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadGT (a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadNE ((>):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadNE ((>):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadLT (a,b,i1,i2) ->
                match quadElementType.GetType a with
                |TYPE_int ->
                    actCmp QuadNE ((<):intCmpType) a b i1 i2 Int tryGetIntConstant
                |TYPE_byte ->
                    actCmp QuadNE ((<):byteCmpType) a b i1 i2 Byte tryGetByteConstant
                |_-> failwith "wtf" 
            |QuadArray (ar,index,s) ->
                match tryGetIntConstant index with
                |Some c ->
                    h.quad <- QuadArray(ar,Int c,s)
                |None ->
                    ()
                h :: acc
            |QuadAssign (a,s) ->
                match quadElementType.GetType s with
                |TYPE_int ->
                    match tryGetIntConstant s with
                    |Some c ->
                        h.quad <- QuadAssign(a,Int c)
                    |None ->
                        ()
                |TYPE_byte ->
                    match tryGetByteConstant s with
                    |Some c ->
                        h.quad <- QuadAssign(a,Byte c)
                    |None ->
                        ()
                |_-> failwith "wtf"
                h :: acc
            | _ ->
                h :: acc
            |> chooser t
    block.block.quads <- chooser block.block.quads []
    block
//Control Flow Analysis
let makeControlFlowGraphOfUnit (unit:blockWithIdType list) =
    let childrenArray = Array.create unit.Length []
    let parentsArray = Array.create unit.Length []
    let inline func (code:blockWithIdType) =  
        let index = code.name
        let inline getChildren q =
            match q.quad with
            |QuadEQ (_,_,l1,l2)|QuadNE (_,_,l1,l2)|QuadLT (_,_,l1,l2)
            |QuadGT (_,_,l1,l2)|QuadGE (_,_,l1,l2)|QuadLE (_,_,l1,l2) ->
                let childIndex1 = blockNameHash.[!l1]
                let childIndex2 = blockNameHash.[!l2]
                childrenArray.[index] <- [childIndex1;childIndex2] 
                parentsArray.[childIndex1] <- index :: parentsArray.[childIndex1]
                parentsArray.[childIndex2] <- index :: parentsArray.[childIndex2]
            |QuadJump(l) ->
                let childIndex = blockNameHash.[!l]
                childrenArray.[index] <- childIndex :: childrenArray.[index]
//Optimize junk block elimination
                if parentsArray.[childIndex] <> [] then
                    parentsArray.[childIndex] <- index :: parentsArray.[childIndex]
            |QuadRet _ -> 
                childrenArray.[index] <- unit.Length - 1 :: childrenArray.[index]
                parentsArray.[unit.Length - 1] <- index :: parentsArray.[unit.Length - 1]
            |QuadEndUnit _ -> 
                ()
            |_ ->
                childrenArray.[index] <- index + 1 :: childrenArray.[index]
                parentsArray.[index+1] <- index :: parentsArray.[index+1]
        match code.block.quads with
        |[] ->
            failwith "empty block???"
        |h::t ->
            getChildren h
    List.iter func unit
    let rec helper i (cUnit:blockWithIdType list) acc =
        match cUnit with
        |[] ->
            List.rev acc
        |h::t ->
//Junk block elimination optimizing version
            if parentsArray.[h.name] <> [] then
                helper (i+1) t ({ 
                                    id = i; 
                                    predecessors = parentsArray.[i]; 
                                    successors = childrenArray.[i]; 
                                    block = h.block 
                                }::acc)
            else
                helper (i+1) t acc
    helper 0 unit []

let inline optimizeIntermediate unitList = 
    unitList
    |> simpleBackwardPropagation
//    |> makeBasicBlocksOfUnit
