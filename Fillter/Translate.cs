using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fillter
{
    unsafe class ezTransXP
    {
        public enum OHBTRANSID
        {
            /**
             * Not initialized translator.
             */
            OHBID_INVALID = -1,

            /**
             * Japanese-Korean ezTrans XP ChangshinSoft ( http://www.cssoft.co.kr )
             */
            OHBID_EZTR_JK = 0,

            /**
             * Korean-Japanese ezTrans XP ChangshinSoft. ( http://www.cssoft.co.kr )
             */
            OHBID_EZTR_KJ = 1,

            /**
             * Japanese-Korean Babel TOP 2002 Unisoft. ( http://www.unisoft.co.kr )
             */
            OHBID_BABEL_JK = 2,

            /**
             * Korean-Japanese Babel TOP 2002 Unisoft. ( http://www.unisoft.co.kr )
             */
            OHBID_BABEL_KJ = 3,
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OHBSETTING
        {
            /**
             * Translator ID. If the specified translator is not initialized, it will
             * have OHBID_INVALID.
             */
            OHBTRANSID iTransID;

            /**
             * Translator name in unicode.
             */
            [MarshalAs(UnmanagedType.LPWStr)]
            string szTransName;

            /**
             * Private data pointer of the user. If the user wants to pass a translator
             * dependent arguments, use this pointer.
             */
            int* pData;

            /**
             * Reserved for the future use.
             */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            char[] reserved;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct OHBTRANS
        {
            /**
             * Translated text in unicode.
             */
            [MarshalAs(UnmanagedType.LPWStr)]
            public string lpStr;

            /**
             * OhbTrans sets this pointer with the pSetting function argument.
             */
            OHBSETTING pSetting;

            /**
             * Private data pointer of Oh! Babel Helper Library. The user of Oh! Babel
             * Helper Library should not touch this.
             */
            int* pData;

            /**
             * Reserved for the future use.
             */
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            char[] reserved;
        }
        [DllImport("ohbbhlp.dll", CharSet = CharSet.Unicode)]
        extern public static int OhbInit(OHBTRANSID iTransID);
        [DllImport("ohbbhlp.dll", CharSet = CharSet.Unicode)]
        extern public static void OhbDstroy(OHBTRANSID iTransID);
        [DllImport("ohbbhlp.dll", CharSet = CharSet.Unicode)]
        extern public static IntPtr OhbGetSettings(int* piSettings);
        [DllImport("ohbbhlp.dll", CharSet = CharSet.Unicode)]
        extern public static IntPtr OhbTrans(string IpStr, IntPtr setting);
        IntPtr setting;
        OHBTRANS trans;
        public ezTransXP()
        {
            int temp;
            OhbInit(OHBTRANSID.OHBID_EZTR_JK);
            setting = OhbGetSettings(&temp);
        }
        public string GetString(string source)
        {
            var trans_ptr = OhbTrans(source, setting);
            trans = (OHBTRANS)Marshal.PtrToStructure(trans_ptr, typeof(OHBTRANS));
            return trans.lpStr;
        }
    }
    public class Translate
    {
        ezTransXP ez = new ezTransXP();
        public string[] 일괄번역(string[] sources)
        {
            var result = sources.Select(source => ez.GetString(source)).ToArray();
            return result;
        }
        public string 번역(string source)
        {
            return ez.GetString(source);
        }
    }
}
