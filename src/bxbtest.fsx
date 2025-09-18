#!/usr/bin/env -S dotnet fsi
open System
open System.Text.RegularExpressions
open System.IO
open Microsoft.FSharp.Compiler.Interactive.Settings

#if INTERACTIVE
#load "clicommon.fsx"
#load "bxblib.fsx"
open Clicommon 
open Bxblib 
#else
open Clicommon 
open Bxblib 
#endif 
module Bxbtestfsx=

  #if INTERACTIVE
  printfn "Source File: %s" __SOURCE_FILE__
  printfn "hello from the beginning of bxbtest.fsx"
  let NESTED =
      (let frames = System.Diagnostics.StackTrace().GetFrames()
                  |> Array.map (fun frame -> frame.GetMethod().Name)
      frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
  if NESTED then printfn "nested" else printfn "not nested"
  #else
  #endif

  // bxb.fsx
  // Main script for bxb. Loads the shared library and handles CLI I/O, options, logging, and tests.
  // Designed to be as functional as possible while handling imperative I/O.
  let appName = "bxbtest"



let parseArgs (args: string[]) =
    let mutable i = 0
    while i < args.Length do
        let arg = args.[i]
        if arg = "--i" then
            i <- i + 1
            if i < args.Length then Clicommon.inputFile <- Some args.[i]
        elif arg = "--o" then
            i <- i + 1
            if i < args.Length then Clicommon.outputFile <- Some args.[i]
        elif arg = "--v" then
            i <- i + 1
            if i < args.Length then
                match args.[i].ToUpper() with
                | "INFO" -> Clicommon.verbosity <- Clicommon.LogLevel.Info
                | "WARN" -> Clicommon.verbosity <- Clicommon.LogLevel.Warn
                | "ERROR" -> Clicommon.verbosity <- Clicommon.LogLevel.Error
                | _ -> Clicommon.log Clicommon.LogLevel.Error (sprintf "Invalid verbosity level: %s" args.[i])
        elif arg = "--h" then
            Clicommon.showHelp <- true
        elif arg = "--dt" then
            Clicommon.addDatetime <- true
        elif arg = "--delim" then
            i <- i + 1
            if i < args.Length then Clicommon.delim <- args.[i]
        else
            Clicommon.log Clicommon.LogLevel.Error (sprintf "Unknown option: %s" arg)
        i <- i + 1
    (Clicommon.inputFile, Clicommon.outputFile, Clicommon.showHelp, Clicommon.delim)

let helpText = """
Usage: cliword [options]

Options:
  --i <file>   Input file (default: stdin)
  --o <file>   Output file (default: stdout)
  --v <level>  Verbosity level: INFO, WARN, ERROR (default: ERROR)
  --dt         Enable high precision UTC datetime prefix on log messages
  --delim <regex> Input delimiter as regex pattern (default: \t)
  --h          Show this help

This program reads delimited text, processes it by reversing the order of clusters in each line and reversing the words in each cluster, and outputs the result with tab as delimiter.
It operates in streaming mode, processing line by line.

Examples:
In DOS:
  cliword.cmd --i cliwordsample.txt --o output.txt
  type cliwordsample.txt | cliword.cmd > output.txt
  cliword.cmd --delim "," --i input.csv --o output.txt

In Bash:
  ./cliword --i cliwordsample.txt --o output.txt
  cat cliwordsample.txt | ./cliword > output.txt
  ./cliword --delim "," --i input.csv --o output.txt

If no input is provided, it reads from stdin, which may cliwordear as hanging if waiting for keyboard input.
"""

let main () =
    let args = 
        #if INTERACTIVE
        fsi.CommandLineArgs |> Array.tail
        #else
        Environment.GetCommandLineArgs() |> Array.tail
        #endif
    let inputFile, outputFile, showHelp, delim = parseArgs args
    if showHelp then
        Console.Out.WriteLine helpText
    let reader : TextReader =
        match inputFile with
        | Some file ->
            try
                new StreamReader(file)
            with
            | ex ->
                Clicommon.log Clicommon.LogLevel.Error (sprintf "Error opening input file %s: %s. Falling back to stdin." file ex.Message)
                Console.In
        | None ->
            Console.In
    let writer : TextWriter =
        match outputFile with
        | Some file ->
            try
                new StreamWriter(file)
            with
            | ex ->
                Clicommon.log Clicommon.LogLevel.Error (sprintf "Error opening output file %s: %s. Falling back to stdout." file ex.Message)
                Console.Out
        | None ->
            Console.Out
    try
        try
            let mutable line = reader.ReadLine()
            while line <> null do
                //let fields = Regex.Split(line, delim)
                let processed = Bxblib.processAppDataLine line delim 
                let outLine = String.Join("\t", processed)
                writer.WriteLine outLine
                line <- reader.ReadLine()
        with
        | ex ->
            Clicommon.log Clicommon.LogLevel.Error (sprintf "Error during processing: %s" ex.Message)
    finally
        if reader <> Console.In then reader.Close()
        if writer <> Console.Out then writer.Close()



// THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
// THE TOP LEVEL FSX NEEDS TO HAVE THE NESTED CHECK UNCOMMENTED
#if INTERACTIVE
printfn "Source File: %s" __SOURCE_FILE__
printfn "hello from the end of bxbtest.fsx"
let NESTED =
    (let frames = System.Diagnostics.StackTrace().GetFrames()
                 |> Array.map (fun frame -> frame.GetMethod().Name)
    frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
if NESTED then printfn "nested" else printfn "not nested"
#else
#endif





main ()

