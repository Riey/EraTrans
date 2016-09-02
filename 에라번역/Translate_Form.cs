using EZTrans;
using Fillter;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class Translate_Form : Form
    {
        #region Const Value
        private const string ERB_TAG = "ERB";
        private const string LIST_TAG = "LIST";
        #endregion
        public Translate_Form(Dictionary<string,ERB_Parser>parsers,Setting setting, string version)
        {
            this.parsers = parsers;
            this.version = version;
            this.setting = setting;
            InitializeComponent();
        }

        private void Translate_Form_Load(object sender, EventArgs e)
        {
            korean_cb.CheckState = setting.KoreanCB;
            japanese_cb.CheckState = setting.JapaneseCB;
            etc_cb.CheckState = setting.etcCB;
            currentLineSetting = setting.LineSetting;
            logWatcher = new Thread(CheckLog);
            logWatcher.Start();
            word_update();
            Init = false;
            int line = GetLineCount();
            전체줄수.Text = line.ToString() + "줄";
        }

        private void CheckLog()
        {
            Action<Button, bool> method = (Button btn, bool enable) => { btn.Enabled = enable; };
            try
            {
                while(true)
                {

                    if(_logs.Count > 0)
                    {
                        Invoke(new CheckLogHandler(method), 실행취소버튼, true);
                    }
                    else
                    {
                        Invoke(new CheckLogHandler(method), 실행취소버튼, false);
                    }
                    if(back_logs.Count > 0)
                    {
                        Invoke(new CheckLogHandler(method), 다시실행버튼, true);
                    }
                    else
                    {
                        Invoke(new CheckLogHandler(method), 다시실행버튼, false);
                    }
                    Thread.Sleep(200);
                }

            }
            catch(ObjectDisposedException)
            {
                return;
            }
        }

        private int GetLineCount()
        {
            int line = 0;
            foreach(TreeNode erb_node in word_list.Nodes)
            {
                foreach(TreeNode node in erb_node.Nodes)
                {
                    line += node.Nodes.Count == 0 ? 1 : node.Nodes.Count;
                }
            }

            return line;
        }

        private bool IsVaildLine(LineInfo lineInfo)
        {
            if(string.IsNullOrWhiteSpace(lineInfo.Str))
                return false;
            if(!((korean_cb.CheckState == CheckState.Unchecked && lineInfo.Korean) || (japanese_cb.CheckState == CheckState.Unchecked && lineInfo.Japanese) || (!etc_cb.Checked && !lineInfo.Korean && !lineInfo.Japanese)))
            {
                if(korean_cb.CheckState == CheckState.Unchecked && !lineInfo.Korean)
                    return false;
                if(japanese_cb.CheckState == CheckState.Unchecked && !lineInfo.Japanese)
                    return false;
                if((korean_cb.Checked && lineInfo.Korean) || (japanese_cb.Checked && lineInfo.Japanese))
                    return true;
                if((korean_cb.CheckState == CheckState.Indeterminate && lineInfo.Korean) || (japanese_cb.CheckState == CheckState.Indeterminate && lineInfo.Japanese))
                    return true;
            }
            return true;
        }
        private void word_update()
        {
            TreeNode Top = word_list.TopNode;
            var extends = word_list.Nodes.Cast<TreeNode>().Where(node => node.IsExpanded).Select(node => node.Name).ToArray();
            word_list.Nodes.Clear();
            var erbNodes = new Dictionary<string, TreeNode>();
            AddErbNodes(erbNodes);
            foreach(var extend in extends)
            {
                erbNodes[extend].Expand();
            }
            word_list.Nodes.AddRange(erbNodes.Values.ToArray());
            if(Top != null)
            {
                word_list.TopNode = word_list.Nodes.Find(Top.Name, true).First();
            }
        }

        private void AddErbNodes(Dictionary<string, TreeNode> erbNodes)
        {
            foreach(var parser in parsers)
            {
                var erbNode = new TreeNode(parser.Key.Split('\\').Last());
                erbNode.Name = parser.Key;
                erbNode.Tag = ERB_TAG;
                erbNodes.Add(erbNode.Name, erbNode);
                foreach(var lineInfo in parser.Value.StringDictionary)
                {
                    if(!IsVaildLine(lineInfo.Value))
                    {
                        continue;
                    }
                    var nodeInfo = new NodeInfo(lineInfo.Key, parser.Key, lineInfo.Value);
                    var node = new TreeNode();
                    node.Tag = nodeInfo;
                    node.Text = nodeInfo.GetString(setting.LineSetting);
                    if(nodeInfo.Info.IsList)
                    {
                        if(!erbNode.Nodes.ContainsKey("DATALIST|" + nodeInfo.Info.Parent_line))
                        {
                            TreeNode listNode = new TreeNode(currentLineSetting.GetLine(nodeInfo.Info.Parent_line, " DATALIST"));
                            listNode.Name = "DATALIST|" + nodeInfo.Info.Parent_line;
                            listNode.Tag = LIST_TAG;
                            erbNode.Nodes.Add(listNode);
                        }
                        var list_node = erbNode.Nodes.Find("DATALIST|" + nodeInfo.Info.Parent_line, false).First();
                        list_node.Nodes.Add(node);
                    }
                    else
                    {
                        erbNode.Nodes.Add(node);
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
                Change_Form cf = new Change_Form(item);
                cf.ShowDialog();
                if (Change_Form.TranslatedText == null)
                    continue;
                logs.Push(new ChangeLog(item.ErbName, item.Info.Str, Change_Form.TranslatedText));
                parsers[item.ErbName].StringDictionary[item.Line].Str = Change_Form.TranslatedText;
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
            Parallel.ForEach(parsers, parser =>
            {
                parser.Value.Save();
            });
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
            logWatcher.Abort();
            GC.Collect();
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
                back_logs.Push(ChangeLog.되돌리기(_logs.Pop(), parsers));
                    Refresh_Word();
                changed = true;
            }
        }

        private void 다시실행버튼_Click(object sender, EventArgs e)
        {
            if (back_logs.Count > 0)
            {
                _logs.Push(ChangeLog.되돌리기(back_logs.Pop(), parsers));
                    Refresh_Word();
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
                if (string.IsNullOrWhiteSpace(item.Info.Str))
                    continue;
                string temp = TranslateXP.Translate(item.Info.Str);
                logs.Push(new ChangeLog(item.ErbName, item.Line, item.Info.Str, temp));
                item.Info.Str = temp;
                Node.Text = item.GetString(setting.LineSetting);
            }
            word_list.EndUpdate();
            changed = true;
        }
        #region Field
        public delegate void CheckLogHandler(Button btn, bool enable);
        private bool changed = false;
        private Dictionary<string,ERB_Parser> parsers;
        private string version;
        private LineSetting currentLineSetting;
        private Stack<ChangeLog> _logs=new Stack<ChangeLog>();
        private Stack<ChangeLog> logs
        {
            get
            {
                back_logs.Clear();
                return _logs;
            }
        }
        private Stack<ChangeLog> back_logs = new Stack<ChangeLog>();
        private Thread logWatcher;
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
            번역, 일괄번역
        }
        public string erb_name { get; }
        public int 줄번호 { get; }
        public string str1 { get; }
        public string str2 { get; }
        public 행동 했던일 { get; }
        public ChangeLog(string erb_name,int line, string 원본, string 번역본):this(erb_name,line,원본,번역본,행동.번역)
        {
            //번역용 생성자
        }
        public ChangeLog(string erb_name, string 원본, string 일괄번역본) : this(erb_name, -1, 원본, 일괄번역본, 행동.일괄번역)
        {
            //일괄번역용 생성자
        }
        private ChangeLog(string erb_name, int 줄번호, string str1, string str2, 행동 했던일)
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
                        parsers[log.erb_name].StringDictionary[log.줄번호].Str = log.str1;
                        cl = new ChangeLog(log.erb_name, log.줄번호, log.str2, log.str1);
                        break;
                    }
                case (행동.일괄번역):
                    {
                        List<Tuple<int, string>> diclog = new List<Tuple<int, string>>();
                        foreach (var temp in parsers[log.erb_name].StringDictionary)
                        {
                            diclog.Add(new Tuple<int, string>(temp.Key, temp.Value.Str.Replace(log.str2, log.str1)));
                        }
                        foreach (var temp in diclog)
                        {
                            parsers[log.erb_name].StringDictionary[temp.Item1] = new LineInfo(temp.Item2);
                        }
                        cl = new ChangeLog(log.erb_name, log.str2, log.str1);
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
}
