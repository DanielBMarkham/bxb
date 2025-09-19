#!/usr/bin/env -S dotnet fsi
open System
open System.Text.RegularExpressions
open System.IO
open Microsoft.FSharp.Compiler.Interactive.Settings
module Clicommon=

    #if INTERACTIVE
    printfn "Source File: %s" __SOURCE_FILE__
    printfn "hello from the beginning of Clicommon.fsx"
    let NESTED =
        (let frames = System.Diagnostics.StackTrace().GetFrames()
                    |> Array.map (fun frame -> frame.GetMethod().Name)
        frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
    if NESTED then printfn "nested" else printfn "not nested"
    #endif


    type LogLevel =
        | Info = 1
        | Warn = 2
        | Error = 3


    let mutable verbosity = LogLevel.Error
    let mutable addDatetime = false

    let mutable inputFile:string option = None
    let mutable outputFile:string option = None
    let mutable showHelp = false
    let mutable delim = @"\t"

    let log (level: LogLevel) (msg: string) =
        let prefix = if addDatetime then DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") + " " else ""
        if (int level) >= (int verbosity) then
            Console.Error.WriteLine (prefix + msg)

    let reversecliwordWords (cluster: string) : string =
        if String.IsNullOrEmpty cluster then ""
        else
            cluster.Split([|' '|], StringSplitOptions.None)
            |> Array.rev
            |> String.concat " "

    let processcliwordRow (fields: string[]) : string[] =
        if verbosity = LogLevel.Info then log LogLevel.Info "Entering processcliwordRow"
        let result = 
            fields 
            |> Array.rev 
            |> Array.map reversecliwordWords
        if verbosity = LogLevel.Info then log LogLevel.Info "Exiting processcliwordRow"
        result

    let processcliwordData (data: string[][]) : string[][] =
        if verbosity = LogLevel.Info then log LogLevel.Info "Entering processcliwordData"
        let result = data |> Array.map processcliwordRow
        if verbosity = LogLevel.Info then log LogLevel.Info "Exiting processcliwordData"
        result

    let reversecliwordTestWords (cluster: string) : string =
        if String.IsNullOrEmpty cluster then ""
        else
            cluster.Split([|' '|], StringSplitOptions.None)
            |> Array.rev
            |> String.concat " "

    let processcliwordTestRow (fields: string[]) : string[] =
        if verbosity = LogLevel.Info then log LogLevel.Info "Entering processcliwordTestRow"
        let result = 
            fields 
            |> Array.rev 
            |> Array.map reversecliwordTestWords
        if verbosity = LogLevel.Info then log LogLevel.Info "Exiting processcliwordTestRow"
        result

    let processcliwordTestData (data: string[][]) : string[][] =
        if verbosity = LogLevel.Info then log LogLevel.Info "Entering processcliwordTestData"
        let result = data |> Array.map processcliwordTestRow
        if verbosity = LogLevel.Info then log LogLevel.Info "Exiting processcliwordTestData"
        result

    let cliwordTestArraysEqual (a1: string[]) (a2: string[]) =
        if a1.Length <> a2.Length then false
        else
            Seq.zip a1 a2 |> Seq.forall (fun (x, y) -> x = y)

    let cliwordTestJaggedEqual (d1: string[][]) (d2: string[][]) =
        if d1.Length <> d2.Length then false
        else
            Seq.zip d1 d2 |> Seq.forall (fun (r1, r2) -> cliwordTestArraysEqual r1 r2)





    let parseArgs (args: string[]) =
        let mutable i = 0
        while i < args.Length do
            let arg = args.[i]
            if arg = "--i" then
                i <- i + 1
                if i < args.Length then inputFile <- Some args.[i]
            elif arg = "--o" then
                i <- i + 1
                if i < args.Length then outputFile <- Some args.[i]
            elif arg = "--v" then
                i <- i + 1
                if i < args.Length then
                    match args.[i].ToUpper() with
                    | "INFO" -> verbosity <- LogLevel.Info
                    | "WARN" -> verbosity <- LogLevel.Warn
                    | "ERROR" -> verbosity <- LogLevel.Error
                    | _ -> log LogLevel.Error (sprintf "Invalid verbosity level: %s" args.[i])
            elif arg = "--h" then
                showHelp <- true
            elif arg = "--dt" then
                addDatetime <- true
            elif arg = "--delim" then
                i <- i + 1
                if i < args.Length then delim <- args.[i]
            else
                log LogLevel.Error (sprintf "Unknown option: %s" arg)
            i <- i + 1
        (inputFile, outputFile, showHelp, delim)





    // THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
    #if INTERACTIVE
    printfn "Source File: %s" __SOURCE_FILE__
    printfn "hello from the end of Clicommon.fsx"
    // let NESTED =
    //     (let frames = System.Diagnostics.StackTrace().GetFrames()
    //                  |> Array.map (fun frame -> frame.GetMethod().Name)
    //     frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
    if NESTED then printfn "nested" else printfn "not nested"
    #else
    #endif