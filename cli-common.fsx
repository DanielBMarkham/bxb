#if INTERACTIVE
#else
module CliCommon
#endif 
// cli-common.fsx
// The mutable sharable common code of all cli apps, DRY
// All common side-effects including logging, common IO, etc

// THIS IS WHERE ANY ACTUAL REAL CODE GOES

open System
open System.Text.RegularExpressions

