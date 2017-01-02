﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YeongHun.Common.Config;

namespace YeongHun.EraTrans.WPF
{
    public enum Status
    {
        Enable,
        Disable,
    }
    public class Config : LoadableConfig
    {
        public static readonly string CacheFileName = "Cache.dat";
        public static readonly string UserDictionaryName = "UserDictionary.xml";



        [LoadableProperty("True")]
        public bool FileBackup { get; set; }

        [LoadableProperty("True", Tag = "View")]
        public bool ShowKorean { get; set; }

        [LoadableProperty("True", Tag = "View")]
        public bool ShowJanapanese { get; set; }

        [LoadableProperty("LINENUM+str 번째줄===>", Tag = "View")]
        public LineSetting LineSetting { get; set; }

        [LoadableProperty("UTF-8", Tag = "Encoding")]
        public Encoding ReadEncoding { get; set; }

        [LoadableField("Enable", Key = "Status", Tag = "ezTransXP")]
        private Status _ezTransStatus;

        public bool EzTransEnable => _ezTransStatus == Status.Enable;

        [LoadableField("Disable", Key = "Caching", Tag = "ezTransXP")]
        private Status _ezTransCache;

        public bool EZTransCaching => _ezTransCache == Status.Enable;

        [LoadableField("Disable", Key = "UseUserDictionary", Tag = "ezTransXP")]
        private Status _useUserDictionary;

        public bool UseUserDictionary => _useUserDictionary == Status.Enable;

        [LoadableProperty("", Key = "FolderPath", Tag = "ezTransXP")]
        public string EzTransXP_Path { get; set; }

        protected override void AddParsers(ConfigDic configDic)
        {
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
                    if (int.TryParse(str, out int codePage))
                        return Encoding.GetEncoding(codePage);
                    else
                        throw new InvalidCastException();
                }
            });
        }

        protected override void AddWriters(ConfigDic configDic)
        {
            configDic.AddWriter<Encoding>(encoding => encoding.WebName.ToUpper());
        }

        public ConfigDic ConfigDic { get; }

        public Config(ConfigDic config)
        {
            ConfigDic = config;
        }

        public void Load() => Load(ConfigDic);
        public void Save() => Save(ConfigDic);
    }
}