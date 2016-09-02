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
        public CheckState KoreanCB;
        public CheckState JapaneseCB;
        public CheckState etcCB;
        public LineSetting LineSetting;
        public AuthorSetting AuthorSetting;
        public Setting(CheckState KoreanCB, CheckState JapaneseCB, CheckState etcCB, LineSetting LineSetting,AuthorSetting AuthorSetting)
        {
            this.KoreanCB = KoreanCB;
            this.JapaneseCB = JapaneseCB;
            this.etcCB = etcCB;
            this.LineSetting = LineSetting;
            this.AuthorSetting = AuthorSetting;
        }
    }
}
