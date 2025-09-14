#!/usr/bin/env -S dotnet fsi
#if INTERACTIVE
#else
module BxbLib
#endif 
// bxb-lib.fsx
// Purely functional library for bxb and bxb-test
// No side effects, no I/O, no logging

// THIS IS WHERE ANY ACTUAL REAL CODE GOES

// THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
#if INTERACTIVE
printfn "Source File: %s" __SOURCE_FILE__
printfn "hello from the beginning of bxb-lib.fsx"
let NESTED =
    (let frames = System.Diagnostics.StackTrace().GetFrames()
                 |> Array.map (fun frame -> frame.GetMethod().Name)
    frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
if NESTED then printfn "nested" else printfn "not nested"
open Microsoft.FSharp.Compiler.Interactive.Settings
#load "clicommon.fsx"
#else
  open CliCommon
#endif

open System
open System.Text.RegularExpressions
open System.IO

/// Reverses the columns in a single line based on the delimiter regex pattern.
/// Splits the line using the regex with capturing groups to preserve delimiters, reverses the columns, and joins back with reversed delimiters.
let private reverseColumnsLine (line: string) (delim: string) : string =
    if String.IsNullOrEmpty line then ""
    else
        let pattern = "(" + delim + ")"
        let regex = Regex(pattern)
        let parts = regex.Split(line)
        if parts.Length = 1 then line
        else
            let columns = [ for i in 0 .. 2 .. parts.Length - 1 -> parts.[i] ]
            let delims = if parts.Length > 1 then [ for i in 1 .. 2 .. parts.Length - 2 -> parts.[i] ] else []
            let rev_columns = List.rev columns
            let rev_delims = List.rev delims
            let sb = System.Text.StringBuilder()
            for i in 0 .. rev_columns.Length - 1 do
                sb.Append(rev_columns.[i]) |> ignore
                if i < rev_columns.Length - 1 then
                    sb.Append(rev_delims.[i]) |> ignore
            sb.ToString()

/// Pure function to process a single line for app2 (initially reverses columns).
let processAppDataLine (line: string) (delim: string) : string =
    reverseColumnsLine line delim

/// Pure function to process a single line for app2-test (initially reverses columns).
let processAppTestDataLine (line: string) (delim: string) : string =
    reverseColumnsLine line delim

// THIS IS THE DANCE WE NEED TO DO IN ORDER TO GET NESTING WORKING
#if INTERACTIVE
printfn "Source File: %s" __SOURCE_FILE__
printfn "hello from the end of bxb-lib.fsx"
// let NESTED =
//     (let frames = System.Diagnostics.StackTrace().GetFrames()
//                  |> Array.map (fun frame -> frame.GetMethod().Name)
//     frames.[0] = "main@" && not (Array.contains "EvalParsedSourceFiles" frames))=false 
if NESTED then printfn "nested" else printfn "not nested"
#else
#endif
