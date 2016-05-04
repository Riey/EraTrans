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
    public partial class Select_Line : Form
    {
        CrashLog[] logs;
        List<ChangeLog> 일괄번역로그;
        List<ChangeLog> 최종로그;

        public Select_Line(CrashLog[] logs, List<ChangeLog> 일괄번역로그, List<ChangeLog> 최종로그)
        {
            this.최종로그 = 최종로그;
            this.logs = logs;
            this.일괄번역로그 = 일괄번역로그;
            InitializeComponent();
        }

        private void 선택버튼_Click(object sender, EventArgs e)
        {
            if (crash_list.SelectedIndex == -1)
                return;
            else
            {
                CrashListItem item = crash_list.SelectedItem as CrashListItem;
                if (item.selected)
                    item.selected = false;
                else
                    item.selected = true;
            }
        }

        private void 새로만들기버튼_Click(object sender, EventArgs e)
        {
            int index = crash_list.SelectedIndex;
            if (index == -1)
                return;
            if (!string.IsNullOrEmpty(new_text.Text))
            {
                CrashListItem item = crash_list.SelectedItem as CrashListItem;
                CrashListItem newitem = new CrashListItem(new ChangeLog(item.log.줄번호,item.log.str1,new_text.Text,item.log.했던일));
                newitem.selected = true;
                crash_list.Items.Insert(index, newitem);
            }
        }

        private void Select_Line_Load(object sender, EventArgs e)
        {
            foreach(var log in logs)
            {
                crash_list.Items.Add(new CrashListItem(log.log1));
                crash_list.Items.Add(new CrashListItem(log.log2));
            }
            foreach(var log in 일괄번역로그)
            {
                crash_list.Items.Add(new CrashListItem(log, "일괄번역:{" + log.str1 + "}===>{" + log.str2 + "}"));
            }
        }

        private void 저장버튼_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("정말 저장하시겠습니까?","",MessageBoxButtons.OKCancel,MessageBoxIcon.Information)==DialogResult.OK)
            {
                List<int> check = new List<int>();
                foreach(CrashListItem item in crash_list.Items)
                {
                    if (item.selected)
                    {
                        if (item.log.했던일 == ChangeLog.행동.일괄번역)
                        {
                            최종로그.Add(item.log);
                        }
                        if (check.Contains(item.log.줄번호))
                        {
                            continue;
                        }
                        else
                        {
                            check.Add(item.log.줄번호);
                            최종로그.Add(item.log);
                        }
                    }
                }
                Close(); 
            }
        }

        private void crash_list_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1)
                return;
            CrashListItem item = crash_list.Items[e.Index] as CrashListItem;
            e.DrawBackground();
            if (!item.selected)
                e.Graphics.DrawString(item.text, crash_list.Font, new SolidBrush(Color.Black), e.Bounds);
            else
                e.Graphics.DrawString(item.text, crash_list.Font, new SolidBrush(Color.LightSkyBlue), e.Bounds);
            e.DrawFocusRectangle();
        }
    }
    class CrashListItem
    {
        public ChangeLog log { get; }
        public string text { get; }
        public bool selected = false;
        public CrashListItem(ChangeLog log,string text)
        {
            this.text = text;
            this.log = log;
        }
        public CrashListItem(ChangeLog log)
        {
            this.log = log;
            text = log.했던일.ToString() + ":" + log.줄번호 + "번째줄 {"+log.str1+"}===>{" + log.str2+"}";
        }
    }
}
