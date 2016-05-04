namespace 에라번역
{
    partial class LineSetting_Change_Form
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
            this.Setting_Text = new System.Windows.Forms.TextBox();
            this.적용버튼 = new System.Windows.Forms.Button();
            this.설명 = new System.Windows.Forms.TextBox();
            this.str_text = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Setting_Text
            // 
            this.Setting_Text.Location = new System.Drawing.Point(12, 12);
            this.Setting_Text.Name = "Setting_Text";
            this.Setting_Text.Size = new System.Drawing.Size(260, 21);
            this.Setting_Text.TabIndex = 0;
            // 
            // 적용버튼
            // 
            this.적용버튼.Location = new System.Drawing.Point(195, 156);
            this.적용버튼.Name = "적용버튼";
            this.적용버튼.Size = new System.Drawing.Size(78, 35);
            this.적용버튼.TabIndex = 4;
            this.적용버튼.Text = "적용";
            this.적용버튼.UseVisualStyleBackColor = true;
            this.적용버튼.Click += new System.EventHandler(this.적용버튼_Click);
            // 
            // 설명
            // 
            this.설명.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.설명.Location = new System.Drawing.Point(13, 39);
            this.설명.Multiline = true;
            this.설명.Name = "설명";
            this.설명.ReadOnly = true;
            this.설명.Size = new System.Drawing.Size(259, 56);
            this.설명.TabIndex = 5;
            this.설명.Text = "LINENUM:줄번호 LINETEXT:줄내용\r\nstr(등록한 문자가 차례로 나옵니다)\r\n각글자사이엔 + 를 붙여주세요\r\n밑에 있는 텍스트박스는 s" +
    "tr, str, str 식으로 입력";
            // 
            // str_text
            // 
            this.str_text.Location = new System.Drawing.Point(13, 129);
            this.str_text.Name = "str_text";
            this.str_text.Size = new System.Drawing.Size(260, 21);
            this.str_text.TabIndex = 6;
            // 
            // LineSetting_Change_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 203);
            this.Controls.Add(this.str_text);
            this.Controls.Add(this.설명);
            this.Controls.Add(this.적용버튼);
            this.Controls.Add(this.Setting_Text);
            this.Name = "LineSetting_Change_Form";
            this.Text = "표시방식변경";
            this.Load += new System.EventHandler(this.LineSetting_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Setting_Text;
        private System.Windows.Forms.Button 적용버튼;
        private System.Windows.Forms.TextBox 설명;
        private System.Windows.Forms.TextBox str_text;
    }
}