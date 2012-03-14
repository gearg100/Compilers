module Program

open System
open System.IO
open Microsoft.FSharp.Text
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
    let Initialize (inputFile:string) (outputFile:string) =
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
                                    |QuadComparison(_,_,_,l1,l2) ->
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
            eprintfn "Terminate"
            []
        | :? Symbol.Exit -> 
            eprintfn "Exit" 
            []
        | :? System.NullReferenceException ->
            eprintfn "Internal Error: Attempt to dereference null pointer"
            []
        | Failure s -> 
            eprintfn "Syntax Error: Unrecognized Syntax Error.\nHint: Perhaps there are orphaned brackets or missing semicolons"
            []
        | ex->
            eprintfn "Unhandled Exception: %s \n\n%A \n\n%A \n\n%A %A %A" ex.Message ex.Data ex.StackTrace ex.TargetSite ex.InnerException ex
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
    open Block
    let inline printBlocks (unitlist:blockWithIdType list) =
        List.map (fun (block:blockWithIdType) -> 
                       "BlockName "+ block.name.ToString() + " , Id = " + block.id.ToString() 
                       :: "Starts at @" + block.block.startingIndex.ToString() + " and ends at @" + block.block.endingIndex.ToString() 
                       :: List.map quadWithIndexType.print block.block.quads @ [""])
                    unitlist
    let inline printControlFlowGraph (graph:controlFlowGraphNodeType list) =
        List.map (fun (graphNode:controlFlowGraphNodeType) -> 
                      "GraphNodeId "+ graphNode.id.ToString()
                      :: sprintf "Predecessors = %A" graphNode.predecessors
                      :: sprintf "Successors = %A" graphNode.successors
                      :: "Code :"
                      :: List.map quadWithIndexType.print graphNode.block.quads
                      @ [""]
                 ) graph
        //|> (fun x -> printfn "%A" x; x) 

let main () =
    let inputFileCandidate = ref ""
    let outputFileCandidate = ref ""
    let produceFinal = ref true
    let optimizeFlag = ref false
    let debugFlags = [|"Blocks";"SimpleOpts";"ControlGraph";"SimpleOpts+ControlGraph";"SimplifyBlocks"|]
    let debugMode = ref "" 
    let specs =
        [
            "-o", ArgType.String (fun s -> outputFileCandidate := s), "Name of the output"
            "-O", ArgType.Set optimizeFlag, "Enable Optimizations"
            "-i", ArgType.Clear produceFinal, "Emits intermediate code"
            "-f", ArgType.Set produceFinal, "Emits final code"
            "--debug", ArgType.String (fun s -> debugMode := s ), "Emits basic blocks, simply optimized blocks or control graph nodes"
        ]
        |> List.map (fun (sh, ty, desc) -> ArgInfo(sh, ty, desc))
    do ArgParser.Parse(specs, (fun infile -> inputFileCandidate := infile)) 
    try
        if !inputFileCandidate = "" then failwith "Unspecified Input File"
        elif not <| File.Exists !inputFileCandidate then failwithf "Specified Input File \"%s\" does not exist" !inputFileCandidate
        if !debugMode <> "" && not <| Array.exists ((=) !debugMode) debugFlags then failwithf "Specified Debug Flag \"%s\" does not exist" !debugMode
        let inputFile = 
            !inputFileCandidate
        let outputFile =
            if !outputFileCandidate <> "" then !outputFileCandidate
            elif !debugMode <> "" then Path.GetFileNameWithoutExtension(inputFile) + "." + (!debugMode).ToLower()
            elif not !produceFinal then Path.GetFileNameWithoutExtension(inputFile) + ".quads"
            else (Path.GetFileNameWithoutExtension inputFile) + ".asm"
        do Compiler.Initialize inputFile outputFile
        let intermediate = Compiler.frontend ()
        if intermediate <> [] then
            intermediate |> if !debugMode <> "" 
                            then List.iter (match !debugMode with
                                            |"SimpleOpts" ->
                                                simpleBackwardPropagation >> Block.makeBasicBlocksOfUnit >> List.map (constantFolding) >> Compiler.printBlocks >> Compiler.printResult
                                            |"Blocks" ->
                                                Block.makeBasicBlocksOfUnit >> Compiler.printBlocks >> Compiler.printResult
                                            |"SimplifyBlocks" ->
                                                simpleBackwardPropagation >> Block.makeBasicBlocksOfUnit >> List.map (constantFolding) >> shortenJumpPaths >> Compiler.printBlocks >> Compiler.printResult
                                            |"ControlGraph" ->
                                                Block.makeBasicBlocksOfUnit >> makeControlFlowGraphOfUnit >> Compiler.printControlFlowGraph >> Compiler.printResult
                                            |"SimpleOpts+ControlGraph" ->
                                                simpleBackwardPropagation >> Block.makeBasicBlocksOfUnit >> List.map (constantFolding) >> shortenJumpPaths >> makeControlFlowGraphOfUnit >> Compiler.printControlFlowGraph >> Compiler.printResult
                                            |_ ->
                                                eprintfn "Not Implemented"
                                                ignore)
                            else (if !optimizeFlag
                                    then Compiler.optimizer
                                    else id)
                                >> fun intermediateList ->
                                    if not !produceFinal
                                        then Compiler.printIntermediate intermediateList
                                        else intermediateList |> Compiler.backend 
                                             |> Compiler.printFinal
                                >> Compiler.printResult
    with
    | Failure s -> Console.Error.WriteLine s
    | ex -> Console.Error.WriteLine (ex.Message + Environment.NewLine + ex.StackTrace)
main ()