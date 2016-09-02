using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class LineSetting_Change_Form : Form
    {
        LineSetting setting;
        public LineSetting_Change_Form(LineSetting setting)
        {
            this.setting = setting;
            InitializeComponent();
        }

        private void LineSetting_Form_Load(object sender, EventArgs e)
        {
            Setting_Text.Text = setting.Setting;
            for(int i=0;i<setting.Str.Length-1;i++)
            {
                str_text.Text += setting.Str[i] + ",";
            }
            str_text.Text += setting.Str.Last();
        }

        private void 적용버튼_Click(object sender, EventArgs e)
        {
            setting.Setting = Setting_Text.Text;
            setting.Str = str_text.Text.Split(',');
            Close();
        }
    }
}
