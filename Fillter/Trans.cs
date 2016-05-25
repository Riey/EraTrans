using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fillter
{
    public class Trans
    {
        [DllImport("EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        extern static int Init();
        [DllImport("EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        extern static void Terminate();
        [DllImport("EZTrans.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        extern static string Translate(string str);
        public static bool Initializing() => Init() == 0;
        public static void Destory() => Terminate();
        public static string GetString(string str) => Translate(str);
    }
}
