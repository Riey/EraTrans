using Fillter2.Parsing;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class BatchTrans : Form
    {
        private Dictionary<string, ErbParser> parsers;
        private Stack<ChangeLog> logs;
        public BatchTrans(Dictionary<string, ErbParser> parsers,Stack<ChangeLog>logs)
        {
            this.parsers = parsers;
            this.logs = logs;
            InitializeComponent();
        }

        private void 적용버튼_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(번역본.Text))
            {
                DialogResult result = MessageBox.Show(원본.Text + "를(을) " + 번역본.Text + "로 정말 바꾸겠습니까?", "변경", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    foreach (var parser in parsers)
                    {
                        foreach(var line in parser.Value.PrintLines)
                        {
                            if (line.Value.PrintStr.Contains(원본.Text))
                            {
                                line.Value.PrintStr = line.Value.PrintStr.Replace(원본.Text, 번역본.Text);
                                logs.Push(new ChangeLog(parser.Key, 원본.Text, 번역본.Text));
                            }
                        }
                    }
                    Close();
                }
            }
            else
            {
                MessageBox.Show("글자를 입력해주세요");
            }
        }
    }
}
