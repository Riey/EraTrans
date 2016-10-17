using System;
using System.Windows.Forms;
using System.IO;
using YeongHun.EZTrans;
using YeongHun.Common.Config;
using System.Diagnostics;

namespace 에라번역
{
    static class Program
    {
        internal static readonly string RootPath = Application.StartupPath + Path.DirectorySeparatorChar;
        internal static readonly string LogFilePath = RootPath + "log.dat";
        internal static readonly string ConfigFilePath = RootPath + "Config.txt";
        internal static readonly string ResourceFolderPath = RootPath + "Res" + Path.DirectorySeparatorChar;

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists(ResourceFolderPath))
                Directory.CreateDirectory(ResourceFolderPath);
            if (!Directory.Exists(RootPath + "Backup"))
                Directory.CreateDirectory(RootPath + "Backup");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                string ezPath = "";
                ConfigDic config = new ConfigDic();
                config.Load(ConfigFilePath);
                if (!config.TryGetValue("ezTransXP_Path", out ezPath))
                {
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.ShowNewFolderButton = true;
                    dialog.Description = "ezTrans XP가 설치된 경로를 선택해 주세요";
                    dialog.ShowDialog();
                    ezPath = dialog.SelectedPath;
                    config.SetValue("ezTransXP_Path", ezPath);
                    dialog.Dispose();
                }
                int result = TranslateXP.Initialize(ezPath);
                if (result != 0)
                {
                    MessageBox.Show("EZTransXP 로드에 실패하였습니다.\nCode: " + result);
                    return;
                }
                Application.Run(new MainForm(config));
                config.Save(ConfigFilePath);
                TranslateXP.Terminate();
            }
            catch (Exception e)
            {
                FileStream fs = new FileStream(LogFilePath, FileMode.Create);
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(fs, e);
                fs.Flush();
                fs.Dispose();
                Trace.Assert(false);
            }
        }
    }
}
