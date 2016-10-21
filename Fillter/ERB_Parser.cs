using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Fillter
{
    public class ERB_Parser
    {
        public ERB_Parser(string erb_path):this(erb_path,Encoding.Unicode)
        {
            
        }
        public ERB_Parser(string erb_path,Encoding encoding)
        {
            this.ErbPath = erb_path;
            if (!File.Exists(erb_path))
            {
                throw new FileNotFoundException();
            }
            FileInfo info = new FileInfo(erb_path);
            if (!File.Exists(Application.StartupPath + "\\Backup\\" + info.Name))
            {
                info.CopyTo(Application.StartupPath + "\\Backup\\" + info.Name);
            }
            using (FileStream ErbStream = info.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //string plainPattern = @"(?<LeftBlank>\s*)(?<FunctionCode>PRINT[^(FORM)(FORMS) ]*)( (?<Text>[^;]+))?(?<RightComment>;.+)?";
                //string formPattern = @"(?<LeftBlank>\s*)(?<FunctionCode>PRINTFORM[^ S]*)( (?<Text>[^;]+))?(?<RightComment>;.+)?";
                //string formsPattern = @"(?<LeftBlank>\s*)(?<FunctionCode>PRINTFORMS[^ ]*)( (?<Text>[^;]+))?(?<RightComment>;.+)?";

                StreamReader reader = encoding != null ? new StreamReader(ErbStream, encoding) : new StreamReader(ErbStream, true);
                {
                    ErbEncoding = reader.CurrentEncoding;
                    int lineNo = -1;

                    while (!reader.EndOfStream)
                    {
                        string temp = reader.ReadLine();
                        lineNo++;
                        OriginalTexts.Add(temp);//원본 저장

                        var match = ParseLine(temp);

                        if (match.Success)
                        {
                            if (match.Groups["FunctionCode"].Value == "PRINTDATA")//PRINTDATA문
                            {
                                int printDataLine = lineNo;
                                int listLine = -1;
                                bool exit = false;
                                while (!exit)
                                {
                                    temp = reader.ReadLine();
                                    lineNo++;
                                    OriginalTexts.Add(temp);
                                    match = ParseLine(temp);
                                    if (!match.Success)
                                        continue;
                                    switch (match.Groups["FunctionCode"].Value)
                                    {
                                        case ("DATALIST"):
                                            {
                                                listLine = lineNo;
                                                break;
                                            }
                                        case ("DATA"):
                                            {
                                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, false, printDataLine, listLine));
                                                break;
                                            }
                                        case ("DATAFORM"):
                                            {
                                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, true, printDataLine, listLine));
                                                break;
                                            }
                                        case ("ENDLIST"):
                                            {
                                                listLine = -1;
                                                break;
                                            }
                                        case ("ENDDATA"):
                                            {
                                                exit = true;
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("구문분석중 오류가 발생되었습니다.\r\n줄수:" + lineNo + "\r\n처리중인 문자열:" + temp);
                                            }
                                    }
                                }

                            }
                            else
                            {//일반 PRINT문
                                string code = match.Groups["FunctionCode"].Value;
                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, code.Contains("FORM"), code.Contains("FORMS")));
                            }
                        }
                    }
                }
            }
            return;
        }

        public Match ParseLine(string rawLine)
        {
            return Regex.Match(rawLine, @"(?<Left>\s*(?<FunctionCode>(PRINT[^\s;]*)|(DATA((FORM)|(LIST))?)|(ENDDATA)|(ENDLIST)) ?)(?<Content>[^;]+)?(?<Right>;.*)?");
        }
        
        public void Save()
        {
            using (FileStream ErbStream = new FileStream(ErbPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(ErbStream, ErbEncoding))
                {
                    foreach (var a in StringDictionary)
                    {
                        string temp = NonStringDictionary[a.Key].Item1 + a.Value.Str + NonStringDictionary[a.Key].Item2;
                        OriginalTexts[a.Key] = temp;
                    }
                    foreach (var a in OriginalTexts)
                    {
                        writer.WriteLine(a);
                    }
                    writer.Flush();
                }
            }
        }
        private string erbPath;
        private Encoding erbEncoding;

        private List<string> OriginalTexts { get; set; } = new List<string>();

        public Dictionary<int, LineInfo> StringDictionary { get; private set; } = new Dictionary<int, LineInfo>();

        public Dictionary<int, Tuple<string, string>> NonStringDictionary { get; private set; } = new Dictionary<int, Tuple<string, string>>();

        public string ErbPath
        {
            get
            {
                return erbPath;
            }

            set
            {
                erbPath = value;
            }
        }

        public Encoding ErbEncoding
        {
            get
            {
                return erbEncoding;
            }

            set
            {
                erbEncoding = value;
            }
        }
    }
    static class GetLang
    {
        public static void Get(string str, out bool Korean, out bool Japanese)
        {
            Korean = false;
            Japanese = false;
            foreach (var a in str)
            {
                if ((a >= 0x3040 && a <= 0x30FF) || (a >= 0x4E00 && a <= 0x9FAF))
                    Japanese = true;
                if ((a >= 0x1100 && a <= 0x11FF) || (a >= 0x3131 && a <= 0x318F) || (a >= 0xAC00 && a <= 0xD7FF))
                    Korean = true;
            }
        }
    }
}
