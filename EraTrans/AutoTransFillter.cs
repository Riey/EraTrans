using System.Collections.Generic;
using System.Text;

namespace YeongHun.EraTrans
{
    static class AutoTransFillter
    {
        private class SeperatedString
        {
            public string[] SeperatedStrings { get; }

            private string[] seperators;

            public SeperatedString(string rawStr, params string[] seperators)
            {
                var stringTemp = new List<string>();
                var seperatorTemp = new List<string>();
                var sb = new StringBuilder();
                for (int i = 0; i < rawStr.Length; i++)
                {
                    var index = IndexOf(rawStr, i, seperators);
                    if (index == -1)
                    {
                        sb.Append(rawStr[i]);
                    }
                    else
                    {
                        stringTemp.Add(sb.ToString());
                        sb.Clear();
                        seperatorTemp.Add(seperators[index]);
                        i -= 1;
                        i += seperators[index].Length;
                    }
                }
                stringTemp.Add(sb.ToString());
                SeperatedStrings = stringTemp.ToArray();
                this.seperators = seperatorTemp.ToArray();
            }

            private int IndexOf(string str, int startIndex, string[] array)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (str.Length <= startIndex + array[i].Length) continue;
                    if (str.Substring(startIndex, array[i].Length) == array[i])
                        return i;
                }
                return -1;
            }

            public override string ToString()
            {
                var sb = new StringBuilder(SeperatedStrings[0]);
                for (int i = 1; i < SeperatedStrings.Length; i++)
                {
                    sb.Append(seperators[i - 1]);
                    sb.Append(SeperatedStrings[i]);
                }
                return sb.ToString();
            }
        }

        public static string TranslateWithFillter(LineInfo info)
        {
            if (info.IsForm)
            {
                return TranslateWithEscape(info.Str);
            }
            else if (info.IsFormS)
            {
                return null;
            }
            else
            {
                return YeongHun.EZTrans.TranslateXP.Translate(info.Str);
            }
        }

        private static string TranslateWithEscape(string jpStr)
        {
            var perSep = new SeperatedString(jpStr, "%");
            for (int i = 0; i < perSep.SeperatedStrings.Length; i++)
            {
                if (i % 2 == 1) continue;
                var braceSep = new SeperatedString(perSep.SeperatedStrings[i], "{", "}");
                for (int j = 0; j < braceSep.SeperatedStrings.Length; j++)
                {
                    if (j % 2 == 1) continue;
                    var atSep = new SeperatedString(braceSep.SeperatedStrings[j], "\\@");
                    for (int k = 0; k < atSep.SeperatedStrings.Length; k++)
                    {
                        if (k % 2 == 1) continue;
                        YeongHun.EZTrans.TranslateXP.Translate(ref atSep.SeperatedStrings[k]);
                    }
                    braceSep.SeperatedStrings[j] = atSep.ToString();
                }
                perSep.SeperatedStrings[i] = braceSep.ToString();
            }
            return perSep.ToString();
        }

    }
}
