#!/usr/bin/env -S dotnet fsi
#if INTERACTIVE
//module CliCommon
#else
module CliCommon
#endif 
// cli-common.fsx
// The mutable sharable common code of all cli apps, DRY
// All common side-effects including logging, common IO, etc

// THIS IS WHERE ANYTHING COMMON ACROSS THE FRAMEWORK GOES


// THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
#if INTERACTIVE
printfn "Source File: %s" __SOURCE_FILE__
printfn "hello from the beginning of clicommon.fsx"
let NESTED =
    (let frames = System.Diagnostics.StackTrace().GetFrames()
                 |> Array.map (fun frame -> frame.GetMethod().Name)
    frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
if NESTED then printfn "nested" else printfn "not nested"
#endif


open System
open System.Text.RegularExpressions
type LogLevel =
    | Info = 1
    | Warn = 2
    | Error = 3


let poppo="dog"


let mutable verbosity = LogLevel.Error
let mutable addDatetime = false

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


// THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
#if INTERACTIVE
printfn "Source File: %s" __SOURCE_FILE__
printfn "hello from the end of clicommon.fsx"
// let NESTED =
//     (let frames = System.Diagnostics.StackTrace().GetFrames()
//                  |> Array.map (fun frame -> frame.GetMethod().Name)
//     frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
if NESTED then printfn "nested" else printfn "not nested"
#else
#endif