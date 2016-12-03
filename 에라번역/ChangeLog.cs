
using System;
using System.Collections.Generic;

namespace YeongHun.EraTrans
{
    [Serializable]
    public class ChangeLog
    {
        public enum Action
        {
            TRANSLATE, BATCH_TRANSLATE
        }
        public string ErbName { get; }
        public int LineNum { get; }
        public string Str1 { get; }
        public string Str2 { get; }
        public Action PreAction { get; }
        /// <summary>
        /// Normal Translate
        /// </summary>
        /// <param name="erbName"></param>
        /// <param name="lineNum"></param>
        /// <param name="original"></param>
        /// <param name="translation"></param>
        public ChangeLog(string erbName, int lineNum, string original, string translation) : this(erbName, lineNum, original, translation, Action.TRANSLATE)
        {
        }
        /// <summary>
        /// Batch Translate
        /// </summary>
        /// <param name="erbName"></param>
        /// <param name="original"></param>
        /// <param name="batchTranslation"></param>
        public ChangeLog(string erbName, string original, string batchTranslation) : this(erbName, -1, original, batchTranslation, Action.BATCH_TRANSLATE)
        {
        }
        private ChangeLog(string erbName, int lineNum, string str1, string str2, Action preAction)
        {
            ErbName = erbName;
            LineNum = lineNum;
            Str1 = str1;
            Str2 = str2;
            PreAction = preAction;
        }
        public static bool Equals(ChangeLog log1, ChangeLog log2)
        {
            if (log1.ErbName != log2.ErbName)
                return false;
            if (log1.Str1 == log2.Str1 && log1.Str2 == log2.Str2 && log1.LineNum == log2.LineNum && log1.PreAction == log2.PreAction)
            {
                return true;
            }
            return false;
        }
        public static ChangeLog Redo(ChangeLog log, Dictionary<string, ErbParser> parsers)
        {
            switch (log.PreAction)
            {
                case (Action.TRANSLATE):
                    {
                        return Back(new ChangeLog(log.ErbName, log.LineNum, log.Str2, log.Str1), parsers);
                    }
                case (Action.BATCH_TRANSLATE):
                    {
                        return Back(new ChangeLog(log.ErbName, log.Str2, log.Str1), parsers);
                    }
                default:
                    {
                        throw new ArgumentException("행동을 알수없습니다.");
                    }
            }
        }
        public static ChangeLog Back(ChangeLog log, Dictionary<string, ErbParser> parsers)
        {
            switch (log.PreAction)
            {
                case (Action.TRANSLATE):
                    {
                        parsers[log.ErbName].StringDictionary[log.LineNum].Str = log.Str1;
                        return new ChangeLog(log.ErbName, log.LineNum, log.Str2, log.Str1);
                    }
                case (Action.BATCH_TRANSLATE):
                    {
                        List<Tuple<int, LineInfo, string>> diclog = new List<Tuple<int, LineInfo,string>>();
                        foreach (var temp in parsers[log.ErbName].StringDictionary)
                        {
                            diclog.Add(new Tuple<int,LineInfo, string>(temp.Key,temp.Value, temp.Value.Str.Replace(log.Str2, log.Str1)));
                        }
                        foreach (var temp in diclog)
                        {
                            parsers[log.ErbName].StringDictionary[temp.Item1] = new LineInfo(temp.Item3, temp.Item2.IsForm, temp.Item2.IsFormS, temp.Item2.OriginalString);
                        }
                        return new ChangeLog(log.ErbName, log.Str2, log.Str1);
                    }
                default:
                    {
                        throw new ArgumentException("행동을 알수없습니다.");
                    }
            }
        }
    }
}
