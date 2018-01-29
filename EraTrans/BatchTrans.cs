using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Riey.EraTrans
{
    public partial class BatchTrans : Form
    {
        private Dictionary<string, ErbParser> parsers;
        private Stack<ChangeLog> logs;
        public BatchTrans(Dictionary<string, ErbParser> parsers, Stack<ChangeLog> logs)
        {
            this.parsers = parsers;
            this.logs = logs;
            InitializeComponent();
        }

        private void �����ư_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(������.Text))
            {
                if (������.Text.Contains("%") || ������.Text.Contains("{") || ������.Text.Contains("}") || ������.Text.Contains("\\@"))
                {
                    MessageBox.Show("�ٲ� ���ڿ��� ������ ���ڰ� ���ԵǾ� �ֽ��ϴ�", "���", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                DialogResult result = MessageBox.Show(����.Text + "��(��) " + ������.Text + "�� ���� �ٲٰڽ��ϱ�?", "����", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    foreach (var parser in parsers)
                    {
                        foreach (var item in parser.Value.StringDictionary)
                        {
                            if (item.Value.Str.Contains(����.Text))
                            {
                                item.Value.Str = item.Value.Str.Replace(����.Text, ������.Text);
                                logs.Push(new ChangeLog(parser.Key, ����.Text, ������.Text));
                            }
                        }
                    }
                    Close();
                }
            }
            else
            {
                MessageBox.Show("���ڸ� �Է����ּ���");
            }
        }
    }
}