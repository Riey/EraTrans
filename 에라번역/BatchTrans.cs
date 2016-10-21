using Fillter;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class BatchTrans : Form
    {
        private Dictionary<string, ERB_Parser> parsers;
        private Stack<ChangeLog> logs;
        public BatchTrans(Dictionary<string, ERB_Parser> parsers, Stack<ChangeLog> logs)
        {
            this.parsers = parsers;
            this.logs = logs;
            InitializeComponent();
        }

        private void 적용버튼_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(번역본.Text))
            {
                if (번역본.Text.Contains("%") || 번역본.Text.Contains("{") || 번역본.Text.Contains("}") || 번역본.Text.Contains("\\@"))
                {
                    MessageBox.Show("바뀔 문자열에 위험한 문자가 포함되어 있습니다", "경고", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                DialogResult result = MessageBox.Show(원본.Text + "를(을) " + 번역본.Text + "로 정말 바꾸겠습니까?", "변경", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    foreach (var parser in parsers)
                    {
                        foreach (var item in parser.Value.StringDictionary)
                        {
                            if (item.Value.Str.Contains(원본.Text))
                            {
                                item.Value.Str = item.Value.Str.Replace(원본.Text, 번역본.Text);
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