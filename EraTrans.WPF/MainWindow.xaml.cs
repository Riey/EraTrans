using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using YeongHun.Common.Config;
using System.IO;

namespace YeongHun.EraTrans.WPF
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private ConfigDic _configDic;
        private Config _config;
        

        public string EncodingText
        {
            get
            {
                return _config.ReadEncoding.WebName.ToUpper();
            }
        }
        public bool FileBackup
        {
            get => _config.FileBackup;
            set => _config.FileBackup = value;
        }


        public MainWindow()
        {
            _configDic = new ConfigDic();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Config.txt"))
                _configDic.Load(File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "Config.txt"));

            else
                File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "Config.txt").Close();

            _config = new Config(_configDic);
            _config.Load();
            _config.Save();
            _configDic.Save(File.OpenWrite(AppDomain.CurrentDomain.BaseDirectory + "Config.txt"));

            InitializeComponent();
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            Title = $"에라번역 version {version.Major}.{version.Minor}.{version.Revision}.{version.Build}";

            DataContext = this;
        }

        private void FileTranslateButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "ERB파일(*.ERB)|*.ERB|모든파일(*.*)|*.*";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() ?? false)
                StartTranslate(new[] { dialog.FileName });
        }

        private void FolderTranslateButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void StartTranslate(string[] filePaths)
        {
            if (FileBackup && !Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Backup\\"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Backup\\");
            try
            {
                var e = Encoding.GetEncoding(_encodingTextBox.Text);
                _config.ReadEncoding = e;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Fail GetEncoding Message : " + e.Message);
            }

            var parsers = new Dictionary<string, ErbParser>();
            foreach (var path in filePaths)
            {
                try
                {
                    var parser = new ErbParser(path, _config.ReadEncoding, _config.FileBackup);
                    lock (parsers)
                        parsers.Add(path, parser);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Raise error when creating parser");
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }

            var workingWindow = new WorkingWindow(parsers, _config);

            this.Visibility = Visibility.Collapsed;
            this.IsEnabled = false;
            WindowState = WindowState.Minimized;

            workingWindow.ShowDialog();

            this.Visibility = Visibility.Visible;
            this.IsEnabled = true;
            WindowState = WindowState.Maximized;
        }
    }
}
