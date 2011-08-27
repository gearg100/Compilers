namespace BonusPack
module Identifier=
    type id = Hashcons.hash_consed<string>
    
    let Hid = Hashcons.Make<string> (HashIdentity.Structural<string>)

    let id_make = Hashcons.register_hcons Hid.f ()
    let id_name = Hashcons.hash_value

    let pretty_id ppf id =
      fprintf ppf "%s" (id_name id)