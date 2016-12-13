namespace 에라번역
{
    partial class 자동번역설정창
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
            this.id = new System.Windows.Forms.TextBox();
            this.secret = new System.Windows.Forms.TextBox();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // id
            // 
            this.id.Location = new System.Drawing.Point(13, 13);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(178, 21);
            this.id.TabIndex = 0;
            // 
            // secret
            // 
            this.secret.Location = new System.Drawing.Point(13, 41);
            this.secret.Name = "secret";
            this.secret.Size = new System.Drawing.Size(178, 21);
            this.secret.TabIndex = 1;
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(13, 69);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(75, 23);
            this.저장버튼.TabIndex = 2;
            this.저장버튼.Text = "저장";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // 자동번역설정창
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(203, 100);
            this.Controls.Add(this.저장버튼);
            this.Controls.Add(this.secret);
            this.Controls.Add(this.id);
            this.Name = "자동번역설정창";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "자동번역";
            this.Load += new System.EventHandler(this.자동번역설정창_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox id;
        private System.Windows.Forms.TextBox secret;
        private System.Windows.Forms.Button 저장버튼;
    }
}