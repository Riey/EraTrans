using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Fillter;
namespace 에라번역
{
    public partial class 설정창 : Form
    {
        private Dictionary<string, ERB_Parser> parsers;
        private Setting setting;
        public 설정창(Setting setting, Dictionary<string, ERB_Parser> parsers)
        {
            this.setting = setting;
            this.parsers = parsers;
            InitializeComponent();
        }

        private void 줄표시변경버튼_Click(object sender, EventArgs e)
        {
            LineSetting_Change_Form ls = new LineSetting_Change_Form(setting.LineSetting);
            ls.ShowDialog();
        }

        private void 작성자설정버튼_Click(object sender, EventArgs e)
        {
            작성자설정창 설정창 = new 작성자설정창(setting.AuthorSetting);
            설정창.ShowDialog();
        }

        private void 저장버튼_Click(object sender, EventArgs e)
        {
            foreach (var parser in parsers)
            {
                switch (인코딩설정.SelectedIndex)
                {
                    case (0):
                        {
                            parser.Value.erb_encoding = Encoding.UTF8;
                            break;
                        }
                    case (1):
                        {
                            parser.Value.erb_encoding = Encoding.Unicode;
                            break;
                        }
                    case (2):
                        {
                            parser.Value.erb_encoding = Encoding.UTF32;
                            break;
                        }
                    case (3):
                        {
                            break;
                        }
                } 
            }
            Close();
        }
        
        private void 설정창_Load(object sender, EventArgs e)
        {
            인코딩설정.Items.AddRange(new string[] { "UTF-8", "UTF-16(Unicode)", "UTF-32" });
            if (parsers.First().Value.erb_encoding == Encoding.UTF8)
            {
                인코딩설정.SelectedIndex = 0;
            }
            else if (parsers.First().Value.erb_encoding == Encoding.Unicode)
            {
                인코딩설정.SelectedIndex = 1;
            }
            else if (parsers.First().Value.erb_encoding == Encoding.UTF32)
            {
                인코딩설정.SelectedIndex = 2;
            }
            else
            {
                인코딩설정.Items.Add(parsers.First().Value.erb_encoding.WebName);
                인코딩설정.SelectedIndex = 1;
            }
        }
    }
}
