using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using Fillter;
using System.Diagnostics;

namespace 에라번역
{
    public partial class Translate_Form : Form
    {
        public Translate_Form(Dictionary<string,ERB_Parser>parsers,Stack<ChangeLog>logs,Stack<ChangeLog>back_logs,Setting setting, string version)
        {
            this.parsers = parsers;
            this.version = version;
            this.setting = setting;
            p_logs = logs;
            this.back_logs = back_logs;
            InitializeComponent();
        }

        private void Translate_Form_Load(object sender, EventArgs e)
        {
            korean_cb.CheckState = setting.KoreanCB;
            japanese_cb.CheckState = setting.JapaneseCB;
            etc_cb.CheckState = setting.etcCB;
            LineSetting = setting.LineSetting;
            logthread = new Thread(() =>
            {
                Action<Button, bool> method = (Button btn, bool enable) => { btn.Enabled = enable; };
                try
                {
                    while (true)
                    {

                        if (p_logs.Count > 0)
                        {
                            Invoke(new LogHandler(method), new object[] { 실행취소버튼, true });
                        }
                        else
                        {
                            Invoke(new LogHandler(method), new object[] { 실행취소버튼, false });
                        }
                        if (back_logs.Count > 0)
                        {
                            Invoke(new LogHandler(method), new object[] { 다시실행버튼, true });
                        }
                        else
                        {
                            Invoke(new LogHandler(method), new object[] { 다시실행버튼, false });
                        }
                        Thread.Sleep(200);
                    }

                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            });
            logthread.Start();
            word_update();
            Init = false;
            int line = 0;
            foreach (TreeNode erb_node in word_list.Nodes)
            {
                foreach(TreeNode node in erb_node.Nodes)
                {
                    line += node.Nodes.Count == 0 ? 1 : node.Nodes.Count;
                }
            }
            전체줄수.Text = line.ToString() + "줄";
        }
        private void word_update()
        {
            word_list.Nodes.Clear();
            foreach (var parser in parsers)
            {
                var items = parser.Value.dic.Where(item =>
                {
                    if (!((korean_cb.CheckState == CheckState.Unchecked && item.Value.Korean) || (japanese_cb.CheckState == CheckState.Unchecked && item.Value.Japanese) || (!etc_cb.Checked && !item.Value.Korean && !item.Value.Japanese)))
                    {
                        if (korean_cb.CheckState == CheckState.Unchecked && !item.Value.Korean)
                            return false;
                        if (japanese_cb.CheckState == CheckState.Unchecked && !item.Value.Japanese)
                            return false;
                        if ((korean_cb.Checked && item.Value.Korean) || (japanese_cb.Checked && item.Value.Japanese))
                            return true;
                        if ((korean_cb.CheckState == CheckState.Indeterminate && item.Value.Korean) || (japanese_cb.CheckState == CheckState.Indeterminate && item.Value.Japanese))
                            return true;
                    }
                    return true;
                }).Where(item =>
                {
                    return !(string.IsNullOrWhiteSpace(item.Value.str) && !빈줄표시.Checked);
                }).Select(item => new NodeInfo(item.Key, parser.Key, item.Value)).ToArray();
                foreach (var item in items)
                {
                    if (!word_list.Nodes.ContainsKey(item.erb_name))
                    {
                        TreeNode erbNode = new TreeNode(item.erb_filename);
                        erbNode.Name = item.erb_name;
                        erbNode.Tag = "ERB";
                        word_list.Nodes.Add(erbNode);
                    }
                    var erb_node = word_list.Nodes.Find(item.erb_name, false).First();
                    var node = new TreeNode();
                    node.Tag = item;
                    node.Text = item.GetString(setting.LineSetting);
                    if (item.info.IsList)
                    {
                        if (!erb_node.Nodes.ContainsKey("DATALIST|" + item.info.parent_line))
                        {
                            TreeNode listNode = new TreeNode(LineSetting.GetLine(item.info.parent_line, " DATALIST", LineSetting));
                            listNode.Name = "DATALIST|" + item.info.parent_line;
                            listNode.Tag = "LIST";
                            erb_node.Nodes.Add(listNode);
                        }
                        var list_node = erb_node.Nodes.Find("DATALIST|" + item.info.parent_line, false).First();
                        list_node.Nodes.Add(node);
                    }
                    else
                    {
                        erb_node.Nodes.Add(node);
                    }
                }
            }
        }
        private void 번역버튼_Click(object sender, EventArgs e)
        {
            if (word_list.SelectedNodes.Count == 0)
                return;
            word_list.BeginUpdate();
            foreach (TreeNode Node in word_list.SelectedNodes)
            {
                if (!(Node.Tag is NodeInfo))
                    continue;
                NodeInfo item = Node.Tag as NodeInfo;
                Change_Form cf = new Change_Form(item, trans);
                cf.ShowDialog();
                if (Change_Form.TranslatedText == null)
                    continue;
                logs.Push(new ChangeLog(item.erb_name, item.info.str, Change_Form.TranslatedText));
                parsers[item.erb_name].dic[item.line].str = Change_Form.TranslatedText;
                Node.Text = item.GetString(setting.LineSetting);
            }
            word_list.EndUpdate();
            changed = true;
        }
        private void 표시언어바꾸기(object sender, EventArgs e)
        {
            if (Init) return;
            setting.KoreanCB = korean_cb.CheckState;
            setting.JapaneseCB = japanese_cb.CheckState;
            setting.etcCB = etc_cb.CheckState;
            word_update();
        }
        private void 저장버튼_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            foreach (var parser in parsers)
            {
                parser.Value.Save(); 
            }
            MessageBox.Show("저장완료!");
            changed = false;
        }
        private void Translate_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changed)
            {
                if (MessageBox.Show("저장되지 않았습니다 나가시겠습니까?", "경고", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
            logthread.Abort();
            GC.Collect();
        }

        private void 삭제버튼_Click(object sender, EventArgs e)
        {
            if (word_list.SelectedNodes.Count == 0)
                return;
            DialogResult result = MessageBox.Show("정말 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                foreach (TreeNode Node in word_list.SelectedNodes)
                {
                    if (!(Node.Tag is NodeInfo))
                    {
                        if ((string)Node.Tag == "ERB")
                        {
                            if (!parsers.ContainsKey(Node.Name))
                                continue;
                            parsers.Remove(Node.Name);
                            word_list.SelectedNodes.Clear();
                        }
                        continue;
                    }
                    if (Node.Name.Split('|').First() == "DATALIST")
                    {
                        foreach(TreeNode node in Node.Nodes)
                        {
                            NodeInfo nodeinfo = node.Tag as NodeInfo;
                            logs.Push(new ChangeLog(nodeinfo.erb_name, nodeinfo.line));
                            parsers[nodeinfo.erb_name].Remove(nodeinfo.line);
                        }
                    }
                    else
                    {
                        NodeInfo node = Node.Tag as NodeInfo;
                        logs.Push(new ChangeLog(node.erb_name, node.line));
                        parsers[node.erb_name].Remove(node.line);
                    }
                }
                word_update();
                changed = true;
            }
        }
        private void 설정버튼_Click(object sender, EventArgs e)
        {
            설정창 설정 = new 설정창(setting, parsers);
            설정.ShowDialog();
            Refresh_Word();
        }
        private void 일괄번역버튼_Click(object sender, EventArgs e)
        {
            Batch_Trans bt = new Batch_Trans(parsers, logs);
            bt.ShowDialog();
            changed = true;
            Refresh_Word();
        }

        private void 실행취소버튼_Click(object sender, EventArgs e)
        {
            if (logs.Count > 0)
            {
                back_logs.Push(ChangeLog.되돌리기(p_logs.Pop(), parsers));
                word_update();
                changed = true;
            }
        }

        private void 다시실행버튼_Click(object sender, EventArgs e)
        {
            if (back_logs.Count > 0)
            {
                p_logs.Push(ChangeLog.되돌리기(back_logs.Pop(), parsers));
                word_update();
                changed = true;
            }
        }

        private void Translate_Form_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void Refresh_Word()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            word_list.BeginUpdate();
            foreach (TreeNode erb_node in word_list.Nodes)
            {
                foreach (TreeNode node in erb_node.Nodes)
                {
                    if (node.Name.Split('|').First() == "DATALIST")
                        continue;
                    NodeInfo nodeInfo = node.Tag as NodeInfo;
                    node.Text = nodeInfo.GetString(setting.LineSetting);
                }
            }
            word_list.EndUpdate();
#if DEBUG
            sw.Stop();
            MessageBox.Show("Refresh 걸린시간:" + sw.ElapsedMilliseconds + "ms");
#endif
        }
        private void 새로고침버튼_Click(object sender, EventArgs e)
        {
            Refresh_Word();
        }

        private void word_list_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            번역버튼_Click(null, null);
        }

