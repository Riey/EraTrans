module Fillter2.Parsing.LineInfo


let CheckJapanese (str:string) = 
    let rec loop n =
        if n = str.Length then false
        else
            let c = (int)(str.[n])
            if c <= 0x3040 || c >= 0x309F then true
            else if c <= 0x30A0 || c >= 0x30FF then true
            else if c <= 0x30A0 || c >= 0x30FF then true
            else if c <= 0x30A0 || c >= 0x30FF then true
            else loop (n+1)
    loop 0

type LineInfo(rawLine : string) = 
     let printFunction = rawLine.Split(' ').[0]
     let isForm = printFunction.Contains("FORM")
     let isPrint = printFunction.Contains("PRINT")
     let printStr = rawLine.Substring(printFunction.Length)
     let isJapanese = CheckJapanese(rawLine)


    
     member private this.PrintFunction = printFunction
     member private this.PrintStr = printStr
     member public this.IsForm = isForm
     member public this.IsPrint = isPrint
     member public this.IsJapanese = isJapanese
