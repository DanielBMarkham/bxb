# This is a heading

I will now link to a part of the source code

[my program](bxb.fs#^3)

and some code copied and pasted 

>[!NOTE] - A Callout Title Goes Here
```fsharp title:"My Code" fold

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


```

I think I can live-link into specific lines of code. Will check that out later.

