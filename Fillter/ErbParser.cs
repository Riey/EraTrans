using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace YeongHun.EraTrans
{
    public class ErbParser
    {
        public ErbParser(string erb_path):this(erb_path,Encoding.Unicode)
        {
            
        }
        public ErbParser(string erb_path,Encoding encoding)
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
                StreamReader reader = encoding != null ? new StreamReader(ErbStream, encoding) : new StreamReader(ErbStream, true);
                {
                    ErbEncoding = reader.CurrentEncoding;
                    int lineNo = -1;

                    string originalText = null;
                    string temp = null;

                    Match ParseNextLine()
                    {
                        temp = reader.ReadLine();
                        lineNo++;
                        var original = IsOriginalLine(temp);
                        if (original.Success)
                        {
                            originalText = original.Groups[1].Value;
                            temp = reader.ReadLine();
                            lineNo++;
                        }
                        else
                        {
                            originalText = null;
                        }
                        OriginalTexts.Add(lineNo, temp);
                        return ParseLine(temp);
                    }

                    while (!reader.EndOfStream)
                    {
                        var match = ParseNextLine();

                        if (match.Success)
                        {
                            string code = match.Groups["FunctionCode"].Value;
                            if (Regex.IsMatch(code, "PRINTDATA[KD]?[LW]?"))//PRINTDATA문
                            {
                                int printDataLine = lineNo;
                                int listLine = -1;
                                bool exit = false;
                                while (!exit)
                                {
                                    match = ParseNextLine();
                                    if (!match.Success)
                                        continue;
                                    switch (match.Groups["FunctionCode"].Value)
                                    {
                                        case "DATALIST":
                                            {
                                                listLine = lineNo;
                                                break;
                                            }
                                        case "DATA":
                                            {
                                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, false, originalText, printDataLine, listLine));
                                                break;
                                            }
                                        case "DATAFORM":
                                            {
                                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, true, originalText, printDataLine, listLine));
                                                break;
                                            }
                                        case "ENDLIST":
                                            {
                                                listLine = -1;
                                                break;
                                            }
                                        case "ENDDATA":
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
                            else if (Regex.IsMatch(code, "PRINTBUTTON[C(LC)]"))
                            {
                                var buttonMatch = Regex.Match(match.Groups["Content"].Value, @"""(?<PrintText>[^""]*)(?<Right>"".+)");
                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value + "\"", buttonMatch.Groups["Right"].Value + match.Groups["Right"].Value));
                                StringDictionary.Add(lineNo, new LineInfo(buttonMatch.Groups["PrintText"].Value, false, false, originalText));
                            }
                            else
                            {//일반 PRINT문
                                NonStringDictionary.Add(lineNo, Tuple.Create(match.Groups["Left"].Value, match.Groups["Right"].Value));
                                StringDictionary.Add(lineNo, new LineInfo(match.Groups["Content"].Value, code.Contains("FORM"), code.Contains("FORMS"),originalText));
                            }

                        }
                    }
                }
            }
            return;
        }

        private Regex originalRegex = new Regex(@";OriginalString : (?<Original>.*)", RegexOptions.Compiled);
        public Match IsOriginalLine(string rawLine)
        {
            return originalRegex.Match(rawLine);
        }

        private Regex printLineRegex = new Regex(@"^(?<Left>\s*(?<FunctionCode>(PRINTSINGLE(V|S|(FORM)|(FORMS))?[KD]?)|(PRINTBUTTON(C|(LC))?)|(PRINTPLAIN(FORM)?)|(PRINTDATA[KD]?[LW]?)|(PRINT(V|S|(FORM)|(FORMS))?[KD]?[LW]?)|(DATA((FORM)|(LIST))?)|(ENDDATA)|(ENDLIST)) ?)(?<Content>[^;]+)?(?<Right>;.*)?$", RegexOptions.Compiled);
        public Match ParseLine(string rawLine)
        {
            return printLineRegex.Match(rawLine);
        }

        public enum OutputType
        {
            Working, Release
        }

        public void Save(OutputType type)
        {
            using (FileStream ErbStream = new FileStream(ErbPath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(ErbStream, ErbEncoding))
                {
                    switch (type)
                    {
                        case OutputType.Working:
                            foreach (var original in OriginalTexts)
                            {
                                int lineNo = original.Key;
                                if (StringDictionary.ContainsKey(lineNo))
                                {
                                    writer.WriteLine(";OriginalString : " + StringDictionary[lineNo].OriginalString);
                                    writer.WriteLine(NonStringDictionary[lineNo].Item1 + StringDictionary[lineNo].Str + NonStringDictionary[lineNo].Item2);
                                }
                                else
                                {
                                    writer.WriteLine(OriginalTexts[lineNo]);
                                }
                            }
                            break;
                        case OutputType.Release:
                            foreach (var original in OriginalTexts)
                            {
                                int lineNo = original.Key;
                                if (StringDictionary.ContainsKey(lineNo))
                                {
                                    writer.WriteLine(NonStringDictionary[lineNo].Item1 + StringDictionary[lineNo].Str + NonStringDictionary[lineNo].Item2);
                                }
                                else
                                {
                                    writer.WriteLine(OriginalTexts[lineNo]);
                                }
                            }
                            break;
                    }
                    writer.Flush();
                }
            }
        }
        private string erbPath;
        private Encoding erbEncoding;

        private Dictionary<int,string> OriginalTexts { get; set; } = new Dictionary<int, string>();

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
