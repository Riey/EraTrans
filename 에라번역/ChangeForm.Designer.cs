namespace YeongHun.EraTrans
{
    partial class ChangeForm
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
            this.Original_Text = new System.Windows.Forms.TextBox();
            this.Translated_Text = new System.Windows.Forms.TextBox();
            this.현재줄 = new System.Windows.Forms.TextBox();
            this.자동번역버튼 = new System.Windows.Forms.Button();
            this.종료버튼 = new System.Windows.Forms.Button();
            this.복사버튼 = new System.Windows.Forms.Button();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Original_Text
            // 
            this.Original_Text.Cursor = System.Windows.Forms.Cursors.Default;
            this.Original_Text.Location = new System.Drawing.Point(13, 16);
            this.Original_Text.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Original_Text.Name = "Original_Text";
            this.Original_Text.ReadOnly = true;
            this.Original_Text.Size = new System.Drawing.Size(654, 23);
            this.Original_Text.TabIndex = 1;
            // 
            // Translated_Text
            // 
            this.Translated_Text.Location = new System.Drawing.Point(12, 174);
            this.Translated_Text.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Translated_Text.Name = "Translated_Text";
            this.Translated_Text.Size = new System.Drawing.Size(654, 23);
            this.Translated_Text.TabIndex = 0;
            this.Translated_Text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Translated_Text_KeyDown);
            // 
            // 현재줄
            // 
            this.현재줄.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.현재줄.Cursor = System.Windows.Forms.Cursors.Default;
            this.현재줄.Font = new System.Drawing.Font("굴림", 12F);
            this.현재줄.Location = new System.Drawing.Point(13, 99);
            this.현재줄.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.현재줄.Multiline = true;
            this.현재줄.Name = "현재줄";
            this.현재줄.ReadOnly = true;
            this.현재줄.Size = new System.Drawing.Size(287, 67);
            this.현재줄.TabIndex = 3;
            // 
            // 자동번역버튼
            // 
            this.자동번역버튼.Location = new System.Drawing.Point(13, 47);
            this.자동번역버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.자동번역버튼.Name = "자동번역버튼";
            this.자동번역버튼.Size = new System.Drawing.Size(89, 44);
            this.자동번역버튼.TabIndex = 6;
            this.자동번역버튼.Text = "자동번역";
            this.자동번역버튼.UseVisualStyleBackColor = true;
            this.자동번역버튼.Click += new System.EventHandler(this.자동번역버튼_Click);
            // 
            // 종료버튼
            // 
            this.종료버튼.Location = new System.Drawing.Point(396, 47);
            this.종료버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.종료버튼.Name = "종료버튼";
            this.종료버튼.Size = new System.Drawing.Size(89, 44);
            this.종료버튼.TabIndex = 7;
            this.종료버튼.Text = "종료";
            this.종료버튼.UseVisualStyleBackColor = true;
            this.종료버튼.Click += new System.EventHandler(this.종료버튼_Click);
            // 
            // 복사버튼
            // 
            this.복사버튼.Location = new System.Drawing.Point(108, 47);
            this.복사버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.복사버튼.Name = "복사버튼";
            this.복사버튼.Size = new System.Drawing.Size(89, 44);
            this.복사버튼.TabIndex = 8;
            this.복사버튼.Text = "복사";
            this.복사버튼.UseVisualStyleBackColor = true;
            this.복사버튼.Click += new System.EventHandler(this.복사버튼_Click);
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(203, 47);
            this.저장버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(89, 44);
            this.저장버튼.TabIndex = 9;
            this.저장버튼.Text = "저장";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // Change_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 215);
            this.Controls.Add(this.저장버튼);
            this.Controls.Add(this.복사버튼);
            this.Controls.Add(this.종료버튼);
            this.Controls.Add(this.자동번역버튼);
            this.Controls.Add(this.현재줄);
            this.Controls.Add(this.Translated_Text);
            this.Controls.Add(this.Original_Text);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Change_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "번역창";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Change_Form_FormClosing);
            this.Load += new System.EventHandler(this.Change_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Original_Text;
        private System.Windows.Forms.TextBox Translated_Text;
        private System.Windows.Forms.TextBox 현재줄;
        private System.Windows.Forms.Button 자동번역버튼;
        private System.Windows.Forms.Button 종료버튼;
        private System.Windows.Forms.Button 복사버튼;
        private System.Windows.Forms.Button 저장버튼;
    }
}