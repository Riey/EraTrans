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
using YeongHun.Common.Config;

namespace 에라번역
{
    public partial class MainForm : Form
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        private Setting setting;

        public MainForm(ConfigDic config)
        {
            setting = new Setting(config);
            InitializeComponent();
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            VersionText.Text = "Version:  ";
            VersionText.Text += v.Major + "." + v.Minor + "." + v.Build;
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
            if (EncodingText.Text != setting.Config[nameof(setting.ReadEncoding)])
            {
                try
                {
                    setting.ReadEncoding = System.Text.Encoding.GetEncoding(EncodingText.Text);
                    setting.Config.Save(Program.ConfigFilePath);
                }
                catch
                {

                }
            }

            Dictionary<string, ERB_Parser> parsers = new Dictionary<string, ERB_Parser>();
            Parallel.ForEach(paths, path =>
            {
                ERB_Parser parser;
                try
                {
                    parser = new ERB_Parser(path, setting.ReadEncoding);
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
            Save();
            EncodingText.Text = setting.Config[nameof(setting.ReadEncoding)];
        }

        private void Save()
        {
            setting.Config.Save(Program.ConfigFilePath);
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            var infos = files.Where(file => File.Exists(file))
                            .Concat(files
                                .Where(dir => Directory.Exists(dir))
                                .SelectMany(dir => Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Where(file => Path.GetExtension(file).ToUpper() == ".ERB")));
            Translate(infos.ToArray());
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Save();
        }

        private void 폴더열기버튼_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "ERB파일이 들어있는 폴더를 선택해주세요";
            folderBrowserDialog1.ShowDialog();
            if (folderBrowserDialog1.SelectedPath == "")
                return;
            var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*", SearchOption.AllDirectories).Where(file => Path.GetExtension(file).ToUpper() == ".ERB").ToArray();
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
