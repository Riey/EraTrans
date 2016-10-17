namespace 에라번역
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.파일열기버튼 = new System.Windows.Forms.Button();
            this.VersionText = new System.Windows.Forms.TextBox();
            this.EncodingText = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.폴더열기버튼 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // 파일열기버튼
            // 
            this.파일열기버튼.Location = new System.Drawing.Point(24, 24);
            this.파일열기버튼.Margin = new System.Windows.Forms.Padding(15, 12, 3, 4);
            this.파일열기버튼.Name = "파일열기버튼";
            this.파일열기버튼.Size = new System.Drawing.Size(82, 39);
            this.파일열기버튼.TabIndex = 0;
            this.파일열기버튼.Text = "파일열기";
            this.파일열기버튼.UseVisualStyleBackColor = true;
            this.파일열기버튼.Click += new System.EventHandler(this.파일열기버튼_Click);
            // 
            // VersionText
            // 
            this.VersionText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.VersionText.Cursor = System.Windows.Forms.Cursors.Default;
            this.VersionText.Location = new System.Drawing.Point(61, 208);
            this.VersionText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.VersionText.Name = "VersionText";
            this.VersionText.ReadOnly = true;
            this.VersionText.Size = new System.Drawing.Size(274, 16);
            this.VersionText.TabIndex = 1;
            this.VersionText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // EncodingText
            // 
            this.EncodingText.Location = new System.Drawing.Point(235, 177);
            this.EncodingText.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.EncodingText.Name = "EncodingText";
            this.EncodingText.Size = new System.Drawing.Size(100, 23);
            this.EncodingText.TabIndex = 5;
            this.EncodingText.Text = "UTF-8";
            this.EncodingText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "ERB파일을 선택해주세요";
            // 
            // 폴더열기버튼
            // 
            this.폴더열기버튼.Location = new System.Drawing.Point(113, 24);
            this.폴더열기버튼.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.폴더열기버튼.Name = "폴더열기버튼";
            this.폴더열기버튼.Size = new System.Drawing.Size(82, 39);
            this.폴더열기버튼.TabIndex = 6;
            this.폴더열기버튼.Text = "폴더열기";
            this.폴더열기버튼.UseVisualStyleBackColor = true;
            this.폴더열기버튼.Click += new System.EventHandler(this.폴더열기버튼_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 237);
            this.Controls.Add(this.폴더열기버튼);
            this.Controls.Add(this.EncodingText);
            this.Controls.Add(this.VersionText);
            this.Controls.Add(this.파일열기버튼);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "에라번역";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Main_Form_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 파일열기버튼;
        private System.Windows.Forms.TextBox VersionText;
        private System.Windows.Forms.TextBox EncodingText;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button 폴더열기버튼;
    }
}

