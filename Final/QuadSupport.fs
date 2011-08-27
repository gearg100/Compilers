module QuadSupport

open Types
open Symbol
open Error
open Identifier
let toString x y =System.String.Format(x,y) 
type entryWithTypeAndNesting = entry*typ*int

type quadElementType =
    |QNone
    |Entry of entryWithTypeAndNesting
    |Valof of entryWithTypeAndNesting
    |Int of int
    |Byte of byte
    |String of string list*string
    static member GetValueAsString = function
        |QNone -> ""
        |Entry (e,_,_)-> 
            id_name e.entry_id
        |Valof (e,_,_)-> 
           toString "[{0}]" [|(id_name e.entry_id)|] //sprintf "[%s]"
        |Int i -> 
            i.ToString() //sprintf "%d" i
        |Byte c -> 
            c.ToString() //sprintf "%d" c
        |String (lst,s) -> 
            s
    static member GetType = function
        |QNone -> TYPE_none
        |Entry (e,_,_)-> 
            entry_info.GetType e
        |Valof (_,t,_)-> 
            t
        |Int i -> 
            TYPE_int
        |Byte c -> 
            TYPE_byte
        |String (_,s) -> 
            TYPE_array (TYPE_byte, s.Length)

type quadType =
    |QuadNone
    |QuadUnit of entry
    |QuadEndUnit of entry
    |QuadAdd of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadSub of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadNeg of quadElementType * entryWithTypeAndNesting
    |QuadMult of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadDiv of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadMod of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadAssign of quadElementType * quadElementType
    |QuadArray of entryWithTypeAndNesting * quadElementType * entryWithTypeAndNesting
    |QuadEQ of quadElementType * quadElementType * (int ref)
    |QuadNE of quadElementType * quadElementType * (int ref)
    |QuadLT of quadElementType * quadElementType * (int ref)
    |QuadGT of quadElementType * quadElementType * (int ref)
    |QuadGE of quadElementType * quadElementType * (int ref)
    |QuadLE of quadElementType * quadElementType * (int ref)
    |QuadJump of (int ref)
    |QuadCall of entryWithTypeAndNesting
    |QuadPar of quadElementType * pass_mode
    |QuadRet of entry
    static member print (i,quad)= 
        match quad with
        |QuadNone -> ""
        |QuadUnit s ->
            toString "unit , {0}, - , -" [|(id_name s.entry_id)|] 
            //sprintf "unit , %s, - , -" s.entry_id.node
        |QuadEndUnit s->
            toString "endu , {0}, - , -" [|(id_name s.entry_id)|]
            //sprintf "endu , %s, - , -" s.entry_id.node
        |QuadAdd(a,b,(e,_,_)) ->
            toString "+    , {0}, {1}, {2}" [|
            //sprintf "+    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
            |]
        |QuadSub(a,b,(e,_,_)) ->
            toString "-    , {0}, {1}, {2}" [|
            //sprintf "-    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
            |]
        |QuadNeg(a,(e,_,_)) ->
            toString "-    , {0}, - , {1}" [|
            //sprintf "-    , %s, - , %s"
                (quadElementType.GetValueAsString a)
                (id_name e.entry_id)
            |]
        |QuadMult(a,b,(e,_,_)) ->
            toString "*    , {0}, {1}, {2}" [|
            //sprintf "*    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
            |]
        |QuadDiv(a,b,(e,_,_)) ->
            toString "/    , {0}, {1}, {2}" [|
            //sprintf "/    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
            |]
        |QuadMod(a,b,(e,_,_)) ->
            toString "%    , {0}, {1}, {2}" [|
            //sprintf "%    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
            |]
        |QuadAssign(a,e) ->
            toString ":=   , {0}, - , {1}" [|
            //sprintf ":=   , %s, - , %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString e)
            |]
        |QuadArray((e1,_,_), a, (e2,_,_)) ->
            toString "array, {0}, {1}, {2}" [|
            //sprintf "array, %s, %s, %s" 
                (id_name e1.entry_id) 
                (quadElementType.GetValueAsString a) 
                (id_name e2.entry_id)
            |]
        |QuadEQ(a,b,i) ->
            toString "==   , {0}, {1}, {2:d}" [|
            //sprintf "==   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |]
        |QuadNE(a,b,i) ->
            toString "!=   , {0}, {1}, {2:d}" [|
            //sprintf "!=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |]
        |QuadLT(a,b,i) ->
            toString "<    , {0}, {1}, {2:d}" [|
            //sprintf "<    , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |]
        |QuadGT(a,b,i) ->
            toString ">    , {0}, {1}, {2:d}" [|
            //sprintf ">   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |] 
        |QuadGE(a,b,i) ->
            toString ">=   , {0}, {1}, {2:d}" [|
            //sprintf ">=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |]
        |QuadLE(a,b,i) ->
            toString "<=   , {0}, {1}, {2:d}" [|
            //sprintf "<=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
            |]
        |QuadJump i ->
            toString "jump , - , - , {0:d}" [|!i|]
            //sprintf "jump , - , - , %d" !i
        |QuadCall (e,_,_) ->
            toString "call , - , - , {0}" [|id_name e.entry_id|]
            //sprintf "call , - , - , %s" (id_name e.entry_id)
        |QuadPar(a, mode) ->
            toString "par  , {0} , {1} , -" [| 
            //sprintf "par  , %s , %s , -"
                (quadElementType.GetValueAsString a)
                (pass_mode.print mode)
            |]
        |QuadRet _ -> 
            "ret  , -, -, -"
