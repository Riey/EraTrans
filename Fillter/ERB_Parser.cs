using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
                                for (int i = 0; i < buffer.Count; i++)
                                {
                                    ERBInfo erbinfo = buffer.Dequeue();
                                    if (!erbinfo.DATA)
                                    {
                                        string temp = erbinfo.Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                                        StringDictionary.Add(erbinfo.Line, new LineInfo(erbinfo.Text.Replace(temp, "")));
                                        NonStringDictionary.Add(erbinfo.Line, temp);
                                    }
                                    else
                                    {
                                        foreach (var listinfo in erbinfo.Data_list)
                                        {
                                            for (int a = 1; a <= listinfo.Forms.Length; a++)
                                            {
                                                if (listinfo.Forms[a - 1].Contains("DATAFORM"))
                                                {
                                                    string[] line = listinfo.Forms[a - 1].Split(new string[] { "DATAFORM" }, StringSplitOptions.None);
                                                    string _str = line[0] + "DATAFORM";
                                                    string str = line[1] ?? "";
                                                    StringDictionary.Add(listinfo.Line + a, new LineInfo(str, listinfo.Line));
                                                    NonStringDictionary.Add(listinfo.Line + a, _str);
                                                }
                                                else if (listinfo.Forms[a - 1].Contains("DATA"))
                                                {
                                                    string[] line = listinfo.Forms[a - 1].Split(new string[] { "DATA" }, StringSplitOptions.None);
                                                    string _str = line[0] + "DATA";
                                                    string str = line[1] ?? "";
                                                    StringDictionary.Add(listinfo.Line + a, new LineInfo(str, listinfo.Line));
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
            var ErbStream = new FileStream(ErbPath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            using(StreamWriter writer = new StreamWriter(ErbStream, ErbEncoding))
            {
                foreach(var a in StringDictionary)
                {
                    string temp = NonStringDictionary[a.Key] + a.Value.Str;
                    OriginalTexts[a.Key] = temp;
                }
                foreach(var a in OriginalTexts)
                {
                    writer.WriteLine(a);
                }
                writer.Flush();
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
            private string text;
            private int line;
            private bool dATA;
            private List<DataListInfo> data_list;

            public string Text
            {
                get
                {
                    return text;
                }

                set
                {
                    text = value;
                }
            }

            public int Line
            {
                get
                {
                    return line;
                }

                set
                {
                    line = value;
                }
            }

            public bool DATA
            {
                get
                {
                    return dATA;
                }

                set
                {
                    dATA = value;
                }
            }

            public List<DataListInfo> Data_list
            {
                get
                {
                    return data_list;
                }

                set
                {
                    data_list = value;
                }
            }

            public ERBInfo(string text, int line)
            {
                this.Text = text;
                this.Line = line;
                DATA = false;
            }
            public ERBInfo(List<DataListInfo> data_list)
            {
                this.Data_list = data_list;
                DATA = true;
            }
        }
    }
    public class LineInfo
    {
        private bool isList;
        private bool korean;
        private bool japanese;
        private string str;
        private int parent_line;

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

        public bool IsList
        {
            get
            {
                return isList;
            }
        }

        public bool Korean
        {
            get
            {
                return korean;
            }
        }

        public bool Japanese
        {
            get
            {
                return japanese;
            }
        }

        public int Parent_line
        {
            get
            {
                return parent_line;
            }

            set
            {
                parent_line = value;
            }
        }

        public LineInfo(string str)
        {
            Str = str;
            isList = false;
        }
        public LineInfo(string str, int parent_line)
        {
            //parent_line 부모 DATALIST의 줄수이며 이것으로 같은 FORM인지 구분
            Str = str;
            Parent_line = parent_line;
            isList = true;
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
                if ((a >= 0x3040 && a <= 0x309F) || (a >= 0x30A0 && a <= 0x30FF) || (a >= 0x31F0 && a <= 0x31FF))
                    Japanese = true;
                if ((a >= 0x1100 && a <= 0x11FF) || (a >= 0x3131 && a <= 0x318F) || (a >= 0xAC00 && a <= 0xD7FF))
                    Korean = true;
            }
        }
    }
}
