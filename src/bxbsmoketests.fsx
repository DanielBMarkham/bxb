#!/usr/bin/env -S dotnet fsi
open System
open System.Text.RegularExpressions
open System.IO
open Microsoft.FSharp.Compiler.Interactive.Settings

#if INTERACTIVE
//#load "clicommon.fsx"
//#load "bxblib.fsx"
//open Clicommon 
//open Bxblib 
#else
//open Clicommon 
//open Bxblib 
#endif 
module Bxbsmoketests=

  #if INTERACTIVE
  printfn "Source File: %s" __SOURCE_FILE__
  printfn "hello from the beginning of bxbsmoketests.fsx"
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
  let appName = "bxbsmoketests"



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


  // THERE ARE TWO DIFFERENT TYPES OF SMOKE TESTS
  // TESTING TO SEE IF ANY COMMAND LINE INPUT CAN CRASH THE PROGRAM
  // TESTING TO SEE IF THE PIPE GOING IN AND OUT OF BXBLIB IS
  // WORKING GIVEN THE DEFAULT NOTIONAL FUNCTIONALITY AND
  // VARIOUS TRIPLETS OF INPUT, PARAMETERS, AND EXPECTED OUTPUTS


/// Can any possible combaintion of CLI Parameters crash the program?
  type CLIResillienceSmokeTest={
    TestDescription:string
    DosCliCommand:string
    UnixCommand:string 
    }

  // Does the IO pipe from anywhere make it to and from BxbEntry point?
  type DefaultFunctionalityIOPipingSmokeTest={
    TestDescription:string
    DosCliCommand:string
    UnixCommand:string
    SampleInputFile:string
    ExpectedOutputFile:string 
    }



  /// Smoke tests determine if all the scaffolding is working
  /// so that the user can begin coding their app at the main
  /// BxbLib entry point without worrying about any other code
  /// on the stack
  let runSmokeTests()=
    printfn "This is a very exciting function smoke testing"


  // THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
  // THE TOP LEVEL FSX NEEDS TO HAVE THE NESTED CHECK UNCOMMENTED
  #if INTERACTIVE
  printfn "Source File: %s" __SOURCE_FILE__
  printfn "hello from the end of bxbsmoketests.fsx"
  let NESTED2 =
      (let frames = System.Diagnostics.StackTrace().GetFrames()
                  |> Array.map (fun frame -> frame.GetMethod().Name)
      frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
  if NESTED2 then printfn "nested" else printfn "not nested"

  runSmokeTests()
  #else
  #endif

