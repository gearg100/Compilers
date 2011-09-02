module QuadSupport

open Types
open Symbol
open Error
open Identifier
open System.Collections.Generic

let toString x y =System.String.Format(x,y) 

type quadElementType =
    |QNone
    |Entry of entryWithTypeAndNesting
    |Valof of entryWithTypeAndNesting
    |Int of int16
    |Byte of byte
    |String of string list*string
    static member GetValueAsString = function
        |QNone -> ""
        |Entry ent-> 
            id_name ent.entry.entry_id
        |Valof ent-> 
           toString "[{0}]" [|(id_name ent.entry.entry_id)|] //sprintf "[%s]"
        |Int i -> 
            i.ToString() //sprintf "%d" i
        |Byte c -> 
            c.ToString() //sprintf "%d" c
        |String (lst,s) -> 
            s
    static member GetType = function
        |QNone -> TYPE_none
        |Entry ent-> 
            entry.GetType ent.entry
        |Valof ent-> 
            ent.entryType
        |Int i -> 
            TYPE_int
        |Byte c -> 
            TYPE_byte
        |String (_,s) -> 
            TYPE_array (TYPE_byte,int16 s.Length)

type quadType =
    |QuadNone
    |QuadUnit of entry
    |QuadEndUnit of entry
    |QuadAdd of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadSub of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadNeg of quadElementType * entryWithTypeAndNesting
    |QuadPos of quadElementType * entryWithTypeAndNesting
    |QuadMult of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadDiv of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadMod of quadElementType * quadElementType * entryWithTypeAndNesting
    |QuadAssign of quadElementType * quadElementType
    |QuadArray of entryWithTypeAndNesting * quadElementType * entryWithTypeAndNesting
    |QuadEQ of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadNE of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadLT of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadGT of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadGE of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadLE of quadElementType * quadElementType * (int ref)*(int ref)
    |QuadJump of (int ref)
    |QuadCall of entryWithTypeAndNesting * HashSet<entryWithTypeAndNesting>
    |QuadPar of quadElementType * pass_mode
    |QuadRet of entry
    static member print quad= 
        match quad with
        |QuadNone -> ""
        |QuadUnit s ->
            toString "unit , {0}, - , -" [|(id_name s.entry_id)|] 
            //sprintf "unit , %s, - , -" s.entry_id.node
        |QuadEndUnit s->
            toString "endu , {0}, - , -" [|(id_name s.entry_id)|]
            //sprintf "endu , %s, - , -" s.entry_id.node
        |QuadAdd(a,b,e) ->
            toString "+    , {0}, {1}, {2}" [|
            //sprintf "+    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry.entry_id)
            |]
        |QuadSub(a,b,e) ->
            toString "-    , {0}, {1}, {2}" [|
            //sprintf "-    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry.entry_id)
            |]
        |QuadPos(a,e) ->
            toString "+    , {0}, - , {1}" [|
            //sprintf "-    , %s, - , %s"
                (quadElementType.GetValueAsString a)
                (id_name e.entry.entry_id)
            |]
        |QuadNeg(a,e) ->
            toString "-    , {0}, - , {1}" [|
            //sprintf "-    , %s, - , %s"
                (quadElementType.GetValueAsString a)
                (id_name e.entry.entry_id)
            |]
        |QuadMult(a,b,e) ->
            toString "*    , {0}, {1}, {2}" [|
            //sprintf "*    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry.entry_id)
            |]
        |QuadDiv(a,b,e) ->
            toString "/    , {0}, {1}, {2}" [|
            //sprintf "/    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry.entry_id)
            |]
        |QuadMod(a,b,e) ->
            toString "%    , {0}, {1}, {2}" [|
            //sprintf "%    , %s, %s, %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b) 
                (id_name e.entry.entry_id)
            |]
        |QuadAssign(a,e) ->
            toString ":=   , {0}, - , {1}" [|
            //sprintf ":=   , %s, - , %s" 
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString e)
            |]
        |QuadArray(e1, a, e2) ->
            toString "array, {0}, {1}, {2}" [|
            //sprintf "array, %s, %s, %s" 
                (id_name e1.entry.entry_id)
                (quadElementType.GetValueAsString a) 
                (id_name e2.entry.entry_id)
            |]
        |QuadEQ(a,b,i1,i2) ->
            toString "==   , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf "==   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1 
                !i2
            |]
        |QuadNE(a,b,i1,i2) ->
            toString "!=   , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf "!=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1
                !i2
            |]
        |QuadLT(a,b,i1,i2) ->
            toString "<    , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf "<    , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1
                !i2
            |]
        |QuadGT(a,b,i1,i2) ->
            toString ">    , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf ">   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1
                !i2 
            |]
        |QuadGE(a,b,i1,i2) ->
            toString ">=   , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf ">=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1
                !i2 
            |]
        |QuadLE(a,b,i1,i2) ->
            toString "<=   , {0}, {1}, ({2:d}, {3:d})" [|
            //sprintf "<=   , %s, %s, %d"
                (quadElementType.GetValueAsString a) 
                (quadElementType.GetValueAsString b)
                !i1
                !i2 
            |]
        |QuadJump i ->
            toString "jump , - , - , {0:d}" [|!i|]
            //sprintf "jump , - , - , %d" !i
        |QuadCall (e,_) ->
            toString "call , - , - , {0}" [|(id_name e.entry.entry_id)|]
            //sprintf "call , - , - , %s" (id_name e.entry_id)
        |QuadPar(a, mode) ->
            toString "par  , {0} , {1} , -" [| 
            //sprintf "par  , %s , %s , -"
                (quadElementType.GetValueAsString a)
                (pass_mode.print mode)
            |]
        |QuadRet _ -> 
            "ret  , -, -, -"
    override x.ToString() = quadType.print x

type quadWithIndexType =
    {
        index:int;
        mutable quad:quadType;
    }
    static member print (item:quadWithIndexType) =
        item.index.ToString() + ":\t" + item.quad.ToString()
