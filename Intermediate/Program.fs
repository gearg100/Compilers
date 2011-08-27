module Semantic

open System
open Microsoft.FSharp.Text.Lexing

open Lexer
open Parser
open QuadSupport
open AuxFunctions
let readAndProcess() =
    let lexbuf=LexBuffer<char>.FromTextReader stdin
    let inline loop () =
        try
            (Parser.start Lexer.token lexbuf)
        with 
        | :? Error.Terminate -> printfn "Terminate" ;Console.ReadLine()|>ignore; []
        | :? Symbol.Exit -> printfn "Exit" ;Console.ReadLine()|>ignore; []
        | Failure s -> printfn "Syntax Error: Unrecognized Syntax Error.\nHint: Perhaps there are orphaned brackets or missing semicolons"; []
        | ex->
            printfn "Unhandled Exception: %s \n\n%A \n\n%A \n\n%A" ex.Message ex.Data ex.StackTrace ex.TargetSite
            Console.ReadLine()|>ignore; []
    setInitialPos lexbuf "default"
    let x = 
        loop ()
        |>List.rev
    x |> List.iteri (fun i quad -> 
                        match quad with
                        |QuadEQ (_,_,l)|QuadNE (_,_,l)|QuadLT (_,_,l)
                        |QuadGT (_,_,l)|QuadGE (_,_,l)|QuadLE (_,_,l) ->
                            l:= i + !l + 1
                        |QuadJump(l) ->
                            l:= i + !l + 1
                        |_ -> ())
    List.map quadType.print x

readAndProcess() |> List.iteri (fun i quad -> Console.WriteLine (string i + ":\t" + quad))