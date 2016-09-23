using Fillter2.Parsing;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// For EmueraFramework
        /// </summary>
        /// <param name="args"></param>
        public MainForm(string[] args):this()
        {
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
            Dictionary<string, ErbParser> parsers = new Dictionary<string, ErbParser>();
            Parallel.ForEach(paths, path =>
            {
                ErbParser parser;
                try
                {
                    int code;
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        if (int.TryParse(EncodingText.Text, out code))
                        {
                            parser = new ErbParser(new StreamReader(stream, System.Text.Encoding.GetEncoding(code)));
                        }
                        else
                        {
                            parser = new ErbParser(new StreamReader(stream, true));
                        }
                    }

                    if(!File.Exists(Application.StartupPath + "\\Backup\\" + Path.GetFileName(path)))
                        File.Copy(path, Application.StartupPath + "\\Backup\\" + Path.GetFileName(path));

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
                setting = new Setting(
                    CheckState.Indeterminate, CheckState.Indeterminate, CheckState.Unchecked,
                    LineSetting.Default, AuthorSetting.Default, System.Text.Encoding.UTF8);
            }
            Save();
            EncodingText.Text = setting.ErbEncoding?.CodePage.ToString() ?? "65001";
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

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            FileInfo[] infos = files.Select(file => new FileInfo(file)).ToArray();
            foreach (var info in infos)
            {
                if (info.Extension == ".ERB" || info.Extension == ".erb")
                    Translate(info.FullName);
            }
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
}
