module Semantic

open System
open Microsoft.FSharp.Text.Lexing

open Lexer
open Parser

let readAndProcess() =
    let lexbuf=LexBuffer<char>.FromTextReader stdin
    let loop () =
        try
            (Parser.start Lexer.token lexbuf)
        with 
        | :? Error.Terminate -> printfn "Terminate" ;Console.ReadLine()|>ignore
        | :? Symbol.Exit -> printfn "Exit" ;Console.ReadLine()|>ignore
        | Failure s -> printfn "Syntax Error: Unrecognized Syntax Error.\nHint: Perhaps there are orphaned brackets or missing semicolons" 
        | ex->
            printfn "Unhandled Exception: %s \n\n%A \n\n%A \n\n%A" ex.Message ex.Data ex.StackTrace ex.TargetSite
            Console.ReadLine()|>ignore
    setInitialPos lexbuf "default"
    loop ()

readAndProcess()