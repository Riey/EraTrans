using Riey.EZTrans;
using System;
using System.Windows.Forms;

namespace YeongHun.EraTrans
{
    public partial class ChangeForm : Form
    {
        public static string TranslatedText = null;
        bool exit = false;
        public ChangeForm(NodeInfo item)
        {
            TranslatedText = null;
            InitializeComponent();
            Original_Text.Text = item.LineInfo.OriginalString;
            Translated_Text.Text = item.LineInfo.Str;
            현재줄.Text = item.ErbFileName + Environment.NewLine + item.LineNo + "번째줄";
        }

        private void Change_Form_Load(object sender, EventArgs e)
        {
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
            DialogResult result = MessageBox.Show(현재줄.Text + "의\r\n" + Original_Text.Text + "\t\t을\r\n" + Translated_Text.Text + "\t\t로 번역하시겠습니까?", "번역", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
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
            Translated_Text.Text = TranslateXP.Translate(Original_Text.Text);
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
