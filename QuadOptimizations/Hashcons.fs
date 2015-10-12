module Hashcons
open  System.Collections.Generic


type hash_consed<'a> = {
    node : 'a
    tag  : int
}

let hash_value x = x.node
let hash_tag x = x.tag

type Make<'a> (X: IEqualityComparer<'a>)=
    member me.f () =
        let mutable gen_tag = 0
        let table : hash_consed<'a> list array = Array.create 397 []
        fun x ->
            let i = abs((X.GetHashCode x) % table.Length)
            let rec look_and_add = function
            | [] ->
                let hv = {
                            tag = gen_tag
                            node = x
                            }
                table.[i] <- hv :: table.[i]
                gen_tag <- gen_tag + 1
                hv
            | hd::tl ->
                if X.Equals (hd.node,x) then
                    hd
                else
                    look_and_add tl
            look_and_add table.[i]

let mutable hashcons_resets = []
let init () = List.iter (fun f -> f()) hashcons_resets

let register_hcons h u =
    let mutable hf = (h u)
    let reset () = hf <- h u
    hashcons_resets <- (reset :: hashcons_resets)
    (function x -> hf x)
