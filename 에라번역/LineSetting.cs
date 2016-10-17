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
        public string Format { get; }
        public string[] Strs { get; }

        public LineSetting(string format, string[] strs)
        {
            Format = format;
            Strs = strs;
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
            StringBuilder result = new StringBuilder();
            int count = 0;
            foreach (string temp in Format.Split('+'))
            {
                switch (temp)
                {
                    case ("LINENUM"):
                        {
                            result.Append(linenum);
                            break;
                        }
                    case ("LINETEXT"):
                        {
                            result.Append(linetext);
                            break;
                        }
                    case ("str"):
                        {
                            if (count <= Strs.Length)
                            {
                                result.Append(Strs[count++]);
                            }
                            break;
                        }
                }
            }
            return result.ToString();
        }

        public override string ToString()
        {
            //string strs = Strs.Length == 0 ? "" : Strs.Length == 1 ? Strs[0] : string.Join("|", Strs);
            return $"{Format} {string.Join("|", Strs)}";
        }
    }
}
