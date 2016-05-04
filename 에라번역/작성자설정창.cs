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
    public partial class 작성자설정창 : Form
    {
        AuthorSetting setting;
        public 작성자설정창(AuthorSetting setting)
        {
            this.setting = setting;
            InitializeComponent();
        }

        private void 적용버튼_Click(object sender, EventArgs e)
        {
            setting.이름 = 이름.Text;
            setting.설명 = 설명.Text;
            Close();
        }

        private void 작성자설정창_Load(object sender, EventArgs e)
        {
            이름.Text = setting.이름;
            설명.Text = setting.설명;
        }
    }
}
