﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YeongHun.Common.Config;

namespace 에라번역
{
    [Serializable]
    public class Setting
    {
        public CheckState KoreanCB
        {
            get
            {
                return Config.GetValue<CheckState>(nameof(KoreanCB));
            }
            set
            {
                Config.SetValue(nameof(KoreanCB), value);
            }
        }

        public CheckState JapaneseCB
        {
            get
            {
                return Config.GetValue<CheckState>(nameof(JapaneseCB));
            }
            set
            {
                Config.SetValue(nameof(JapaneseCB), value);
            }
        }

        public CheckState etcCB
        {
            get
            {
                return Config.GetValue<CheckState>(nameof(etcCB));
            }
            set
            {
                Config.SetValue(nameof(etcCB), value);
            }
        }

        public LineSetting LineSetting
        {
            get
            {
                return Config.GetValue<LineSetting>(nameof(LineSetting));
            }
        }

        public Encoding ReadEncoding
        {
            get
            {
                return Config.GetValue<Encoding>(nameof(ReadEncoding));
            }
            set
            {
                Config.SetValue(nameof(ReadEncoding), value);
            }
        }
        
        public ConfigDic Config { get; }

        public Setting(ConfigDic config)
        {
            Config = config;

            config.AddParser(str =>
            {
                try
                {
                    return Encoding.GetEncoding(str);
                }
                catch
                {
                    int codePage;
                    if (int.TryParse(str, out codePage))
                        return Encoding.GetEncoding(codePage);
                    else
                        throw new InvalidCastException();
                }
            });

            config.AddWriter<Encoding>(encoding => encoding.WebName.ToUpper());

            config.AddParser(str => (CheckState)Enum.Parse(typeof(CheckState), str));
            config.AddParser(str =>
            {
                var format = Regex.Match(str, @"[^\s]+").Value;
                var strMatch = Regex.Match(Regex.Replace(str, @"[^\s]+\s(.*)", "$1"), @"([^\|]+)");
                List<string> strs = new List<string>();
                while (strMatch.Value != string.Empty)
                {
                    strs.Add(strMatch.Value);
                    strMatch = strMatch.NextMatch();
                }
                return new LineSetting(format, strs.ToArray());
            });

            if (!config.HasKey(nameof(KoreanCB)))
                config[nameof(KoreanCB)] = CheckState.Checked.ToString();
            if (!config.HasKey(nameof(JapaneseCB)))
                config[nameof(JapaneseCB)] = CheckState.Checked.ToString();
            if (!config.HasKey(nameof(etcCB)))
                config[nameof(etcCB)] = CheckState.Checked.ToString();
            if (!config.HasKey(nameof(LineSetting)))
                config[nameof(LineSetting)] = LineSetting.Default.ToString();
            if (!config.HasKey(nameof(ReadEncoding)))
                config[nameof(ReadEncoding)] = Encoding.UTF8.WebName.ToUpper();
        }
    }
}
