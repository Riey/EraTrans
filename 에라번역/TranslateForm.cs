using EZTrans;
using Fillter2.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 에라번역
{
    public partial class TranslateForm : Form
    {
        #region Const Value
        private const string ERB_TAG = "ERB";
        private const string LIST_TAG = "LIST";
        #endregion
        public TranslateForm(Dictionary<string,ErbParser>parsers,Setting setting, string version)
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
            etc_cb.CheckState = setting.EtcCB;
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
            if(string.IsNullOrWhiteSpace(lineInfo.PrintStr))
                return false;
            if(!((korean_cb.CheckState == CheckState.Unchecked && lineInfo.IsKorean) || (japanese_cb.CheckState == CheckState.Unchecked && lineInfo.IsJapanese) || (!etc_cb.Checked && !lineInfo.IsKorean && !lineInfo.IsJapanese)))
            {
                if(korean_cb.CheckState == CheckState.Unchecked && !lineInfo.IsKorean)
                    return false;
                if(japanese_cb.CheckState == CheckState.Unchecked && !lineInfo.IsJapanese)
                    return false;
                if((korean_cb.Checked && lineInfo.IsKorean) || (japanese_cb.Checked && lineInfo.IsJapanese))
                    return true;
                if((korean_cb.CheckState == CheckState.Indeterminate && lineInfo.IsKorean) || (japanese_cb.CheckState == CheckState.Indeterminate && lineInfo.IsJapanese))
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
                foreach(var lineInfo in parser.Value.PrintLines)
                {
                    if(!IsVaildLine(lineInfo.Value))
                    {
                        continue;
                    }
                    var nodeInfo = new NodeInfo(lineInfo.Key, parser.Key, lineInfo.Value);
                    var node = new TreeNode();
                    node.Tag = nodeInfo;
                    node.Text = nodeInfo.GetString(setting.LineSetting);
                    erbNode.Nodes.Add(node);
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
                ChangeForm cf = new ChangeForm(item);
                cf.ShowDialog();
                if (ChangeForm.TranslatedText == null)
                    continue;
                logs.Push(new ChangeLog(item.ErbName, item.Info.PrintStr, ChangeForm.TranslatedText));
                parsers[item.ErbName].PrintLines[item.LineNo].PrintStr = ChangeForm.TranslatedText;
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
            setting.EtcCB = etc_cb.CheckState;
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
                parser.Value.Save(parser.Key);
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
            BatchTrans bt = new BatchTrans(parsers, logs);
            bt.ShowDialog();
            changed = true;
            Refresh_Word();
        }

        private void 실행취소버튼_Click(object sender, EventArgs e)
        {
            if (logs.Count > 0)
            {
                back_logs.Push(ChangeLog.Back(_logs.Pop(), parsers));
                    Refresh_Word();
                changed = true;
            }
        }

        private void 다시실행버튼_Click(object sender, EventArgs e)
        {
            if (back_logs.Count > 0)
            {
                _logs.Push(ChangeLog.Back(back_logs.Pop(), parsers));
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
                if (string.IsNullOrWhiteSpace(item.Info.PrintStr))
                    continue;
                string result = "";
                if (item.Info.IsForm)
                {
                    var temp = new StringBuilder();
                    var terms = item.Info.PrintStr.Split('%');

                    for(int i = 0; i < terms.Length; i++)
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

                    result = temp.ToString();
                }
                else
                {
                    result = TranslateXP.Translate(item.Info.PrintStr);
                }
                logs.Push(new ChangeLog(item.ErbName, item.LineNo, item.Info.PrintStr, result));
                item.Info.PrintStr = result;
                Node.Text = item.GetString(setting.LineSetting);
            }
            word_list.EndUpdate();
            changed = true;
        }
        #region Field
        public delegate void CheckLogHandler(Button btn, bool enable);
        private bool changed = false;
        private Dictionary<string, ErbParser> parsers;
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
}
