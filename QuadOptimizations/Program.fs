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
open QuadOptimizations

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
                UnitList.mapi 1 (fun i quad -> 
                                    match quad with
                                    |QuadEQ (_,_,l1,l2)|QuadNE (_,_,l1,l2)|QuadLT (_,_,l1,l2)
                                    |QuadGT (_,_,l1,l2)|QuadGE (_,_,l1,l2)|QuadLE (_,_,l1,l2) ->
                                        l1:= i + !l1
                                        l2:= i + !l2
                                        labelRegistry.Add (!l1) |>ignore
                                        labelRegistry.Add (!l2) |>ignore
                                    |QuadJump(l) ->
                                        l:= i + !l ; labelRegistry.Add (!l) |>ignore
                                    |_ -> ()
                                    quad) unitList
            let intermediate:quadType list list = 
                Parser.start Lexer.token lexbuf
                |> List.fold (fun acc unitQuads -> (List.rev unitQuads) :: acc) []
            updateJumps intermediate
            |>  UnitList.mapi 1 (fun i quad -> { index=i; quad=quad })
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
        optimizeIntermediate rawIntermediate

    let backend intermediate =
        UnitList.map (fun item -> QuadtoFinal item labelRegistry) intermediate
        |> List.concat
    
    let inline printIntermediate intermediate=    
        UnitList.map (fun item -> quadWithIndexType.print item) intermediate
    let inline printFinal final =
        [codeHead ()]::(UnitList.map finalType.print final) @ [codeTail ()]
    let inline printResult unitList =
        UnitList.iter (fun (str:string) -> output.WriteLine(str)) unitList
        output.Flush()
    let inline printBlocks (unitlist:blockWithIdType list) =
        List.map (fun (block:blockWithIdType) -> 
                       "BlockName "+ block.name.ToString() + " , Id = " + block.id.ToString() 
                       :: "Starts at @" + block.block.startingIndex.ToString() + " and ends at @" + block.block.endingIndex.ToString() 
                       :: List.map quadWithIndexType.print block.block.quads @ [""])
                    unitlist
    let inline printControlFlowGraph (graph:controlFlowGraphNodeType list) =
        List.map (fun (graphNode:controlFlowGraphNodeType) -> 
                      "GraphNodeId "+ graphNode.id.ToString()
                      ::  sprintf "Predecessors = %A" graphNode.predecessors
                      :: sprintf "Successors = %A" graphNode.successors
                      :: "Code :"
                      :: List.map quadWithIndexType.print graphNode.block.quads
                      @ [""]
                 ) graph

let main () =
    let args = System.Environment.GetCommandLineArgs()
    let arguments = 
        dict [for i in args ->
                let ar=i.Split(':')
                if ar.Length=1 then (ar.[0].Trim(),"null")
                else (ar.[0].Trim(),ar.[1].Trim())]
    try
        let inputFile =
            if (arguments.ContainsKey "-in") && (File.Exists arguments.["-in"]) then
                arguments.["-in"]
            else failwith "Proper Usage: QuadOptimizations.exe [-i|-f|-debug:[Blocks|ControlGraph|SimpleOpts]] [-optimize] -in:<inputFile> -out:<outputFile>"
        let outputFile = 
            if arguments.ContainsKey "-out" then
                arguments.["-out"]
            elif arguments.ContainsKey "-debug" then
                (Path.GetFileNameWithoutExtension inputFile) + "." + arguments.["-debug"]
            elif arguments.ContainsKey "-i" then 
                (Path.GetFileNameWithoutExtension inputFile) + ".quads" 
            else 
                (Path.GetFileNameWithoutExtension inputFile) + ".asm"
        Compiler.initialize inputFile outputFile
        if arguments.ContainsKey "-debug" then
            match arguments.["-debug"] with
            |"SimpleOpts" ->
                Compiler.frontend() |> simpleBackwardPropagation |> (List.iter (makeBasicBlocksOfUnit >> List.map (constantFolding) >> Compiler.printBlocks >> Compiler.printResult))
            |"Blocks" ->
                Compiler.frontend() |> (List.iter (makeBasicBlocksOfUnit >> Compiler.printBlocks >> Compiler.printResult))
            |"ControlGraph" ->
                 Compiler.frontend() |> (List.iter (makeBasicBlocksOfUnit >> makeControlFlowGraphOfUnit >> Compiler.printControlFlowGraph >> Compiler.printResult))
            |_ ->
                printfn "Not Implemented"
        else
            Compiler.frontend() |> (if arguments.ContainsKey "-optimize" 
                                    then Compiler.optimizer
                                    else id)
                                |> (fun intermediateList ->
                                        if arguments.ContainsKey "-i"
                                        then Compiler.printIntermediate intermediateList
                                        else intermediateList |> Compiler.backend |> Compiler.printFinal)
                                |> Compiler.printResult
    with
    | Failure s -> Console.WriteLine s
    | ex -> Console.WriteLine (ex.Message + Environment.NewLine + ex.StackTrace) 

main ()