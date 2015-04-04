namespace OPCT
{
    partial class About
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
            this.versionLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.qqLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.closeAboutButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(104, 30);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(83, 12);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.Text = "Version:1.1.0";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.Location = new System.Drawing.Point(105, 52);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(65, 12);
            this.copyrightLabel.TabIndex = 1;
            this.copyrightLabel.Text = "Copyright?";
            // 
            // qqLabel
            // 
            this.qqLabel.AutoSize = true;
            this.qqLabel.Location = new System.Drawing.Point(105, 73);
            this.qqLabel.Name = "qqLabel";
            this.qqLabel.Size = new System.Drawing.Size(71, 12);
            this.qqLabel.TabIndex = 2;
            this.qqLabel.Text = "QQ:38656306";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "email:38656306@qq.com";
            // 
            // closeAboutButton
            // 
            this.closeAboutButton.Location = new System.Drawing.Point(304, 146);
            this.closeAboutButton.Name = "closeAboutButton";
            this.closeAboutButton.Size = new System.Drawing.Size(75, 23);
            this.closeAboutButton.TabIndex = 6;
            this.closeAboutButton.Text = "关闭";
            this.closeAboutButton.UseVisualStyleBackColor = true;
            this.closeAboutButton.Click += new System.EventHandler(this.closeAboutButton_Click);
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 181);
            this.Controls.Add(this.closeAboutButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.qqLabel);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.versionLabel);
            this.Name = "About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label copyrightLabel;
        private System.Windows.Forms.Label qqLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button closeAboutButton;
    }
}