        private void word_list_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                번역버튼_Click(null, null);
            }
            if (e.Alt && e.KeyCode == Keys.S)
            {
                //저장
                Save();
            }
        }

        private void 자동번역버튼_Click(object sender, EventArgs e)
        {
            if (word_list.SelectedNodes.Count == 0)
                return;
            word_list.BeginUpdate();
            foreach (TreeNode Node in word_list.SelectedNodes)
            {
                if (!(Node.Tag is NodeInfo))
                    continue;
                var item = Node.Tag as NodeInfo;
                if (string.IsNullOrWhiteSpace(item.info.str))
                    continue;
                string temp = trans.번역(item.info.str);
                logs.Push(new ChangeLog(item.erb_name, item.line, item.info.str, temp));
                item.info.str = temp;
                Node.Text = item.GetString(setting.LineSetting);
            }
            word_list.EndUpdate();
            changed = true;
        }
        #region 필드
        public delegate void LogHandler(Button btn, bool enable);
        bool changed = false;
        Dictionary<string,ERB_Parser> parsers;
        string version;
        LineSetting LineSetting;
        Stack<ChangeLog> p_logs;
        Stack<ChangeLog> logs
        {
            get
            {
                back_logs.Clear();
                return p_logs;
            }
        }
        Stack<ChangeLog> back_logs = new Stack<ChangeLog>();
        Translate trans = new Translate();
        Thread logthread;
        private Setting setting;
        private bool Init = true;
        #endregion

        private void 빈줄표시_CheckedChanged(object sender, EventArgs e)
        {
            word_update();
        }
    }
    [Serializable]
    public class AuthorSetting
    {
        public string 이름 { get; set; }
        public string 설명 { get; set; }
        public static AuthorSetting Default
        {
            get
            {
                return new AuthorSetting("");
            }
        }

        public AuthorSetting(string 이름)
        {
            this.이름 = 이름;
            설명 = "";
        }
    }
    [Serializable]
    public class ChangeLog
    {
        public enum 행동
        {
            번역, 일괄번역, 삭제, 복원
        }
        public string erb_name { get; }
        public int 줄번호 { get; }
        public string str1 { get; }
        public string str2 { get; }
        public 행동 했던일;
        public ChangeLog(string erb_name,int line, string 원본, string 번역본)
        {
            //번역용 생성자
            this.erb_name = erb_name;
            줄번호 = line;
            했던일 = 행동.번역;
            str1 = 원본;
            str2 = 번역본;
            /*
            삭제의 경우엔
            str1:dic의 삭제된내용
            str2:_dic의 삭제된내용

            복원의 경우는
            str1,str2가 존재하지않음
            */
        }
        public ChangeLog(string erb_name, string 원본, string 일괄번역본)
        {
            //일괄번역용 생성자
            this.erb_name = erb_name;
            했던일 = 행동.일괄번역;
            str1 = 원본;
            str2 = 일괄번역본;
        }
        public ChangeLog(string erb_name, int line, ERB_Parser parser)
        {
            //삭제용 생성자
            this.erb_name = erb_name;
            줄번호 = line;
            했던일 = 행동.삭제;
            str1 = parser.dic[line].str;
            str2 = parser._dic[line];
        }
        public ChangeLog(string erb_name, int line)
        {
            //복원용 생성자
            this.erb_name = erb_name;
            줄번호 = line;
            했던일 = 행동.복원;
        }
        public ChangeLog(string erb_name, int 줄번호, string str1, string str2, 행동 했던일)
        {
            this.erb_name = erb_name;
            this.줄번호 = 줄번호;
            this.str1 = str1;
            this.str2 = str2;
            this.했던일 = 했던일;
        }
        public static bool 같은가(ChangeLog log1, ChangeLog log2)
        {
            if (log1.erb_name != log2.erb_name)
                return false;
            if (log1.했던일 == 행동.삭제 && log2.했던일 == 행동.삭제 && log1.줄번호 == log2.줄번호)
            {
                return true;
            }
            if (log1.str1 == log2.str1 && log1.str2 == log2.str2 && log1.줄번호 == log2.줄번호 && log1.했던일 == log2.했던일)
            {
                return true;
            }
            return false;
        }
        public static ChangeLog 다시실행(ChangeLog log, Dictionary<string,ERB_Parser>parsers)
        {
            switch (log.했던일)
            {
                case (행동.번역):
                    {
                        return 되돌리기(new ChangeLog(log.erb_name, log.줄번호, log.str2, log.str1), parsers);
                    }
                case (행동.일괄번역):
                    {
                        return 되돌리기(new ChangeLog(log.erb_name, log.str2, log.str1), parsers);
                    }
                case (행동.삭제):
                    {
                        return 되돌리기(new ChangeLog(log.erb_name, log.줄번호, parsers[log.erb_name]), parsers);
                    }
                case (행동.복원):
                    {
                        return 되돌리기(new ChangeLog(log.erb_name, log.줄번호), parsers);
                    }
            }
            return null;
        }
        public static ChangeLog 되돌리기(ChangeLog log, Dictionary<string, ERB_Parser> parsers)
        {
            ChangeLog cl;
            switch (log.했던일)
            {
                case (행동.번역):
                    {
                        parsers[log.erb_name].dic[log.줄번호].str = log.str1;
                        cl = new ChangeLog(log.erb_name, log.줄번호, log.str2, log.str1);
                        break;
                    }
                case (행동.일괄번역):
                    {
                        List<Tuple<int, string>> diclog = new List<Tuple<int, string>>();
                        foreach (var temp in parsers[log.erb_name].dic)
                        {
                            diclog.Add(new Tuple<int, string>(temp.Key, temp.Value.str.Replace(log.str2, log.str1)));
                        }
                        foreach (var temp in diclog)
                        {
                            parsers[log.erb_name].dic[temp.Item1] = new LineInfo(temp.Item2);
                        }
                        cl = new ChangeLog(log.erb_name, log.str2, log.str1);
                        break;
                    }
                case (행동.삭제):
                    {
                        parsers[log.erb_name].Add(log.줄번호, log.str1, log.str2);
                        cl = new ChangeLog(log.erb_name, log.줄번호);
                        break;
                    }
                case (행동.복원):
                    {
                        cl = new ChangeLog(log.erb_name, log.줄번호, parsers[log.erb_name]);
                        parsers[log.erb_name].Remove(log.줄번호);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("행동을 알수없습니다.");
                    }
            }
            return cl;
        }
    }
    [Serializable]
    public class LineSetting
    {
        public string setting { get; set; }
        public string[] str;
        public LineSetting(string setting, string[] str)
        {
            this.setting = setting;
            this.str = str;
        }
        public static LineSetting Default
        {
            get
            {
                return new LineSetting("LINENUM+str+LINETEXT", new string[] { "번째줄===>" });
            }
        }
        public static string GetLine(int linenum, string linetext, LineSetting setting)
        {
            string result = "";
            int count = 0;
            foreach (string temp in setting.setting.Split('+'))
            {
                switch (temp)
                {
                    case ("LINENUM"):
                        {
                            result += linenum;
                            break;
                        }
                    case ("LINETEXT"):
                        {
                            result += linetext;
                            break;
                        }
                    case ("str"):
                        {
                            if (count <= setting.str.Length)
                            {
                                result += setting.str[count++];
                            }
                            break;
                        }
                }
            }
            return result;
        }
    }
    public class NodeInfo
    {
        public int line;
        public string erb_name;
        public string erb_filename;
        public string ID
        {
            get
            {
                return erb_name + "|" + line;
            }
        }
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
        public LineInfo info;
        public string GetString(LineSetting setting)
        {
            return LineSetting.GetLine(line+1, info.str, setting);
        }
        public NodeInfo(int line,string erb_name, LineInfo info)
        {
            this.line = line;
            this.erb_name = erb_name;
            this.info = info;
            erb_filename = erb_name.Split('\\').Last();
        }
    }
}
