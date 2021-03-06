﻿using System;
using System.Text;

namespace Riey.EraTrans
{
    [Serializable]
    public class LineSetting
    {
        private string _format;
        private string[] _strs;

        public LineSetting(string format, string[] strs)
        {
            _format = format;
            _strs = strs;
        }

        public static LineSetting Default
        {
            get
            {
                return new LineSetting("LINENUM+str+LINETEXT", new string[] { "번째줄===>" });
            }
        }

        public string GetLine(long linenum, string linetext)
        {
            var result = new StringBuilder();
            int count = 0;
            foreach (string temp in _format.Split('+'))
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
                            if (count <= _strs.Length)
                            {
                                result.Append(_strs[count++]);
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
            return $"{_format} {string.Join("|", _strs)}";
        }
    }
}
