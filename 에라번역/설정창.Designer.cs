namespace 에라번역
{
    partial class 설정창
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
            this.작성자설정버튼 = new System.Windows.Forms.Button();
            this.줄표시변경버튼 = new System.Windows.Forms.Button();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.사전파일열기 = new System.Windows.Forms.OpenFileDialog();
            this.인코딩설정 = new System.Windows.Forms.ComboBox();
            this.설명1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // 작성자설정버튼
            // 
            this.작성자설정버튼.Location = new System.Drawing.Point(29, 77);
            this.작성자설정버튼.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.작성자설정버튼.Name = "작성자설정버튼";
            this.작성자설정버튼.Size = new System.Drawing.Size(91, 35);
            this.작성자설정버튼.TabIndex = 1;
            this.작성자설정버튼.Text = "작성자설정";
            this.작성자설정버튼.UseVisualStyleBackColor = true;
            this.작성자설정버튼.Click += new System.EventHandler(this.작성자설정버튼_Click);
            // 
            // 줄표시변경버튼
            // 
            this.줄표시변경버튼.Location = new System.Drawing.Point(29, 24);
            this.줄표시변경버튼.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.줄표시변경버튼.Name = "줄표시변경버튼";
            this.줄표시변경버튼.Size = new System.Drawing.Size(91, 35);
            this.줄표시변경버튼.TabIndex = 2;
            this.줄표시변경버튼.Text = "줄표시변경";
            this.줄표시변경버튼.UseVisualStyleBackColor = true;
            this.줄표시변경버튼.Click += new System.EventHandler(this.줄표시변경버튼_Click);
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(197, 226);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(75, 23);
            this.저장버튼.TabIndex = 3;
            this.저장버튼.Text = "저장";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // 사전파일열기
            // 
            this.사전파일열기.FileName = "openFileDialog1";
            this.사전파일열기.Filter = "사전파일|*.xml";
            // 
            // 인코딩설정
            // 
            this.인코딩설정.BackColor = System.Drawing.SystemColors.Window;
            this.인코딩설정.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.인코딩설정.Font = new System.Drawing.Font("굴림", 8F);
            this.인코딩설정.ForeColor = System.Drawing.SystemColors.WindowText;
            this.인코딩설정.FormattingEnabled = true;
            this.인코딩설정.Location = new System.Drawing.Point(29, 226);
            this.인코딩설정.Name = "인코딩설정";
            this.인코딩설정.Size = new System.Drawing.Size(123, 19);
            this.인코딩설정.TabIndex = 4;
            // 
            // 설명1
            // 
            this.설명1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.설명1.Location = new System.Drawing.Point(29, 206);
            this.설명1.Name = "설명1";
            this.설명1.ReadOnly = true;
            this.설명1.Size = new System.Drawing.Size(91, 14);
            this.설명1.TabIndex = 5;
            this.설명1.Text = "출력인코딩설정";
            // 
            // 설정창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.설명1);
            this.Controls.Add(this.인코딩설정);
            this.Controls.Add(this.저장버튼);
            this.Controls.Add(this.줄표시변경버튼);
            this.Controls.Add(this.작성자설정버튼);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "설정창";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "설정창";
            this.Load += new System.EventHandler(this.설정창_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 작성자설정버튼;
        private System.Windows.Forms.Button 줄표시변경버튼;
        private System.Windows.Forms.Button 저장버튼;
        private System.Windows.Forms.OpenFileDialog 사전파일열기;
        private System.Windows.Forms.ComboBox 인코딩설정;
        private System.Windows.Forms.TextBox 설명1;
    }
}