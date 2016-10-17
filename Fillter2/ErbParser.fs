namespace Fillter2.Parsing


open System
open System.IO
open System.Collections.Generic


type ErbParser(reader : StreamReader) = 
            
    let mutable rawLines:string[] = null
    let printLines = Dictionary<int, LineInfo>()
    let leftLines = Dictionary<int, Tuple<string, string>>()


    do
        let tempLines = List<string>()
        let mutable lineNo = 0
        while not reader.EndOfStream do
            let mutable line = reader.ReadLine()
            let mutable left = ""
            let mutable right = ""
            tempLines.Add(line)

            match line.Contains(";") with
                |false -> right <- ""
                |true ->
                    right <- line.Substring(line.IndexOf(';'))
                    line <- line.Substring(0, line.IndexOf(';'))

            match line = String.Empty with
                |true
                |false ->
                    let trimmed = line.TrimStart()
                    left <- line.Substring(0, (line.Length - trimmed.Length))
                    let info = LineInfo(line.TrimStart())
                    match info.IsVaild with
                        |false -> 
                            let dataInfo = DataLineInfo(line)
                            if dataInfo.IsVaild then
                                printLines.Add(lineNo, dataInfo)
                                leftLines.Add(lineNo, Tuple<string,string>(left, right))
                        |true ->
                            printLines.Add(lineNo, info)
                            leftLines.Add(lineNo, Tuple<string,string>(left, right))
            
            lineNo <- (lineNo + 1)
        rawLines <- tempLines.ToArray()

    member public this.PrintLines = printLines
    member public this.Save(path:string) =
        use stream = new FileStream(path, FileMode.Create)
        let writer = new StreamWriter(stream)
        let length = rawLines.Length
        let rec WriteLine lineNo = 
            match lineNo with
            |_ when lineNo = length -> ()
            |_ when this.PrintLines.ContainsKey lineNo -> 
                let left = leftLines.[lineNo]
                writer.Write left.Item1
                writer.Write printLines.[lineNo]
                writer.WriteLine left.Item2
            |_ -> writer.WriteLine rawLines.[lineNo]
        WriteLine 0
        writer.Flush|>ignore