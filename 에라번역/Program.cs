using System;
using System.Windows.Forms;
using System.IO;
using EZTrans;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                {
                    if (!Directory.Exists(Application.StartupPath + "\\Res"))
                        Directory.CreateDirectory(Application.StartupPath + "\\Res");
                    if (!Directory.Exists(Application.StartupPath + "\\Backup"))
                        Directory.CreateDirectory(Application.StartupPath + "\\Backup");
                    string ezPath = "";
                    if (!File.Exists(Application.StartupPath + "\\Res\\ezTransXP_Path.txt"))
                    {
                        ResetEZTransPath();
                    }
                    while (true)
                    {
                        using (StreamReader reader = File.OpenText(Application.StartupPath + "\\Res\\ezTransXP_Path.txt"))
                        {
                            ezPath = reader.ReadLine();
                        }
                        if (Directory.Exists(ezPath))
                            break;
                        else
                        {
                            File.Delete(Application.StartupPath + "\\Res\\ezTransXP_Path.txt");
                            ResetEZTransPath();
                        }
                    }
                    int result = TranslateXP.Init(ezPath, false);
                    if (result != 0)
                    {
                        MessageBox.Show("EZTransXP 로드에 실패하였습니다.\nCode: " + result);
                        return;
                    }
                }
                Application.Run(new Main_Form());
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
        private static void ResetEZTransPath()
        {
            using (StreamWriter writer = File.CreateText(Application.StartupPath + "\\Res\\ezTransXP_Path.txt"))
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
                dialog.ShowNewFolderButton = true;
                dialog.Description = "ezTrans XP가 설치된 경로를 선택해 주세요";
                dialog.ShowDialog();
                writer.WriteLine(dialog.SelectedPath);
                writer.Flush();
            }
        }
    }
}
