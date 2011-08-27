module QuadSupport
open Types
open Symbol
open Error
open Identifier

type quadElementType =
    |QNone
    |Entry of entry
    |Valof of entry
    |Int of int
    |Byte of byte
    |String of string
    static member GetValueAsString = function
        |QNone -> ""
        |Entry e-> 
            id_name e.entry_id
        |Valof e-> 
            id_name e.entry_id |> sprintf "[%s]" 
        |Int i -> 
            sprintf "%d" i
        |Byte c -> 
            sprintf "%d" c
        |String s -> 
            sprintf "%A" s
    static member GetType = function
        |QNone -> TYPE_none
        |Entry e-> 
            entry_info.GetType e.entry_info
        |Valof e-> 
            entry_info.GetType e.entry_info
        |Int i -> 
            TYPE_int
        |Byte c -> 
            TYPE_byte
        |String s -> 
            TYPE_array (TYPE_byte, s.Length)

type quadType =
    |QuadUnit of entry
    |QuadEndUnit of entry
    |QuadAdd of quadElementType * quadElementType * entry
    |QuadSub of quadElementType * quadElementType * entry
    |QuadNeg of quadElementType * entry
    |QuadMult of quadElementType * quadElementType * entry
    |QuadDiv of quadElementType * quadElementType * entry
    |QuadMod of quadElementType * quadElementType * entry
    |QuadAssign of quadElementType * entry
    |QuadArray of entry * quadElementType * entry
    |QuadEQ of quadElementType * quadElementType * (int ref)
    |QuadNE of quadElementType * quadElementType * (int ref)
    |QuadLT of quadElementType * quadElementType * (int ref)
    |QuadGT of quadElementType * quadElementType * (int ref)
    |QuadGE of quadElementType * quadElementType * (int ref)
    |QuadLE of quadElementType * quadElementType * (int ref)
    |QuadJump of (int ref)
    |QuadCall of entry
    |QuadPar of quadElementType * pass_mode
    |QuadRet
    static member print = 
        function
        |QuadUnit s ->
            sprintf "unit , %s, - , -" s.entry_id.node
        |QuadEndUnit s->
            sprintf "endu , %s, - , -\n" s.entry_id.node
        |QuadAdd(a,b,e) ->
            sprintf "+    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
        |QuadSub(a,b,e) ->
            sprintf "-    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
        |QuadNeg(a,e) ->
            sprintf "-    , %s, - , %s"
                (quadElementType.GetValueAsString a)
                (id_name e.entry_id)
        |QuadMult(a,b,e) ->
            sprintf "*    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
        |QuadDiv(a,b,e) ->
            sprintf "/    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
        |QuadMod(a,b,e) ->
            sprintf "mod  , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry_id)
        |QuadAssign(a,e) ->
            sprintf ":=   , %s, - , %s" 
                (quadElementType.GetValueAsString a) 
                (id_name e.entry_id)
        |QuadArray(e1, a, e2) -> 
            sprintf "array, %s, %s, %s" 
                (id_name e1.entry_id) 
                (quadElementType.GetValueAsString a) 
                (id_name e2.entry_id)
        |QuadEQ(a,b,i) ->
            sprintf "==   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
        |QuadNE(a,b,i) ->
            sprintf "!=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i  
        |QuadLT(a,b,i) ->
            sprintf "<    , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
        |QuadGT(a,b,i) ->
            sprintf ">    , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
        |QuadGE(a,b,i) ->
            sprintf ">=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i 
        |QuadLE(a,b,i) ->
            sprintf "<=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i  
        |QuadJump i ->
            sprintf "jump , - , - , %d" !i
        |QuadCall e ->
            sprintf "call , - , - , %s" (id_name e.entry_id)
        |QuadPar(a, mode) ->
            sprintf "par  , %s , %s , -"
                (quadElementType.GetValueAsString a)
                (pass_mode.print mode)
        |QuadRet -> 
            sprintf "ret  , -, -, -"

