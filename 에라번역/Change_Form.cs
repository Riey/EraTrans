using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Text;
using System.IO;
using Fillter;
using System.Security.Cryptography;

namespace 에라번역
{
    public partial class Change_Form : Form
    {
        Translate Translate;
        IReceiveData<string> receive;
        string line;
        string original;
        bool exit = false;
        string trans = null;
        public Change_Form(NodeInfo item, IReceiveData<string> receive, Translate Translate)
        {
            this.receive = receive;
            line = item.erb_filename + "\r\n" + item.line + "번째줄";
            original = item.info.str;
            this.Translate = Translate;
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
                receive.SetData(new string[] { null });
                return;
            }
            if (exit)
            {
                receive.SetData(new string[] { null });
                return;
            }
            DialogResult result = MessageBox.Show(line + "의\r\n" + original + "\t\t을\r\n" + Translated_Text.Text + "\t\t로 번역하시겠습니까?", "번역", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if(Translated_Text.Text.First()!=' ')
                {
                    Translated_Text.Text = " " + Translated_Text.Text;
                }
                receive.SetData(new string[] { Translated_Text.Text });
            }
            if (result == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
        }

        private void 자동번역버튼_Click(object sender, EventArgs e)
        {
            if (trans != null)
            {
                Translated_Text.Text = trans;
                return;
            }
            Translated_Text.Text = Translate.번역(Original_Text.Text);
            trans = Translated_Text.Text;
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
