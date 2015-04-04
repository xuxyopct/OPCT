namespace OPCT
{
    partial class KillSession
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
            this.killSessionGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.killSessionDsnTextBox = new System.Windows.Forms.TextBox();
            this.killSessionButton = new System.Windows.Forms.Button();
            this.killSessionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.killSessionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // killSessionGroupBox
            // 
            this.killSessionGroupBox.Controls.Add(this.label2);
            this.killSessionGroupBox.Controls.Add(this.killSessionDsnTextBox);
            this.killSessionGroupBox.Controls.Add(this.killSessionButton);
            this.killSessionGroupBox.Controls.Add(this.killSessionTextBox);
            this.killSessionGroupBox.Controls.Add(this.label1);
            this.killSessionGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.killSessionGroupBox.Location = new System.Drawing.Point(0, 0);
            this.killSessionGroupBox.Name = "killSessionGroupBox";
            this.killSessionGroupBox.Size = new System.Drawing.Size(550, 282);
            this.killSessionGroupBox.TabIndex = 0;
            this.killSessionGroupBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "多行批量";
            // 
            // killSessionDsnTextBox
            // 
            this.killSessionDsnTextBox.Location = new System.Drawing.Point(25, 16);
            this.killSessionDsnTextBox.Name = "killSessionDsnTextBox";
            this.killSessionDsnTextBox.Size = new System.Drawing.Size(354, 21);
            this.killSessionDsnTextBox.TabIndex = 3;
            this.killSessionDsnTextBox.Visible = false;
            // 
            // killSessionButton
            // 
            this.killSessionButton.Location = new System.Drawing.Point(414, 196);
            this.killSessionButton.Name = "killSessionButton";
            this.killSessionButton.Size = new System.Drawing.Size(75, 23);
            this.killSessionButton.TabIndex = 2;
            this.killSessionButton.Text = "确定";
            this.killSessionButton.UseVisualStyleBackColor = true;
            this.killSessionButton.Click += new System.EventHandler(this.killSessionButton_Click);
            // 
            // killSessionTextBox
            // 
            this.killSessionTextBox.Location = new System.Drawing.Point(214, 28);
            this.killSessionTextBox.Multiline = true;
            this.killSessionTextBox.Name = "killSessionTextBox";
            this.killSessionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.killSessionTextBox.Size = new System.Drawing.Size(165, 193);
            this.killSessionTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "会话ID(输入格式：SID,SERIR#)：";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 260);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(550, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // KillSession
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 282);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.killSessionGroupBox);
            this.MaximizeBox = false;
            this.Name = "KillSession";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KillUserSession";
            this.Load += new System.EventHandler(this.KillSession_Load);
            this.killSessionGroupBox.ResumeLayout(false);
            this.killSessionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox killSessionGroupBox;
        private System.Windows.Forms.Button killSessionButton;
        private System.Windows.Forms.TextBox killSessionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox killSessionDsnTextBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Label label2;
    }
}