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
    public partial class 단어추가창 : Form
    {
        bool c_ori = false, c_recom = false, c_ex = false;
        public 단어추가창()
        {
            InitializeComponent();
        }
        private void text_Clicked(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text == box.Name)
                box.Text = "";
        }
        private void text_leaved(object sender, EventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box.Text == "")
                box.Text = box.Name;
        }
        private void text_changed(object sender,EventArgs e)
        {
            TextBox tb = sender as TextBox;
            switch (tb.Name)
            {
                case ("원본"):
                    {
                        c_ori = true;
                        break;
                    }
                case ("추천단어"):
                    {
                        c_recom = true;
                        break;
                    }
                case ("설명"):
                    {
                        c_ex = true;
                        break;
                    }
            }
        }
        private void 추가버튼_Click(object sender, EventArgs e)
        {
            if (c_ori)
            {
                c_ori = false;

            }
        }
    }
}
