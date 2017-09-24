using System;
using System.Windows.Forms;
using System.IO;
using YeongHun.EZTrans;
using YeongHun.Common.Config;

namespace YeongHun.EraTrans
{
    static class Program
    {
        internal static readonly string RootPath = Application.StartupPath + Path.DirectorySeparatorChar;
        internal static readonly string LogFilePath = RootPath + "log.dat";
        internal static readonly string ConfigFilePath = RootPath + "Config.txt";

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if !DEBUG
            try
            {
#endif
            var config = new ConfigDic();
            config.Load(new FileStream(ConfigFilePath, FileMode.OpenOrCreate));
            if (!config.TryGetValue("ezTransXP_Path", out string ezPath))
            {
                var dialog = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = true,
                    Description = "ezTrans XP가 설치된 경로를 선택해 주세요"
                };
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
            config.Save(new FileStream(ConfigFilePath, FileMode.Create));
            TranslateXP.Terminate();
#if !DEBUG
        }
            catch (Exception e)
            {  
                var fs = new FileStream(LogFilePath, FileMode.Create);
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(fs, e);
                fs.Flush();
                fs.Dispose();
                MessageBox.Show(e.Message + "\r\n\r\n\r\n" + e.StackTrace, "에러발생!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
        } 
#endif
        }
    }
}
