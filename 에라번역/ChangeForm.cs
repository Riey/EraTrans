using System;
using System.Linq;
using System.Windows.Forms;
using EZTrans;
using System.Text;

namespace 에라번역
{
    public partial class ChangeForm : Form
    {
        public static string TranslatedText = null;
        string line;
        string original;
        bool exit = false;
        bool isForm;
        public ChangeForm(NodeInfo item)
        {
            TranslatedText = null;
            line = item.ErbFileName + "\r\n" + item.LineNo + "번째줄";
            original = item.Info.PrintStr;
            isForm = item.Info.IsForm;
            InitializeComponent();
        }

        private void Change_Form_Load(object sender, EventArgs e)
        {
            Original_Text.Text = original;
            현재줄.Text = line;
        }

        private void Change_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(Translated_Text.Text))
            {
                TranslatedText = null;
                return;
            }
            if (exit)
            {
                TranslatedText = null;
                return;
            }
            DialogResult result = MessageBox.Show(line + "의\r\n" + original + "\t\t을\r\n" + Translated_Text.Text + "\t\t로 번역하시겠습니까?", "번역", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if(Translated_Text.Text.First()!=' ')
                {
                    Translated_Text.Text = " " + Translated_Text.Text;
                }
                TranslatedText = Translated_Text.Text;
            }
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }

        private void 자동번역버튼_Click(object sender, EventArgs e)
        {
            if (isForm)
            {
                var temp = new StringBuilder();
                var terms = Original_Text.Text.Split('%');
                for (int i = 0; i < terms.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        temp.Append(TranslateXP.Translate(terms[i]));
                    }
                    else
                    {
                        temp.Append('%');
                        temp.Append(terms[i]);
                        temp.Append('%');
                    }
                }
                Translated_Text.Text = temp.ToString();
            }
            else
            {
                Translated_Text.Text = TranslateXP.Translate(Original_Text.Text);
            }
        }
        private void 종료버튼_Click(object sender, EventArgs e)
        {
            exit = true;
            Close();
        }

        private void 복사버튼_Click(object sender, EventArgs e)
        {
            Translated_Text.Text = Original_Text.Text;
        }

        private void Translated_Text_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Close();
            }
        }

        private void 저장버튼_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
