module Types

type typ =
    | TYPE_none
    | TYPE_int
    | TYPE_byte
    | TYPE_array of typ * int
    | TYPE_proc
    static member CheckEqualTypes x y =
        match x, y with
        |TYPE_array (t1,i1),TYPE_array (t2,i2) when i1 = -1 || i2 = -1 -> t1=t2
        |_ -> x=y

let inline (==) x y = typ.CheckEqualTypes x y

let rec sizeOfType t =
    match t with
    | TYPE_int            -> 2
    | TYPE_byte           -> 1
    | TYPE_array (et, sz) -> sz * sizeOfType et
    | _                   -> 0

let rec equalType t1 t2 =
    match t1, t2 with
    | TYPE_array (et1, sz1), TYPE_array (et2, sz2) -> equalType et1 et2
    | _                                            -> t1 = t2