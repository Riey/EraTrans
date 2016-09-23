using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 에라번역
{
    [Serializable]
    public class Setting
    {
        public CheckState KoreanCB { get; set; }
        public CheckState JapaneseCB { get; set; }
        public CheckState EtcCB { get; set; }
        public LineSetting LineSetting { get; set; }
        public AuthorSetting AuthorSetting { get; set; }
        public Encoding ErbEncoding { get; set; }
        public Setting(CheckState koreanCB, CheckState japaneseCB, CheckState etcCB, LineSetting lineSetting,AuthorSetting authorSetting, Encoding erbEncoding)
        {
            KoreanCB = koreanCB;
            JapaneseCB = japaneseCB;
            EtcCB = etcCB;
            LineSetting = lineSetting;
            AuthorSetting = authorSetting;
            ErbEncoding = erbEncoding;
        }
    }
}
