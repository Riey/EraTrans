using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Riey.Common.Config;

namespace Riey.EraTrans
{
    [Serializable]
    public sealed class Setting:LoadableConfig
    {
        [LoadableProperty("Working", Key = "저장방식", Tag = "Output")]
        public ErbParser.OutputType OutputType { get; set; }

        [LoadableProperty("Checked", Tag = "View")]
        public CheckState KoreanCB { get; set; }

        [LoadableProperty("Checked", Tag = "View")]
        public CheckState JapaneseCB { get; set; }

        [LoadableProperty("Checked", Tag = "View")]
        public CheckState EtcCB { get; set; }

        [LoadableProperty("LINENUM+str+LINETEXT 번째줄===>", Tag = "View")]
        public LineSetting LineSetting { get; set; }

        [LoadableProperty("UTF-8", Tag = "Encoding")]
        public Encoding ReadEncoding { get; set; }

        [LoadableProperty("UTF-8", Tag = "Encoding")]
        public Encoding WriteEncoding { get; set; }

        [LoadableProperty("True", Tag = "View")]
        public bool IgnoreBlankERB { get; set; }

        [LoadableProperty("1252 * 800", Tag = "Previous State")]
        public Size PreviousFormSize { get; set; }

        [LoadableProperty("(100, 100)", Tag = "Previous State")]
        public Point PreviousFormPosition { get; set; }

        protected override void AddParsers(ConfigDic configDic)
        {
            configDic.AddParser(str => (CheckState)Enum.Parse(typeof(CheckState), str));
            configDic.AddParser(str =>
            {
                var format = Regex.Match(str, @"[^\s]+").Value;
                var strMatch = Regex.Match(Regex.Replace(str, @"[^\s]+\s(.*)", "$1"), @"([^\|]+)");
                var strs = new List<string>();

                while (strMatch.Value != string.Empty)
                {
                    strs.Add(strMatch.Value);
                    strMatch = strMatch.NextMatch();
                }

                return new LineSetting(format, strs.ToArray());
            });
            configDic.AddParser(str =>
            {
                try
                {
                    return Encoding.GetEncoding(str);
                }
                catch
                {
                    return int.TryParse(str, out int codePage)
                        ? Encoding.GetEncoding(codePage)
                        : throw new InvalidCastException();
                }
            });

            configDic.AddParser(str =>
            {
                Match match = Regex.Match(str, @"\((?<X>\d+), (?<Y>\d+)\)");
                if (match.Success)
                    return new Point(int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value));
                throw new InvalidCastException();
            });

            configDic.AddParser(str =>
            {
                Match match = Regex.Match(str, @"(?<X>\d+) \* (?<Y>\d+)");
                if (match.Success)
                    return new Size(int.Parse(match.Groups["X"].Value), int.Parse(match.Groups["Y"].Value));
                throw new InvalidCastException();
            });
        }

        protected override void AddWriters(ConfigDic configDic)
        {
            configDic.AddWriter<Encoding>(encoding => encoding.WebName.ToUpper());
            configDic.AddWriter<Point>(p => $"({p.X}, {p.Y})");
            configDic.AddWriter<Size>(s => $"{s.Width} * {s.Height}");
        }

        public ConfigDic Config { get; }

        public Setting(ConfigDic config)
        {
            Config = config;
        }

        public void Load() => Load(Config);
        public void Save() => Save(Config);
    }
}
