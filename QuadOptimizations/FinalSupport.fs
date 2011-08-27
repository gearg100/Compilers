module FinalSupport
open Error
open Types
open Symbol

let inline sizeof (t:typ) =
    match t with
    |TYPE_int -> "word"
    |TYPE_byte -> "byte"
    |TYPE_array (t,_)-> "word"
    |_ -> internal_error (__SOURCE_FILE__,__LINE__) "invalid type"

type entry with
    static member GetSizeOffsetMode (entry:entry) =
        match entry.entry_info with
        | ENTRY_variable v ->
            (sizeof v.variable_type, v.variable_offset,PASS_BY_VALUE)
        | ENTRY_parameter p ->
            (sizeof p.parameter_type, p.parameter_offset,p.parameter_mode)
        | ENTRY_temporary t ->
            (sizeof t.temporary_type, t.temporary_offset,PASS_BY_VALUE)
        | _ -> internal_error (__SOURCE_FILE__,__LINE__) "invalid entry"
    static member GetUniqueID (entry:entry) =
        match entry.entry_info with
        | ENTRY_function f ->
            f.function_unique_name
        | _ ->
            internal_error (__SOURCE_FILE__,__LINE__) "invalid function"

type registerType = 
    |AX | BX | CX | DX | AL | AH | CL | CH | DL | DH | DI | SI | BP | SP
    static member print = function
        | AX -> "ax"
        | BX -> "bx"
        | CX -> "cx"
        | DX -> "dx"
        | AL -> "al"
        | AH -> "ah"
        | CL -> "cl"
        | CH -> "ch"
        | DL -> "dl"
        | DH -> "dh"
        | DI -> "di"
        | SI -> "si"
        | BP -> "bp"
        | SP -> "sp"

type operandType =
    | Register of registerType
    | Indirect of (registerType * string * int)
    | Identifier of string
    | Direct of int16
    | Const of int16
    static member print = function
        | Register reg -> 
            registerType.print reg
        | Indirect (reg, size,offset) ->
            match offset with
            | 0 ->
                sprintf "%s ptr [%s]" size (registerType.print reg)
            | _ ->
                sprintf "%s ptr [%s%+d]" size (registerType.print reg) offset
        | Identifier id ->
            id
        | Direct n ->
            string n
        | Const c ->
            string c

type finalType = 
    | Start of string
    | End of string
    | Mov of (operandType * operandType)
    | Call of string
    | Lea of (operandType * operandType)
    | Jump of string
    | Cond of (string * string)
    | Add of (operandType * operandType)
    | Sub of (operandType * operandType)
    | Pos of registerType
    | Neg of registerType
    | IMul of registerType
    | IDiv of registerType
    | Push of operandType
    | Pop of registerType
    | Cwd |Cbw
    | Ret 
    | Label of string
    | Cmp of (registerType * registerType)
    | Proc of string
    | Endp of string
    static member print= function
        | Start str -> "\t"+str
        | End str -> "\t"+str
        | Mov (m1, m2) -> 
            sprintf "\tmov %s, %s" 
                (operandType.print m1) 
                (operandType.print m2)
        | Call str -> 
                sprintf "\tcall near ptr %s" str
        | Lea (m1,m2) ->
            sprintf "\tlea %s, %s" 
                (operandType.print m1) 
                (operandType.print m2)
        | Jump str -> 
            sprintf "\tjmp %s" str
        | Cond (c,str) ->
            sprintf "\t%s %s" c str
        | Add (a1, a2) ->
            sprintf "\tadd %s, %s"
                (operandType.print a1)
                (operandType.print a2)
        | Sub (a1, a2) ->
            sprintf "\tsub %s, %s"
                (operandType.print a1)
                (operandType.print a2)
        | Pos a ->
            sprintf "\tnop"
        | Neg a ->
            sprintf "\tneg %s"
                (registerType.print a)
        | IMul reg ->
            sprintf "\timul %s" 
                (registerType.print reg)
        | IDiv reg ->
            sprintf "\tidiv %s" 
                (registerType.print reg)
        | Push mem ->
            sprintf "\tpush %s"
                (operandType.print mem)
        | Pop reg ->
            sprintf "\tpop %s"
                (registerType.print reg)
        | Cwd -> "\tcwd"
        | Cbw -> "\tcbw"
        | Ret -> "\tret"
        | Label lab ->
            sprintf "%s" lab
        | Cmp (r1,r2) -> 
            sprintf "\tcmp %s, %s"
                (registerType.print r1)
                (registerType.print r2)
        | Proc name ->
            sprintf "%s proc near" name
        | Endp name ->
            sprintf "%s endp\n" name
