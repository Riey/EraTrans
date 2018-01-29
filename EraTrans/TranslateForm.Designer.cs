namespace Riey.EraTrans
{
    partial class TranslateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslateForm));
            this.wordList = new TreeViewMS.TreeViewMS();
            this.번역버튼 = new System.Windows.Forms.Button();
            this.toolPanal = new System.Windows.Forms.FlowLayoutPanel();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.일괄번역버튼 = new System.Windows.Forms.Button();
            this.자동번역버튼 = new System.Windows.Forms.Button();
            this.실행취소버튼 = new System.Windows.Forms.Button();
            this.다시실행버튼 = new System.Windows.Forms.Button();
            this.새로고침버튼 = new System.Windows.Forms.Button();
            this.korean_cb = new System.Windows.Forms.CheckBox();
            this.japanese_cb = new System.Windows.Forms.CheckBox();
            this.etc_cb = new System.Windows.Forms.CheckBox();
            this.saveOriginalCB = new System.Windows.Forms.CheckBox();
            this.toolPanal.SuspendLayout();
            this.SuspendLayout();
            // 
            // wordList
            // 
            this.wordList.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.wordList.Location = new System.Drawing.Point(12, 12);
            this.wordList.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.wordList.Name = "wordList";
            this.wordList.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("wordList.SelectedNodes")));
            this.wordList.ShowLines = false;
            this.wordList.ShowPlusMinus = false;
            this.wordList.Size = new System.Drawing.Size(1078, 848);
            this.wordList.TabIndex = 19;
            this.wordList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.word_list_KeyDown);
            this.wordList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.word_list_MouseDoubleClick);
            // 
            // 번역버튼
            // 
            this.번역버튼.Location = new System.Drawing.Point(3, 4);
            this.번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.번역버튼.Name = "번역버튼";
            this.번역버튼.Size = new System.Drawing.Size(100, 44);
            this.번역버튼.TabIndex = 1;
            this.번역버튼.Text = "번역하기";
            this.번역버튼.UseVisualStyleBackColor = true;
            this.번역버튼.Click += new System.EventHandler(this.번역버튼_Click);
            // 
            // toolPanal
            // 
            this.toolPanal.Controls.Add(this.번역버튼);
            this.toolPanal.Controls.Add(this.저장버튼);
            this.toolPanal.Controls.Add(this.일괄번역버튼);
            this.toolPanal.Controls.Add(this.자동번역버튼);
            this.toolPanal.Controls.Add(this.실행취소버튼);
            this.toolPanal.Controls.Add(this.다시실행버튼);
            this.toolPanal.Controls.Add(this.새로고침버튼);
            this.toolPanal.Controls.Add(this.korean_cb);
            this.toolPanal.Controls.Add(this.japanese_cb);
            this.toolPanal.Controls.Add(this.etc_cb);
            this.toolPanal.Controls.Add(this.saveOriginalCB);
            this.toolPanal.Location = new System.Drawing.Point(1098, 12);
            this.toolPanal.Name = "toolPanal";
            this.toolPanal.Size = new System.Drawing.Size(109, 849);
            this.toolPanal.TabIndex = 20;
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(3, 56);
            this.저장버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(100, 44);
            this.저장버튼.TabIndex = 2;
            this.저장버튼.Text = "저장하기";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // 일괄번역버튼
            // 
            this.일괄번역버튼.Location = new System.Drawing.Point(3, 108);
            this.일괄번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.일괄번역버튼.Name = "일괄번역버튼";
            this.일괄번역버튼.Size = new System.Drawing.Size(100, 44);
            this.일괄번역버튼.TabIndex = 8;
            this.일괄번역버튼.Text = "일괄번역";
            this.일괄번역버튼.UseVisualStyleBackColor = true;
            this.일괄번역버튼.Click += new System.EventHandler(this.일괄번역버튼_Click);
            // 
            // 자동번역버튼
            // 
            this.자동번역버튼.Location = new System.Drawing.Point(3, 160);
            this.자동번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.자동번역버튼.Name = "자동번역버튼";
            this.자동번역버튼.Size = new System.Drawing.Size(100, 44);
            this.자동번역버튼.TabIndex = 17;
            this.자동번역버튼.Text = "자동번역";
            this.자동번역버튼.UseVisualStyleBackColor = true;
            this.자동번역버튼.Click += new System.EventHandler(this.자동번역버튼_Click);
            // 
            // 실행취소버튼
            // 
            this.실행취소버튼.ForeColor = System.Drawing.SystemColors.ControlText;
            this.실행취소버튼.Location = new System.Drawing.Point(3, 212);
            this.실행취소버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.실행취소버튼.Name = "실행취소버튼";
            this.실행취소버튼.Size = new System.Drawing.Size(100, 44);
            this.실행취소버튼.TabIndex = 10;
            this.실행취소버튼.Text = "실행취소";
            this.실행취소버튼.UseVisualStyleBackColor = true;
            this.실행취소버튼.Click += new System.EventHandler(this.실행취소버튼_Click);
            // 
            // 다시실행버튼
            // 
            this.다시실행버튼.ForeColor = System.Drawing.SystemColors.ControlText;
            this.다시실행버튼.Location = new System.Drawing.Point(3, 264);
            this.다시실행버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.다시실행버튼.Name = "다시실행버튼";
            this.다시실행버튼.Size = new System.Drawing.Size(100, 44);
            this.다시실행버튼.TabIndex = 9;
            this.다시실행버튼.Text = "다시실행";
            this.다시실행버튼.UseVisualStyleBackColor = true;
            this.다시실행버튼.Click += new System.EventHandler(this.다시실행버튼_Click);
            // 
            // 새로고침버튼
            // 
            this.새로고침버튼.Location = new System.Drawing.Point(3, 316);
            this.새로고침버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.새로고침버튼.Name = "새로고침버튼";
            this.새로고침버튼.Size = new System.Drawing.Size(100, 44);
            this.새로고침버튼.TabIndex = 11;
            this.새로고침버튼.Text = "새로고침";
            this.새로고침버튼.UseVisualStyleBackColor = true;
            this.새로고침버튼.Click += new System.EventHandler(this.새로고침버튼_Click);
            // 
            // korean_cb
            // 
            this.korean_cb.AutoSize = true;
            this.korean_cb.Checked = true;
            this.korean_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.korean_cb.Location = new System.Drawing.Point(3, 514);
            this.korean_cb.Margin = new System.Windows.Forms.Padding(3, 150, 3, 3);
            this.korean_cb.Name = "korean_cb";
            this.korean_cb.Size = new System.Drawing.Size(62, 19);
            this.korean_cb.TabIndex = 3;
            this.korean_cb.Text = "한국어";
            this.korean_cb.UseVisualStyleBackColor = true;
            this.korean_cb.CheckStateChanged += new System.EventHandler(this.displayLanguageChagned);
            // 
            // japanese_cb
            // 
            this.japanese_cb.AutoSize = true;
            this.japanese_cb.Checked = true;
            this.japanese_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.japanese_cb.Location = new System.Drawing.Point(3, 539);
            this.japanese_cb.Name = "japanese_cb";
            this.japanese_cb.Size = new System.Drawing.Size(62, 19);
            this.japanese_cb.TabIndex = 4;
            this.japanese_cb.Text = "일본어";
            this.japanese_cb.UseVisualStyleBackColor = true;
            this.japanese_cb.CheckStateChanged += new System.EventHandler(this.displayLanguageChagned);
            // 
            // etc_cb
            // 
            this.etc_cb.AutoSize = true;
            this.etc_cb.Checked = true;
            this.etc_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.etc_cb.Location = new System.Drawing.Point(3, 564);
            this.etc_cb.Name = "etc_cb";
            this.etc_cb.Size = new System.Drawing.Size(50, 19);
            this.etc_cb.TabIndex = 5;
            this.etc_cb.Text = "그외";
            this.etc_cb.UseVisualStyleBackColor = true;
            this.etc_cb.CheckStateChanged += new System.EventHandler(this.displayLanguageChagned);
            // 
            // saveOriginalCB
            // 
            this.saveOriginalCB.AutoSize = true;
            this.saveOriginalCB.Checked = true;
            this.saveOriginalCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveOriginalCB.Location = new System.Drawing.Point(3, 686);
            this.saveOriginalCB.Margin = new System.Windows.Forms.Padding(3, 100, 3, 3);
            this.saveOriginalCB.Name = "saveOriginalCB";
            this.saveOriginalCB.Size = new System.Drawing.Size(90, 19);
            this.saveOriginalCB.TabIndex = 18;
            this.saveOriginalCB.Text = "원본을 저장";
            this.saveOriginalCB.UseVisualStyleBackColor = true;
            // 
            // TranslateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 873);
            this.Controls.Add(this.toolPanal);
            this.Controls.Add(this.wordList);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TranslateForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "작업창";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TranslateForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Translate_Form_FormClosed);
            this.Load += new System.EventHandler(this.Translate_Form_Load);
            this.SizeChanged += new System.EventHandler(this.TranslateForm_SizeChanged);
            this.toolPanal.ResumeLayout(false);
            this.toolPanal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeViewMS.TreeViewMS wordList;
        private System.Windows.Forms.Button 번역버튼;
        private System.Windows.Forms.FlowLayoutPanel toolPanal;
        private System.Windows.Forms.Button 저장버튼;
        private System.Windows.Forms.Button 일괄번역버튼;
        private System.Windows.Forms.Button 자동번역버튼;
        private System.Windows.Forms.Button 실행취소버튼;
        private System.Windows.Forms.Button 다시실행버튼;
        private System.Windows.Forms.Button 새로고침버튼;
        private System.Windows.Forms.CheckBox korean_cb;
        private System.Windows.Forms.CheckBox japanese_cb;
        private System.Windows.Forms.CheckBox etc_cb;
        private System.Windows.Forms.CheckBox saveOriginalCB;
    }
}