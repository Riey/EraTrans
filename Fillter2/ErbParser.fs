module Fillter2.Parsing.ErbParser


open System
open System.IO
open System.Collections.Generic
open Fillter2.Parsing.LineInfo


type ErbParser(erbStream : Stream) = 
            
    let mutable rawLines:string[] = null
    let mutable printLines = Dictionary<int64, LineInfo>()
    do
        let reader = new StreamReader(erbStream)
        let tempLines = List<string>()
        let mutable lineNo = 0L
        while not reader.EndOfStream do
            let mutable line = reader.ReadLine()
            lineNo <- (lineNo + 1L)
            tempLines.Add(line)
            match line.Contains(";") with
                |false
                |true -> line <- line.Substring(line.IndexOf(';'))
            match line = String.Empty with
                |true
                |false ->
                    line <- line.TrimStart()
                    let info = LineInfo(line)
                    match info.IsPrint with
                        |false
                        |true -> printLines.Add(lineNo, info)

        rawLines <- tempLines.ToArray()

    member public this.PrintLines = printLines
