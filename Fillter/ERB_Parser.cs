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
            this.erb_path = erb_path;
            if (!File.Exists(erb_path))
            {
                throw new FileNotFoundException();
            }
            FileInfo info = new FileInfo(erb_path);
            if (!File.Exists(Application.StartupPath + "\\Backup\\" + info.Name))
            {
                info.CopyTo(Application.StartupPath + "\\Backup\\" + info.Name);
            }
            erb_dic = info.Directory;
            //해쉬값을 얻음
            using (fs = info.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader reader = encoding != null ? new StreamReader(fs, encoding) : new StreamReader(fs, true))
                {
                    erb_encoding = reader.CurrentEncoding;
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
                                        string temp = erbinfo.text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                                        dic.Add(erbinfo.line, new LineInfo(erbinfo.text.Replace(temp, "")));
                                        _dic.Add(erbinfo.line, temp);
                                    }
                                    else
                                    {
                                        foreach (var listinfo in erbinfo.data_list)
                                        {
                                            for (int a = 1; a <= listinfo.forms.Length; a++)
                                            {
                                                if (listinfo.forms[a - 1].Contains("DATAFORM"))
                                                {
                                                    string[] line = listinfo.forms[a - 1].Split(new string[] { "DATAFORM" }, StringSplitOptions.None);
                                                    string _str = line[0] + "DATAFORM";
                                                    string str = line[1] ?? "";
                                                    dic.Add(listinfo.line + a, new LineInfo(str, listinfo.line));
                                                    _dic.Add(listinfo.line + a, _str);
                                                }
                                                else if (listinfo.forms[a - 1].Contains("DATA"))
                                                {
                                                    string[] line = listinfo.forms[a - 1].Split(new string[] { "DATA" }, StringSplitOptions.None);
                                                    string _str = line[0] + "DATA";
                                                    string str = line[1] ?? "";
                                                    dic.Add(listinfo.line + a, new LineInfo(str, listinfo.line));
                                                    _dic.Add(listinfo.line + a, _str);
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
                        ERB.Add(temp);//원본 저장
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
                                    ERB.Add(temp);
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
            fs = new FileStream(erb_path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter writer = new StreamWriter(fs, erb_encoding);
            foreach (var a in dic)
            {
                string temp = _dic[a.Key] + a.Value.str;
                ERB[a.Key] = temp;
            }
            foreach (var a in ERB)
            {
                writer.WriteLine(a);
            }
            writer.Flush();
            writer.Close();
            fs.Close();
            fs.Dispose();
        }
        public List<string> ERB = new List<string>();
        public Dictionary<int, LineInfo> dic = new Dictionary<int, LineInfo>();
        public Dictionary<int, string> _dic = new Dictionary<int, string>();
        public DirectoryInfo erb_dic;
        public string erb_path;
        public Encoding erb_encoding;
        FileStream fs;
    }
    class DataListInfo
    {
        public string[] forms { get; }
        public int line { get; }
        public DataListInfo(string[] forms, int line)
        {
            this.forms = forms;
            this.line = line;
            //line:부모줄번호
        }
    }
    class ERBInfo
    {
        public string text;
        public int line;
        public bool DATA;
        public List<DataListInfo> data_list;
        public ERBInfo(string text, int line)
        {
            this.text = text;
            this.line = line;
            DATA = false;
        }
        public ERBInfo(List<DataListInfo> data_list)
        {
            this.data_list = data_list;
            DATA = true;
        }
    }
    public class LineInfo
    {
        public bool IsList;
        public bool Korean;
        public bool Japanese;
        private string p_str;
        public string str
        {
            get
            {
                return p_str;
            }
            set
            {
                p_str = value;
                GetLang.Get(value, out Korean, out Japanese);
            }
        }
        public int parent_line;
        public LineInfo(string str)
        {
            this.str = str;
            IsList = false;
        }
        public LineInfo(string str, int parent_line)
        {
            //parent_line 부모 DATALIST의 줄수이며 이것으로 같은 FORM인지 구분
            this.str = str;
            this.parent_line = parent_line;
            IsList = true;
        }
    }
    class GetLang
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
