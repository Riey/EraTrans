using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 에라번역
{
    [Serializable]
    public class LineSetting
    {
        private string setting;
        private string[] str;

        public string Setting
        {
            get
            {
                return setting;
            }

            set
            {
                setting = value;
            }
        }
        public string[] Str
        {
            get
            {
                return str;
            }

            set
            {
                str = value;
            }
        }

        public LineSetting(string setting, string[] str)
        {
            Setting = setting;
            this.str = str;
        }

        public static LineSetting Default
        {
            get
            {
                return new LineSetting("LINENUM+str+LINETEXT", new string[] { "번째줄===>" });
            }
        }

        public string GetLine(int linenum, string linetext)
        {
            string result = "";
            int count = 0;
            foreach (string temp in Setting.Split('+'))
            {
                switch (temp)
                {
                    case ("LINENUM"):
                        {
                            result += linenum;
                            break;
                        }
                    case ("LINETEXT"):
                        {
                            result += linetext;
                            break;
                        }
                    case ("str"):
                        {
                            if (count <= Str.Length)
                            {
                                result += Str[count++];
                            }
                            break;
                        }
                }
            }
            return result;
        }
    }
}
