namespace 에라번역
{
    partial class Select_Line
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
            this.선택버튼 = new System.Windows.Forms.Button();
            this.새로만들기버튼 = new System.Windows.Forms.Button();
            this.new_text = new System.Windows.Forms.TextBox();
            this.저장버튼 = new System.Windows.Forms.Button();
            this.crash_list = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // 선택버튼
            // 
            this.선택버튼.Location = new System.Drawing.Point(208, 13);
            this.선택버튼.Name = "선택버튼";
            this.선택버튼.Size = new System.Drawing.Size(75, 23);
            this.선택버튼.TabIndex = 1;
            this.선택버튼.Text = "선택";
            this.선택버튼.UseVisualStyleBackColor = true;
            this.선택버튼.Click += new System.EventHandler(this.선택버튼_Click);
            // 
            // 새로만들기버튼
            // 
            this.새로만들기버튼.Location = new System.Drawing.Point(208, 393);
            this.새로만들기버튼.Name = "새로만들기버튼";
            this.새로만들기버튼.Size = new System.Drawing.Size(75, 23);
            this.새로만들기버튼.TabIndex = 2;
            this.새로만들기버튼.Text = "새로만들기";
            this.새로만들기버튼.UseVisualStyleBackColor = true;
            this.새로만들기버튼.Click += new System.EventHandler(this.새로만들기버튼_Click);
            // 
            // new_text
            // 
            this.new_text.Location = new System.Drawing.Point(13, 393);
            this.new_text.Name = "new_text";
            this.new_text.Size = new System.Drawing.Size(189, 21);
            this.new_text.TabIndex = 3;
            // 
            // 저장버튼
            // 
            this.저장버튼.Location = new System.Drawing.Point(209, 363);
            this.저장버튼.Name = "저장버튼";
            this.저장버튼.Size = new System.Drawing.Size(75, 23);
            this.저장버튼.TabIndex = 5;
            this.저장버튼.Text = "저장";
            this.저장버튼.UseVisualStyleBackColor = true;
            this.저장버튼.Click += new System.EventHandler(this.저장버튼_Click);
            // 
            // crash_list
            // 
            this.crash_list.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.crash_list.FormattingEnabled = true;
            this.crash_list.Location = new System.Drawing.Point(13, 13);
            this.crash_list.Name = "crash_list";
            this.crash_list.Size = new System.Drawing.Size(189, 368);
            this.crash_list.TabIndex = 6;
            this.crash_list.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.crash_list_DrawItem);
            // 
            // Select_Line
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 426);
            this.Controls.Add(this.crash_list);
            this.Controls.Add(this.저장버튼);
            this.Controls.Add(this.new_text);
            this.Controls.Add(this.새로만들기버튼);
            this.Controls.Add(this.선택버튼);
            this.Name = "Select_Line";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "번역선택";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Select_Line_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button 선택버튼;
        private System.Windows.Forms.Button 새로만들기버튼;
        private System.Windows.Forms.TextBox new_text;
        private System.Windows.Forms.Button 저장버튼;
        private System.Windows.Forms.ListBox crash_list;
    }
}