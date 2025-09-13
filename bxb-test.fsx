// bxb-test.fsx
// Main script for bxb-test. Loads the shared library and handles CLI I/O, options, logging, and tests.
// Designed to be as functional as possible while handling imperative I/O.

#load "bxb-lib.fsx"

open System
open System.IO
open BxbLib
open Microsoft.FSharp.Compiler.Interactive.Settings

let appName = "bxb-txt"

/// Log levels for verbosity control.
type LogLevel = 
    | Info 
    | Warn 
    | Error

    static member ToInt = function Info -> 0 | Warn -> 1 | Error -> 2
    static member ToString = function Info -> "INFO" | Warn -> "WARN" | Error -> "ERROR"

/// Logs a message if the message level meets or exceeds the user-specified verbosity level.
let logMsg (userLevel: LogLevel) (useDt: bool) (level: LogLevel) (msg: string) : unit =
    if LogLevel.ToInt level >= LogLevel.ToInt userLevel then
        let prefix = if useDt then DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff") + " " else ""
        let levelStr = LogLevel.ToString level
        Console.Error.WriteLine($"{prefix}{levelStr}: {msg}")

printfn "Hello World from bxb-test"