using System;
using System.Windows.Forms;
using System.IO;
using YeongHun.EZTrans;
using YeongHun.Common.Config;

namespace 에라번역
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists(Application.StartupPath + "\\Res"))
                Directory.CreateDirectory(Application.StartupPath + "\\Res");
            if (!Directory.Exists(Application.StartupPath + "\\Backup"))
                Directory.CreateDirectory(Application.StartupPath + "\\Backup");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                string ezPath = "";
                ConfigDic config = new ConfigDic();
                config.Load(Application.StartupPath + "\\Config.txt");
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
                config.Save(Application.StartupPath + "\\Res\\Config.txt");
                TranslateXP.Terminate();
            }
            catch (Exception e)
            {
                FileStream fs = new FileStream(Application.StartupPath + "\\log.dat", FileMode.Create);
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(fs, e);
                fs.Flush();
                fs.Dispose();
            }
        }
    }
}
