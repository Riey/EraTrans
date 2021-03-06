﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Riey.EraTrans
{
    public partial class TranslateForm : Form
    {
        #region Const Value
        private const string ERB_TAG = "ERB";
        private const string LIST_TAG = "LIST";
        private const string PRINTDATA_TAG = "PDAT";
        #endregion

        #region Field
        public delegate void CheckLogHandler(Button btn, bool enable);
        private bool changed = false;
        private Dictionary<string, ErbParser> parsers;
        private string version;
        private LineSetting currentLineSetting;
        private Stack<ChangeLog> _logs = new Stack<ChangeLog>();
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

        public TranslateForm(Dictionary<string, ErbParser> parsers, Setting setting, string version)
        {
            this.parsers = parsers;
            this.version = version;
            this.setting = setting;
            InitializeComponent();
        }

        private void Translate_Form_Load(object sender, EventArgs e)
        {
            DesktopLocation = setting.PreviousFormPosition;
            Size = setting.PreviousFormSize;

            korean_cb.CheckState = setting.KoreanCB;
            japanese_cb.CheckState = setting.JapaneseCB;
            etc_cb.CheckState = setting.EtcCB;
            saveOriginalCB.Checked = setting.OutputType == ErbParser.OutputType.Working;


            currentLineSetting = setting.LineSetting;
            logWatcher = new Thread(CheckLog);
            logWatcher.Start();
            word_update();
            Init = false;
            int line = GetLineCount();
        }

        private void CheckLog()
        {
            Action<Button, bool> method = (Button btn, bool enable) => { btn.Enabled = enable; };
            try
            {
                while (true)
                {

                    if (_logs.Count > 0)
                    {
                        Invoke(new CheckLogHandler(method), 실행취소버튼, true);
                    }
                    else
                    {
                        Invoke(new CheckLogHandler(method), 실행취소버튼, false);
                    }
                    if (back_logs.Count > 0)
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
            catch (Exception)
            {
                return;
            }
        }

        private int GetLineCount()
        {
            int line = 0;
            foreach (TreeNode erb_node in wordList.Nodes)
            {
                foreach (TreeNode node in erb_node.Nodes)
                {
                    line += node.Nodes.Count == 0 ? 1 : node.Nodes.Count;
                }
            }

            return line;
        }

        private bool IsVaildLine(LineInfo lineInfo)
        {
            if (string.IsNullOrWhiteSpace(lineInfo.Str))
                return false;
            if ((korean_cb.CheckState == CheckState.Unchecked && lineInfo.Korean) || (japanese_cb.CheckState == CheckState.Unchecked && lineInfo.Japanese) || (!etc_cb.Checked && !lineInfo.Korean && !lineInfo.Japanese))
                return false;
            if ((korean_cb.Checked && lineInfo.Korean) || (japanese_cb.Checked && lineInfo.Japanese))
                return true;
            return true;
        }

        private void word_update()
        {
            TreeNode Top = wordList.TopNode;
            var extends = wordList.Nodes.Cast<TreeNode>().Where(node => node.IsExpanded).Select(node => node.Name).ToArray();
            wordList.Nodes.Clear();
            var erbNodes = new Dictionary<string, TreeNode>();

            AddErbNodes(erbNodes);

            foreach (var extend in extends)
            {
                erbNodes[extend].Expand();
            }

            wordList.Nodes.AddRange(erbNodes.Values.ToArray());
            if (Top != null)
            {
                wordList.TopNode = wordList.Nodes.Find(Top.Name, true).First();
            }
        }

        private void AddNode(NodeInfo nodeInfo, TreeNodeCollection nodes, TreeNode erbNode)
        {
            var node = new TreeNode();
            node.Tag = nodeInfo;
            node.Text = nodeInfo.GetString(setting.LineSetting);
            if (nodeInfo.LineInfo.IsData)
            {
                if (!erbNode.Nodes.ContainsKey("PRINTDATA|" + nodeInfo.LineInfo.PrintDataLine + 1))
                {
                    var printNode = new TreeNode(currentLineSetting.GetLine(nodeInfo.LineInfo.PrintDataLine + 1, "PRINTDATA"));
                    printNode.Name = "PRINTDATA|" + nodeInfo.LineInfo.PrintDataLine + 1;
                    printNode.Tag = PRINTDATA_TAG;
                    erbNode.Nodes.Add(printNode);
                }
                {
                    var printNode = erbNode.Nodes.Find("PRINTDATA|" + nodeInfo.LineInfo.PrintDataLine + 1, false)[0];
                    if (nodeInfo.LineInfo.IsList)
                    {
                        if (!printNode.Nodes.ContainsKey("DATALIST|" + nodeInfo.LineInfo.ListLine + 1))
                        {
                            var listNode = new TreeNode(currentLineSetting.GetLine(nodeInfo.LineInfo.ListLine + 1, "DATALIST"));
                            listNode.Name = "DATALIST|" + nodeInfo.LineInfo.ListLine + 1;
                            listNode.Tag = LIST_TAG;
                            printNode.Nodes.Add(listNode);
                        }
                        {
                            var listNode = printNode.Nodes.Find("DATALIST|" + nodeInfo.LineInfo.ListLine + 1, false)[0];
                            listNode.Nodes.Add(node);
                        }
                    }
                    else
                    {
                        printNode.Nodes.Add(node);
                    }
                }
            }
            else
            {
                erbNode.Nodes.Add(node);
            }
        }

        private void AddErbNodes(Dictionary<string, TreeNode> erbNodes)
        {
            foreach (var parser in parsers)
            {
                var erbNode = new TreeNode(Path.GetFileName(parser.Key));
                erbNode.Name = parser.Key;
                erbNode.Tag = ERB_TAG;
                erbNodes.Add(erbNode.Name, erbNode);
                foreach (var lineInfo in parser.Value.StringDictionary)
                {
                    if (!IsVaildLine(lineInfo.Value))
                    {
                        continue;
                    }
                    AddNode(new NodeInfo(lineInfo.Key, parser.Key, lineInfo.Value), wordList.Nodes, erbNode);
                }
            }
        }

        private void 번역버튼_Click(object sender, EventArgs e)
        {
            if (wordList.SelectedNodes.Count == 0)
                return;
            wordList.BeginUpdate();
            foreach (TreeNode Node in wordList.SelectedNodes)
            {
                if (!(Node.Tag is NodeInfo))
                    continue;
                var item = Node.Tag as NodeInfo;
                var cf = new ChangeForm(item);
                cf.ShowDialog();
                if (ChangeForm.TranslatedText == null)
                    continue;
                logs.Push(new ChangeLog(item.ErbPath, item.LineInfo.Str, ChangeForm.TranslatedText));
                parsers[item.ErbPath].StringDictionary[item.LineNo].Str = ChangeForm.TranslatedText;
                Node.Text = item.GetString(setting.LineSetting);
            }
            wordList.EndUpdate();
            changed = true;
        }

        private void displayLanguageChagned(object sender, EventArgs e)
        {
            if (Init)
                return;
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
            foreach (var parser in parsers)
            {
                //Set Output type
                parser.Value.Save(saveOriginalCB.Checked ? ErbParser.OutputType.Working : ErbParser.OutputType.Release);
            }
            MessageBox.Show("저장완료!");
            changed = false;
        }
        private void TranslateForm_FormClosing(object sender, FormClosingEventArgs e)
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
            setting.PreviousFormSize = Size;
            setting.PreviousFormPosition = DesktopLocation;
            setting.OutputType = saveOriginalCB.Checked ? ErbParser.OutputType.Working : ErbParser.OutputType.Release;
        }
        private void 일괄번역버튼_Click(object sender, EventArgs e)
        {
            var bt = new BatchTrans(parsers, logs);
            bt.ShowDialog();
            changed = true;
            RefreshWord();
        }

        private void 실행취소버튼_Click(object sender, EventArgs e)
        {
            if (logs.Count > 0)
            {
                back_logs.Push(ChangeLog.Back(_logs.Pop(), parsers));
                RefreshWord();
                changed = true;
            }
        }

        private void 다시실행버튼_Click(object sender, EventArgs e)
        {
            if (back_logs.Count > 0)
            {
                _logs.Push(ChangeLog.Back(back_logs.Pop(), parsers));
                RefreshWord();
                changed = true;
            }
        }

        private void Translate_Form_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        private void RefreshWord()
        {
            wordList.BeginUpdate();
            foreach (TreeNode erb_node in wordList.Nodes)
            {
                foreach (TreeNode node in erb_node.Nodes)
                {
                    if (!(node.Tag is NodeInfo))
                        continue;
                    var nodeInfo = node.Tag as NodeInfo;
                    node.Text = nodeInfo.GetString(setting.LineSetting);
                }
            }
            wordList.EndUpdate();
        }
        private void 새로고침버튼_Click(object sender, EventArgs e)
        {
            RefreshWord();
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
                Save();
            }
        }


        private void 자동번역버튼_Click(object sender, EventArgs e)
        {
            if (wordList.SelectedNodes.Count == 0)
                return;
            wordList.BeginUpdate();

            var temp = new Queue<TreeNode>();

            Action<TreeNode> AddNode = null;
            AddNode =
                treeNode =>
            {
                if (!(treeNode.Tag is NodeInfo))
                {
                    foreach (TreeNode node in treeNode.Nodes)
                        AddNode(node);
                }
                else
                    temp.Enqueue(treeNode);
            };

            foreach (TreeNode node in wordList.SelectedNodes)
            {
                AddNode(node);
            }


            while (temp.Count > 0)
            {
                var node = temp.Dequeue();
                var item = node.Tag as NodeInfo;
                if (string.IsNullOrWhiteSpace(item.LineInfo.Str))
                    continue;
                string trans = AutoTransFillter.TranslateWithFillter(item.LineInfo);
                if (trans != null)
                {
                    logs.Push(new ChangeLog(item.ErbPath, item.LineNo, item.LineInfo.Str, trans));
                    item.LineInfo.Str = trans;
                    node.Text = item.GetString(setting.LineSetting);
                }
            }

            wordList.EndUpdate();
            changed = true;
        }

        private void 빈줄표시_CheckedChanged(object sender, EventArgs e)
        {
            word_update();
        }

        private void TranslateForm_SizeChanged(object sender, EventArgs e)
        {
            wordList.Width = Width - 174;
            wordList.Height = Height - 64;
            toolPanal.Location = new System.Drawing.Point(Width - 154, wordList.Location.Y);
            toolPanal.Height = wordList.Height;
        }
    }
}
