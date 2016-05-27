using System;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Text;

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
                Fillter.Trans.Initializing();
                Application.Run(new Main_Form());
                Fillter.Trans.Destory();
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
