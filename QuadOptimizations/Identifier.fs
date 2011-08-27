
module Identifier
open Hashcons
type id = hash_consed<string>
    
let Hid = Make<string> (HashIdentity.Structural<string>)

let id_make = register_hcons Hid.f ()
let id_name = hash_value

let pretty_id ppf id =
    fprintf ppf "%s" (id_name id)