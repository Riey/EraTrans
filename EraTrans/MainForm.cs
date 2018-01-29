using Riey.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Riey.EraTrans
{
    public partial class MainForm : Form
    {
        private BinaryFormatter formatter = new BinaryFormatter();
        private Setting setting;

        public MainForm(ConfigDic config)
        {
            setting = new Setting(config);
            setting.Load();
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
            if (EncodingText.Text != setting.ReadEncoding.WebName.ToUpper())
            {
                try
                {
                    setting.ReadEncoding = System.Text.Encoding.GetEncoding(EncodingText.Text);
                    setting.Config.Save(File.Open(Program.ConfigFilePath, FileMode.Create));
                }
                catch
                {

                }
            }

            var parsers = new Dictionary<string, ErbParser>();
            Parallel.ForEach(paths, path =>
            {
                ErbParser parser;
                try
                {
                    parser = new ErbParser(path, setting.ReadEncoding, setting.WriteEncoding);
                    if (parser.StringDictionary.Count == 0 && setting.IgnoreBlankERB)
                        return;
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
            var tf = new TranslateForm(parsers, setting, VersionText.Text);
            tf.ShowDialog();
        }

        private void Translate(string path)
        {
            Translate(new string[] { path });
        }

        private void Main_Form_Load(object sender, EventArgs e)
        {
            setting.Config.Save(File.Open(Program.ConfigFilePath, FileMode.Open));
            EncodingText.Text = setting.ReadEncoding.WebName.ToUpper();
        }

        private void Save()
        {
            setting.Save();
            setting.Config.Save(File.Open(Program.ConfigFilePath, FileMode.Open));
        }

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
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
}
