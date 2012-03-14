module Types

type typ =
    | TYPE_none
    | TYPE_int
    | TYPE_byte
    | TYPE_array of typ * int16
    | TYPE_proc
    static member CheckEqualTypes x y =
        match x, y with
        |TYPE_array (t1,i1),TYPE_array (t2,i2) when i1 = -1s || i2 = -1s -> t1=t2
        |_ -> x=y
    override me.ToString() =
        match me with
        | TYPE_none -> "TYPE_none"
        | TYPE_int -> "TYPE_int"
        | TYPE_byte -> "TYPE_byte"
        | TYPE_proc -> "TYPE_proc"
        | TYPE_array(t,i) -> "TYPE_array(" + t.ToString() + ", " + i.ToString() + ")"

let inline (==) x y = typ.CheckEqualTypes x y

let rec sizeOfType t =
    match t with
    | TYPE_int            -> 2s
    | TYPE_byte           -> 1s
    | TYPE_array (et, sz) -> sz * sizeOfType et
    | _                   -> 0s

let inline equalType t1 t2 =
    match t1, t2 with
    | TYPE_array (et1, sz1), TYPE_array (et2, sz2) -> et1 = et2
    | _                                            -> t1 = t2