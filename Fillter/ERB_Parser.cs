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
                    int count = -1;
                    bool can_exit = false;
                    #region 해석스레드
                    Thread de_thread = new Thread(() =>
                    {
                        while (true)
                        {
                            lock (this)
                            {
                                while (buffer.Count == 0)
                                {
                                    can_exit = true;
                                    Monitor.Wait(this);
                                }
                                can_exit = false;
                                string printPattern = @"^(?<NonString>\s*PRINT[^ ]* ?)(?<String>.+)?$";
                                string dataPattern = @"^(?<NonString>\s*DATA(FORM)? ?)(?<String>.+)?$";
                                for (int i = 0; i < buffer.Count; i++)
                                {
                                    ERBInfo erbinfo = buffer.Dequeue();
                                    if (!erbinfo.DATA)
                                    {
                                        Match printMatch = Regex.Match(erbinfo.Text, printPattern);
                                        StringDictionary.Add(erbinfo.LineNo, new LineInfo(printMatch.Groups["String"].Value, erbinfo.Text.Contains("FORM"), erbinfo.Text.Contains("FORMS")));
                                        NonStringDictionary.Add(erbinfo.LineNo, printMatch.Groups["NonString"].Value);
                                    }
                                    else
                                    {
                                        foreach (var listinfo in erbinfo.Data_list)
                                        {
                                            for (int a = 1; a <= listinfo.Forms.Length; a++)
                                            {
                                                if (listinfo.Forms[a - 1].Contains("DATAFORM"))
                                                {
                                                    Match dataMatch = Regex.Match(listinfo.Forms[a - 1], dataPattern);
                                                    StringDictionary.Add(listinfo.Line + a, new LineInfo(dataMatch.Groups["String"].Value, listinfo.Line, true, false));
                                                    NonStringDictionary.Add(listinfo.Line + a, dataMatch.Groups["NonString"].Value);
                                                }
                                                else if (listinfo.Forms[a - 1].Contains("DATA"))
                                                {
                                                    string[] line = listinfo.Forms[a - 1].Split(new string[] { "DATA" }, StringSplitOptions.None);
                                                    string _str = line[0] + "DATA";
                                                    string str = line[1] ?? "";
                                                    StringDictionary.Add(listinfo.Line + a, new LineInfo(str, listinfo.Line, false, false));
                                                    NonStringDictionary.Add(listinfo.Line + a, _str);
                                                }
                                                else
                                                {
                                                    throw new Exception("DATALIST를 인식할수 없습니다");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    });
                    #endregion
                    de_thread.Start();//스레드 시작
                    while (!reader.EndOfStream)
                    {
                        string temp = reader.ReadLine();
                        count++;
                        OriginalTexts.Add(temp);//원본 저장
                        temp = temp.Contains(';') ? temp.Substring(0, temp.IndexOf(';')) : temp;//주석제거
                        if (temp.Contains("PRINT"))
                        {
                            if (temp.Contains("PRINTDATA"))//DATA형
                            {
                                List<DataListInfo> printdata = new List<DataListInfo>();
                                List<string> datalist = new List<string>();
                                int line = count;
                                bool exit = false;
                                bool list = false;
                                while (!exit)
                                {
                                    temp = reader.ReadLine();
                                    count++;
                                    OriginalTexts.Add(temp);
                                    switch (temp.Split(' ')[0].Trim())
                                    {
                                        case ("DATALIST"):
                                            {
                                                list = true;
                                                line = count;
                                                break;
                                            }
                                        case ("DATA"):
                                            {
                                                datalist.Add(temp);
                                                break;
                                            }
                                        case ("DATAFORM"):
                                            {
                                                datalist.Add(temp);
                                                break;
                                            }
                                        case ("ENDLIST"):
                                            {
                                                printdata.Add(new DataListInfo(datalist.ToArray(), line));
                                                datalist.Clear();
                                                break;
                                            }
                                        case ("ENDDATA"):
                                            {
                                                if (!list)
                                                {
                                                    printdata.Add(new DataListInfo(datalist.ToArray(), line));
                                                    datalist.Clear();
                                                }
                                                exit = true;
                                                break;
                                            }
                                        default:
                                            {
                                                throw new Exception("구문분석중 오류가 발생되었습니다.\r\n줄수:" + count + "\r\n처리중인 문자열:" + temp);
                                            }
                                    }
                                }
                                lock (this)
                                {
                                    buffer.Enqueue(new ERBInfo(printdata));
                                    Monitor.Pulse(this);
                                }
                            }
                            else {//일반 PRINT문
                                lock (this)
                                {
                                    buffer.Enqueue(new ERBInfo(temp, count));
                                    Monitor.Pulse(this);
                                }
                            }
                        }
                    }
                    while (!can_exit) { Thread.Sleep(10); }//버퍼처리가 끝날때까지 기다림 
                    de_thread.Abort();
                }
            }
            return;
        }

        Queue<ERBInfo> buffer = new Queue<ERBInfo>();
        public void Save()
        {
            using (FileStream ErbStream = new FileStream(ErbPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(ErbStream, ErbEncoding))
                {
                    foreach (var a in StringDictionary)
                    {
                        string temp = NonStringDictionary[a.Key] + a.Value.Str;
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
        private List<string> originalTexts = new List<string>();
        private Dictionary<int, LineInfo> stringDictionary = new Dictionary<int, LineInfo>();
        private Dictionary<int, string> nonStringDictionary = new Dictionary<int, string>();
        private string erbPath;
        private Encoding erbEncoding;

        public List<string> OriginalTexts
        {
            get
            {
                return originalTexts;
            }

            set
            {
                originalTexts = value;
            }
        }

        public Dictionary<int, LineInfo> StringDictionary
        {
            get
            {
                return stringDictionary;
            }

            set
            {
                stringDictionary = value;
            }
        }

        public Dictionary<int, string> NonStringDictionary
        {
            get
            {
                return nonStringDictionary;
            }

            set
            {
                nonStringDictionary = value;
            }
        }

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

        class DataListInfo
        {
            public string[] Forms { get; }
            public int Line { get; }
            public DataListInfo(string[] forms, int line)
            {
                Forms = forms;
                Line = line;
                //line:부모줄번호
            }
        }
        class ERBInfo
        {
            public string Text { get; }

            public int LineNo { get; }

            public bool DATA { get; private set; }

            public List<DataListInfo> Data_list { get; }

            public ERBInfo(string text, int line)
            {
                Text = text;
                LineNo = line;
                DATA = false;
            }
            public ERBInfo(List<DataListInfo> data_list)
            {
                Data_list = data_list;
                DATA = true;
            }
        }
    }
    public class LineInfo
    {
        private bool korean;
        private bool japanese;
        private string str;

        public string Str
        {
            get
            {
                return str;
            }
            set
            {
                str = value;
                GetLang.Get(value, out korean, out japanese);
            }
        }

        public bool IsForm { get; }
        public bool IsFormS { get; }
        public bool IsList { get; }
        public int ParentLine { get; }
        public bool Korean => korean;
        public bool Japanese => japanese;

        public LineInfo(string str, bool isForm, bool isFormS)
        {
            Str = str;
            IsList = false;
            IsForm = !isFormS && isForm;
            IsFormS = isFormS;
        }
        public LineInfo(string str, int parentLine, bool isForm, bool isFormS)
        {
            //parent_line 부모 DATALIST의 줄수이며 이것으로 같은 FORM인지 구분
            Str = str;
            ParentLine = parentLine;
            IsList = true;
            IsForm = isForm;
            IsFormS = isFormS;
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
