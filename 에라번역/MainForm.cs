using Fillter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class MainForm : Form
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        private Setting setting;
        private Tuple<string,string> loadedFile = null;

        public MainForm()
        {
            InitializeComponent();
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            VersionText.Text = "Version:  ";
            VersionText.Text += v.Major + "." + v.Minor + "." + v.Build;
        }

        public MainForm(string[] args):this()
        {
            if (args.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(args));
            loadedFile = Tuple.Create(args[0], args[1]);
        }

        private void 파일열기버튼_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ERB파일(*.ERB)|*.ERB";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileNames.Length == 0)
                return;
            Translate(openFileDialog1.FileNames);
        }
        private void Translate(string[] paths)
        {
            Dictionary<string, ERB_Parser> parsers = new Dictionary<string, ERB_Parser>();
            Parallel.ForEach(paths, path =>
            {
                ERB_Parser parser;
                try
                {
                    int code;
                    if (int.TryParse(EncodingText.Text, out code))
                    {
                        parser = new ERB_Parser(path, System.Text.Encoding.GetEncoding(code));
                    }
                    else
                    {
                        parser = new ERB_Parser(path);
                    }
                    lock (parsers)
                    {
                        parsers.Add(path, parser); 
                    }
                }
                catch (FileNotFoundException fne)
                {
                    MessageBox.Show(fne.Message);
                    return;
                }
                catch (IOException ioe)
                {
                    MessageBox.Show(ioe.Message);
                    return;
                }
                catch (Exception)
                {
                    return;
                }
            });
            TranslateForm tf = new TranslateForm(parsers, setting, VersionText.Text);
            tf.ShowDialog();
        }
        private void Translate(string path)
        {
            Translate(new string[] { path });
        }
        private void Main_Form_Load(object sender, EventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(Application.StartupPath + "\\Res\\Setting.dat", FileMode.Open))
                {
                    setting = formatter.Deserialize(fs) as Setting;
                }
            }
            catch (Exception)
            {
                setting = new Setting(CheckState.Indeterminate, CheckState.Indeterminate, CheckState.Unchecked, LineSetting.Default,AuthorSetting.Default);
            }
            Save();
            if (loadedFile != null)
            {
                EncodingText.Text = loadedFile.Item2;
                Translate(loadedFile.Item1);
            }
        }
        private void Save()
        {
            using (FileStream fs = new FileStream(Application.StartupPath + "\\Res\\Setting.dat", FileMode.Create))
            {
                formatter.Serialize(fs, setting);
            }
        }

        private void Main_Form_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            FileInfo[] infos = files.Select(file => new FileInfo(file)).ToArray();
            foreach (var info in infos)
            {
                if (info.Extension == ".ERB" || info.Extension == ".erb")
                    Translate(info.FullName);
            }
        }

        private void Main_Form_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }
        private static FileInfo[] GetFiles(DirectoryInfo info)
        {
            List<FileInfo> files = new List<FileInfo>(info.GetFiles());
            DirectoryInfo[] dirs = info.GetDirectories();
            if (dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    files.AddRange(GetFiles(dir));
                }
            }
            return files.ToArray();
        }
        /*
        private void 번역적용버튼_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ERB 파일|*.erb|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            string erb_path = openFileDialog1.FileName;
            openFileDialog1.Filter= "번역 파일|*.xml|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            string xml_path = openFileDialog1.FileName;
            if (erb_path == "" || xml_path == "")
                return;
            Apply_Trans(erb_path, xml_path);
        }
        private void 번역합치기버튼_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "번역 파일|*.xml|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            string temp1, temp2;
            temp1 = openFileDialog1.FileName;
            openFileDialog1.ShowDialog();
            temp2 = openFileDialog1.FileName;
            string[] xml_paths = new string[] { temp1, temp2 };
            openFileDialog1.Filter = "ERB 파일|*.erb|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            ERB_Parser parser = new ERB_Parser(openFileDialog1.FileName);
            Queue<ChangeLog>[] logs = GetLogs(xml_paths, parser);
            Dictionary<LogInfo, ChangeLog> check_dic = new Dictionary<LogInfo, ChangeLog>();
            List<ChangeLog> 일괄번역로그 = new List<ChangeLog>();
            List<CrashLog> crashLog = new List<CrashLog>();
            foreach(var log in logs)
            {
                for(int i = 0; i < log.Count; i++)
                {
                    ChangeLog cl = log.Dequeue();
                    try {
                        if (cl.했던일 == ChangeLog.행동.일괄번역)
                        {
                            일괄번역로그.Add(cl);
                        }
                        else
                            check_dic.Add(new LogInfo(cl.했던일, cl.줄번호), cl);
                    }
                    catch (ArgumentException)
                    {
                        ChangeLog pre_log = check_dic[new LogInfo(cl.했던일, cl.줄번호)];
                        if (!ChangeLog.같은가(cl, pre_log))
                            crashLog.Add(new CrashLog(pre_log, cl));
                        else
                            continue;
                    }
                    
                }
            }
            foreach(var crash in crashLog)
            {
                check_dic.Remove(new LogInfo(crash.log1.했던일, crash.log1.줄번호));
            }
            List<ChangeLog> 최종로그 = new List<ChangeLog>();
            Select_Line sl = new Select_Line(crashLog.ToArray(), 일괄번역로그,최종로그);
            sl.ShowDialog();
            foreach(var log in check_dic)
            {
                ChangeLog.실행(log.Value, parser);
            }
            foreach(var log in 최종로그)
            {
                ChangeLog.실행(log, parser);
            }
        }

        private void 되돌리기버튼_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "ERB 파일|*.erb|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            string erb_path = openFileDialog1.FileName;
            openFileDialog1.Filter = "번역 파일|*.xml|모든 파일|*.*";
            openFileDialog1.ShowDialog();
            string xml_path = openFileDialog1.FileName;
            if (erb_path == "" || xml_path == "")
                return;
            Apply_Trans(erb_path, xml_path, true);
        }
        */
        private void EncodingText_Enter(object sender, EventArgs e)
        {
            EncodingText.Text = "";
        }

        private void 폴더열기버튼_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "ERB파일이 들어있는 폴더를 선택해주세요";
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath == "")
                return;
            var files = GetFiles(new DirectoryInfo(folderBrowserDialog1.SelectedPath)).Where(f => f.Extension.ToUpper() == ".ERB").Select(f=>f.FullName).ToArray();
            Translate(files);
        }
    }
    public class CrashLog
    {
        public int line
        {
            get
            {
                return log1.LineNum;
            }
        }
        public string 한일
        {
            get
            {
                return log1.했던일.ToString();
            }
        }
        public string 원본
        {
            get
            {
                return log1.Str1;
            }
        }
        public ChangeLog log1;
        public ChangeLog log2;
        public ChangeLog new_log;
        public CrashLog(ChangeLog log1,ChangeLog log2)
        {
            this.log1 = log1;
            this.log2 = log2;
        }
    }
    class LogInfo
    {
        public ChangeLog.행동 한일;
        public int 줄번호;
        public LogInfo(ChangeLog.행동 한일, int 줄번호)
        {
            this.한일 = 한일;
            this.줄번호 = 줄번호;
        }
    }
}
