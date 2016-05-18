namespace 에라번역
{
    partial class Translate_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Translate_Form));
            this.번역버튼 = new System.Windows.Forms.Button();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.korean_cb = new System.Windows.Forms.CheckBox();
            this.japanese_cb = new System.Windows.Forms.CheckBox();
            this.etc_cb = new System.Windows.Forms.CheckBox();
            this.삭제버튼 = new System.Windows.Forms.Button();
            this.설정버튼 = new System.Windows.Forms.Button();
            this.일괄번역버튼 = new System.Windows.Forms.Button();
            this.다시실행버튼 = new System.Windows.Forms.Button();
            this.실행취소버튼 = new System.Windows.Forms.Button();
            this.새로고침버튼 = new System.Windows.Forms.Button();
            this.전체줄수 = new System.Windows.Forms.TextBox();
            this.자동번역버튼 = new System.Windows.Forms.Button();
            this.빈줄표시 = new System.Windows.Forms.CheckBox();
            this.word_list = new TreeViewMS.TreeViewMS();
            this.SuspendLayout();
            // 
            // 번역버튼
            // 
            this.번역버튼.Location = new System.Drawing.Point(1113, 13);
            this.번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.번역버튼.Name = "번역버튼";
            this.번역버튼.Size = new System.Drawing.Size(100, 44);
            this.번역버튼.TabIndex = 1;
            this.번역버튼.Text = "번역하기";
            this.번역버튼.UseVisualStyleBackColor = true;
            this.번역버튼.Click += new System.EventHandler(this.번역버튼_Click);
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(1113, 65);
            this.저장버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(100, 44);
            this.저장버튼.TabIndex = 2;
            this.저장버튼.Text = "저장하기";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // korean_cb
            // 
            this.korean_cb.AutoSize = true;
            this.korean_cb.Checked = true;
            this.korean_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.korean_cb.Location = new System.Drawing.Point(1113, 639);
            this.korean_cb.Name = "korean_cb";
            this.korean_cb.Size = new System.Drawing.Size(62, 19);
            this.korean_cb.TabIndex = 3;
            this.korean_cb.Text = "한국어";
            this.korean_cb.ThreeState = true;
            this.korean_cb.UseVisualStyleBackColor = true;
            this.korean_cb.CheckStateChanged += new System.EventHandler(this.표시언어바꾸기);
            // 
            // japanese_cb
            // 
            this.japanese_cb.AutoSize = true;
            this.japanese_cb.Checked = true;
            this.japanese_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.japanese_cb.Location = new System.Drawing.Point(1113, 664);
            this.japanese_cb.Name = "japanese_cb";
            this.japanese_cb.Size = new System.Drawing.Size(62, 19);
            this.japanese_cb.TabIndex = 4;
            this.japanese_cb.Text = "일본어";
            this.japanese_cb.ThreeState = true;
            this.japanese_cb.UseVisualStyleBackColor = true;
            this.japanese_cb.CheckStateChanged += new System.EventHandler(this.표시언어바꾸기);
            // 
            // etc_cb
            // 
            this.etc_cb.AutoSize = true;
            this.etc_cb.Checked = true;
            this.etc_cb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.etc_cb.Location = new System.Drawing.Point(1113, 689);
            this.etc_cb.Name = "etc_cb";
            this.etc_cb.Size = new System.Drawing.Size(50, 19);
            this.etc_cb.TabIndex = 5;
            this.etc_cb.Text = "그외";
            this.etc_cb.UseVisualStyleBackColor = true;
            this.etc_cb.CheckStateChanged += new System.EventHandler(this.표시언어바꾸기);
            // 
            // 삭제버튼
            // 
            this.삭제버튼.Location = new System.Drawing.Point(1113, 325);
            this.삭제버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.삭제버튼.Name = "삭제버튼";
            this.삭제버튼.Size = new System.Drawing.Size(100, 44);
            this.삭제버튼.TabIndex = 6;
            this.삭제버튼.Text = "삭제하기";
            this.삭제버튼.UseVisualStyleBackColor = true;
            this.삭제버튼.Click += new System.EventHandler(this.삭제버튼_Click);
            // 
            // 설정버튼
            // 
            this.설정버튼.Location = new System.Drawing.Point(1113, 429);
            this.설정버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.설정버튼.Name = "설정버튼";
            this.설정버튼.Size = new System.Drawing.Size(100, 44);
            this.설정버튼.TabIndex = 7;
            this.설정버튼.Text = "설정";
            this.설정버튼.UseVisualStyleBackColor = true;
            this.설정버튼.Click += new System.EventHandler(this.설정버튼_Click);
            // 
            // 일괄번역버튼
            // 
            this.일괄번역버튼.Location = new System.Drawing.Point(1113, 117);
            this.일괄번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.일괄번역버튼.Name = "일괄번역버튼";
            this.일괄번역버튼.Size = new System.Drawing.Size(100, 44);
            this.일괄번역버튼.TabIndex = 8;
            this.일괄번역버튼.Text = "일괄번역";
            this.일괄번역버튼.UseVisualStyleBackColor = true;
            this.일괄번역버튼.Click += new System.EventHandler(this.일괄번역버튼_Click);
            // 
            // 다시실행버튼
            // 
            this.다시실행버튼.ForeColor = System.Drawing.SystemColors.ControlText;
            this.다시실행버튼.Location = new System.Drawing.Point(1113, 273);
            this.다시실행버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.다시실행버튼.Name = "다시실행버튼";
            this.다시실행버튼.Size = new System.Drawing.Size(100, 44);
            this.다시실행버튼.TabIndex = 9;
            this.다시실행버튼.Text = "다시실행";
            this.다시실행버튼.UseVisualStyleBackColor = true;
            this.다시실행버튼.Click += new System.EventHandler(this.다시실행버튼_Click);
            // 
            // 실행취소버튼
            // 
            this.실행취소버튼.ForeColor = System.Drawing.SystemColors.ControlText;
            this.실행취소버튼.Location = new System.Drawing.Point(1113, 221);
            this.실행취소버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.실행취소버튼.Name = "실행취소버튼";
            this.실행취소버튼.Size = new System.Drawing.Size(100, 44);
            this.실행취소버튼.TabIndex = 10;
            this.실행취소버튼.Text = "실행취소";
            this.실행취소버튼.UseVisualStyleBackColor = true;
            this.실행취소버튼.Click += new System.EventHandler(this.실행취소버튼_Click);
            // 
            // 새로고침버튼
            // 
            this.새로고침버튼.Location = new System.Drawing.Point(1113, 377);
            this.새로고침버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.새로고침버튼.Name = "새로고침버튼";
            this.새로고침버튼.Size = new System.Drawing.Size(100, 44);
            this.새로고침버튼.TabIndex = 11;
            this.새로고침버튼.Text = "새로고침";
            this.새로고침버튼.UseVisualStyleBackColor = true;
            this.새로고침버튼.Click += new System.EventHandler(this.새로고침버튼_Click);
            // 
            // 전체줄수
            // 
            this.전체줄수.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.전체줄수.Cursor = System.Windows.Forms.Cursors.Default;
            this.전체줄수.Location = new System.Drawing.Point(993, 737);
            this.전체줄수.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.전체줄수.Name = "전체줄수";
            this.전체줄수.ReadOnly = true;
            this.전체줄수.Size = new System.Drawing.Size(100, 16);
            this.전체줄수.TabIndex = 12;
            // 
            // 자동번역버튼
            // 
            this.자동번역버튼.Location = new System.Drawing.Point(1113, 169);
            this.자동번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.자동번역버튼.Name = "자동번역버튼";
            this.자동번역버튼.Size = new System.Drawing.Size(100, 44);
            this.자동번역버튼.TabIndex = 17;
            this.자동번역버튼.Text = "자동번역";
            this.자동번역버튼.UseVisualStyleBackColor = true;
            this.자동번역버튼.Click += new System.EventHandler(this.자동번역버튼_Click);
            // 
            // 빈줄표시
            // 
            this.빈줄표시.AutoSize = true;
            this.빈줄표시.Location = new System.Drawing.Point(1113, 583);
            this.빈줄표시.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.빈줄표시.Name = "빈줄표시";
            this.빈줄표시.Size = new System.Drawing.Size(74, 19);
            this.빈줄표시.TabIndex = 18;
            this.빈줄표시.Text = "빈줄표시";
            this.빈줄표시.UseVisualStyleBackColor = true;
            this.빈줄표시.CheckedChanged += new System.EventHandler(this.빈줄표시_CheckedChanged);
            // 
            // word_list
            // 
            this.word_list.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.word_list.Location = new System.Drawing.Point(12, 13);
            this.word_list.Margin = new System.Windows.Forms.Padding(3, 3, 20, 3);
            this.word_list.Name = "word_list";
            this.word_list.SelectedNodes = ((System.Collections.ArrayList)(resources.GetObject("word_list.SelectedNodes")));
            this.word_list.ShowLines = false;
            this.word_list.ShowPlusMinus = false;
            this.word_list.Size = new System.Drawing.Size(1078, 865);
            this.word_list.TabIndex = 19;
            this.word_list.KeyDown += new System.Windows.Forms.KeyEventHandler(this.word_list_KeyDown);
            this.word_list.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.word_list_MouseDoubleClick);
            // 
            // Translate_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 890);
            this.Controls.Add(this.word_list);
            this.Controls.Add(this.빈줄표시);
            this.Controls.Add(this.자동번역버튼);
            this.Controls.Add(this.전체줄수);
            this.Controls.Add(this.새로고침버튼);
            this.Controls.Add(this.실행취소버튼);
            this.Controls.Add(this.다시실행버튼);
            this.Controls.Add(this.일괄번역버튼);
            this.Controls.Add(this.설정버튼);
            this.Controls.Add(this.삭제버튼);
            this.Controls.Add(this.etc_cb);
            this.Controls.Add(this.japanese_cb);
            this.Controls.Add(this.korean_cb);
            this.Controls.Add(this.저장버튼);
            this.Controls.Add(this.번역버튼);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Translate_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "작업창";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Translate_Form_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Translate_Form_FormClosed);
            this.Load += new System.EventHandler(this.Translate_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button 번역버튼;
        private System.Windows.Forms.Button 저장버튼;
        private System.Windows.Forms.CheckBox korean_cb;
        private System.Windows.Forms.CheckBox japanese_cb;
        private System.Windows.Forms.CheckBox etc_cb;
        private System.Windows.Forms.Button 삭제버튼;
        private System.Windows.Forms.Button 설정버튼;
        private System.Windows.Forms.Button 일괄번역버튼;
        private System.Windows.Forms.Button 다시실행버튼;
        private System.Windows.Forms.Button 실행취소버튼;
        private System.Windows.Forms.Button 새로고침버튼;
        private System.Windows.Forms.TextBox 전체줄수;
        private System.Windows.Forms.Button 자동번역버튼;
        private System.Windows.Forms.CheckBox 빈줄표시;
        private TreeViewMS.TreeViewMS word_list;
    }
}