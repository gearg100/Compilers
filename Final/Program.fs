module Program

open System
open System.IO
open Microsoft.FSharp.Text.Lexing

open Lexer
open Parser
open QuadSupport
open AuxFunctions
open FinalSupport
open FinalFunctions

module UnitList = 
    let inline map f (unitList: 'a list list) =
        List.map (List.map f) unitList
    let inline mapi f (unitList: 'a list list) =
        let i = ref -1
        List.map (List.map (fun quad -> incr i; f !i quad)) unitList
    let inline iter f (unitList: 'a list list) =
        List.iter (List.iter f) unitList
    let inline iteri f (unitList: 'a list list) =
        let i = ref -1
        List.iter (List.iter (fun quad -> incr i; f !i quad)) unitList

module Compiler=
    let mutable input = null
    let mutable output = null
    let mutable inFile= ""
    let inline initialize (inputFile:string) (outputFile:string) =
        input <- new StreamReader(inputFile)
        output <- new StreamWriter(outputFile)
        inFile <- inputFile
    let frontend() =
        if input = null then failwith "Internal Error: Uninitialized"
        let lexbuf=LexBuffer<char>.FromTextReader input
        let inline produceIntermediate () =
            let inline updateJumps unitList =
                UnitList.mapi (fun i quad -> 
                                match quad with
                                |QuadEQ (_,_,l)|QuadNE (_,_,l)|QuadLT (_,_,l)
                                |QuadGT (_,_,l)|QuadGE (_,_,l)|QuadLE (_,_,l)
                                |QuadJump(l) ->
                                    l:= i + !l + 1; labelRegistry.Add (!l) |>ignore
                                |_ -> ()
                                quad) unitList
            let intermediate:quadType list list = 
                    Parser.start Lexer.token lexbuf
                    |> List.fold (fun acc unitQuads -> (List.rev unitQuads) :: acc) []
            updateJumps intermediate
        setInitialPos lexbuf inFile
        try
            produceIntermediate ()
        with 
        | :? Error.Terminate -> 
            printfn "Terminate"
            Console.ReadLine() |> ignore
            []
        | :? Symbol.Exit -> 
            printfn "Exit" 
            Console.ReadLine() |> ignore
            []
        | Failure s -> 
            printfn "Syntax Error: Unrecognized Syntax Error.\nHint: Perhaps there are orphaned brackets or missing semicolons"
            Console.ReadLine() |> ignore
            []
        | ex->
            printfn "Unhandled Exception: %s \n\n%A \n\n%A \n\n%A" ex.Message ex.Data ex.StackTrace ex.TargetSite
            Console.ReadLine() |> ignore
            []


    let optimizer rawIntermediate =
       rawIntermediate

    let backend intermediate =
        UnitList.map (fun tuple -> QuadtoFinal tuple labelRegistry) intermediate
        |> List.concat
    
    let inline printIntermediate intermediate=    
        UnitList.map (fun (i,quad) -> i.ToString() + ":\t" + quadType.print (i,quad)) intermediate
    let inline printFinal final =
        [codeHead ()]::(UnitList.map finalType.print final) @ [codeTail ()]
    let inline printResult unitList =
        UnitList.iter (fun (str:string) -> output.WriteLine(str)) unitList
        output.Flush()

let main () =
    let args = System.Environment.GetCommandLineArgs()
    let arguments = dict [for i in args ->
                            let ar=i.Split(':')
                            if ar.Length=1 then (ar.[0],"null")
                            else (ar.[0],ar.[1])]
    try
        let inputFile =
            if (arguments.ContainsKey "-in") && (File.Exists arguments.["-in"]) then
                arguments.["-in"]
            else failwith "Proper Usage: QuadOptimizations.exe (-i|-f) [-optimize] -in:<inputFile> -out:<outputFile>"
        let outputFile = 
            if arguments.ContainsKey "-out" then
                arguments.["-out"]
            elif arguments.ContainsKey "-i" then 
                (Path.GetFileNameWithoutExtension inputFile) + ".quads" 
            else 
                (Path.GetFileNameWithoutExtension inputFile) + ".asm"
        Compiler.initialize inputFile outputFile
        if arguments.ContainsKey "-i" then
            Compiler.frontend() |> UnitList.mapi (fun i quad -> (i,quad)) |> Compiler.printIntermediate
        elif arguments.ContainsKey "-optimize" then
            Compiler.frontend() |> Compiler.optimizer |> UnitList.mapi (fun i quad -> (i,quad)) |> Compiler.backend |> Compiler.printFinal
        else
            Compiler.frontend() |> UnitList.mapi (fun i quad -> (i,quad)) |> Compiler.backend |> Compiler.printFinal
        |> Compiler.printResult
    with
    |Failure s -> Console.WriteLine s
    | ex -> Console.WriteLine ex.Message

main ()