namespace Fillter2.Parsing

open System
open System.Xml



type LineInfo(rawLine : string) = 
    
    let func = rawLine.Split(' ').[0]
    let isForm = func.Contains("FORM")
    let isPrint = not(func.Contains("PRINTDATA")) && func.Contains("PRINT")

    let mutable printStr = rawLine.Substring(func.Length)
    
    
    member public this.Function = func
    member public this.PrintStr
        with get() = printStr
        and set(value) = printStr <- value
    member public this.IsForm = isForm
    member public this.IsKorean = Language.CheckKorean(printStr)
    member public this.IsJapanese = Language.CheckJapanese(printStr)
    member public this.AddXmlElement(doc:XmlDocument) (topNode:XmlNode) =
        let element = doc.CreateElement("EraLine")
        let isFormAttr = doc.CreateAttribute("IsForm");

        isFormAttr.Value <- this.IsForm.ToString()
        element.Attributes.Append isFormAttr |> ignore

        let terms =
            match isForm with
                |true -> printStr.Split('%')
                |false -> [| printStr |]
        
        let rec AddElement n =
            if not (n = terms.Length) then
                element.AppendChild(
                    match n with
                        |_ when n % 2 = 0 -> doc.CreateElement("Plain")
                        |_ -> doc.CreateElement("Form"))

                        |>ignore
                AddElement (n + 1)

        AddElement 0
        
        topNode.AppendChild(element)
                

    abstract member IsVaild:bool

    default this.IsVaild = isPrint

    override this.ToString() = func + " " + printStr

type DataLineInfo(rawLine:string) =
    inherit LineInfo(rawLine)
    let isData = 
        match base.Function with
            |"DATALIST" -> false
            |"ENDDATA" -> false
            |"DATA"|"DATAFORM" -> true
            |_ -> false
    override this.IsVaild = isData
