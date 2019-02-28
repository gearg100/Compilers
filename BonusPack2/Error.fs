namespace BonusPack
open System.IO
open Microsoft.FSharp.Text
open Microsoft.FSharp.Core.Printf
module Error=
    exception Terminate

    type verbose = Vquiet | Vnormal | Vverbose

    let flagVerbose = ref Vnormal

    let numErrors = ref 0
    let maxErrors = ref 10
    let flagWarnings = ref true
    let numWarnings = ref 0
    let maxWarnings = ref 200

    type position =
        PosPoint   of Lexing.Position
      | PosContext of Lexing.Position * Lexing.Position
      | PosDummy

    let position_point lpos = PosPoint lpos
    let position_context lpos_start lpos_end = PosContext (lpos_start, lpos_end)
    let position_dummy = PosDummy

    let print_position ppf pos =
      match pos with
      | PosPoint lpos ->
          fprintf ppf "file \"%s\", line %d, character %d: "
            lpos.FileName lpos.Line lpos.Column
      | PosContext (lpos_start, lpos_end) ->
          if lpos_start.FileName <> lpos_end.FileName then
            fprintf ppf "file \"%s\", line %d, character %d to file %s, line %d, character %d: "
              lpos_start.FileName lpos_start.Line lpos_start.Column 
              lpos_end.FileName lpos_end.Line     lpos_end.Column
          else if lpos_start.Line <> lpos_end.Line then
            fprintf ppf "file \"%s\", line %d, character %d to line %d, character %d: "
              lpos_start.FileName   lpos_start.Line   lpos_start.Column
                                    lpos_end.Line     lpos_end.Column
          else if lpos_start.Column <> lpos_end.Column then
            fprintf ppf "file \"%s\", line %d, characters %d to %d: "
              lpos_start.FileName   lpos_start.Line lpos_start.Column lpos_end.Column
          else
            fprintf ppf "file \"%s\", line %d, character %d: "
              lpos_start.FileName lpos_start.Line lpos_start.Column
      | PosDummy ->
          ()

    let no_out buf pos len = ()
    let no_flush () = ()
    let null_formatter = TextWriter.Null

    let internal_error (fname, lnum) fmt =
        let fmt = TextWriterFormat<'T,unit>(fmt)
        incr numErrors
        let cont ppf = raise Terminate
        eprintf "Internal error occurred at %s:%s, " fname lnum
        kfprintf cont stderr fmt

    let fatal fmt =
        let fmt = TextWriterFormat<'T,unit>("Fatal error: " + fmt)
        incr numErrors
        let cont ppf = raise Terminate
        kfprintf cont stderr fmt

    let error fmt =
        let fmt = TextWriterFormat<'T,unit>("Error: " + fmt)
        incr numErrors
        if !numErrors >= !maxErrors then
            let cont ppf =  eprintf "Too many errors, aborting...\n"
                            raise Terminate
            kfprintf cont stderr fmt
        else 
            eprintf fmt

    let warning fmt =
        let fmt = TextWriterFormat<'T,unit>("Warning: " + fmt)
        if !flagWarnings then
            incr numWarnings
            if !numWarnings >= !maxWarnings then
                let cont ppf =  eprintf "Too many warnings, no more will be shown...\n"
                                flagWarnings := false
                kfprintf cont stderr fmt
            else
                eprintf fmt
        else
            fprintf null_formatter fmt

    let message fmt =
        let fmt = TextWriterFormat<'T,unit>(fmt)
        eprintf fmt
