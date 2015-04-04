namespace OPCT
{
    partial class UserObject
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
            this.uoTabControl = new System.Windows.Forms.TabControl();
            this.indexTableTabPage = new System.Windows.Forms.TabPage();
            this.uoDataGridView = new System.Windows.Forms.DataGridView();
            this.uoStatusStrip = new System.Windows.Forms.StatusStrip();
            this.namelabel = new System.Windows.Forms.Label();
            this.uoGroupBox = new System.Windows.Forms.GroupBox();
            this.typeLable = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.userObjectDsnTextBox = new System.Windows.Forms.TextBox();
            this.queryButton = new System.Windows.Forms.Button();
            this.tableRadioButton = new System.Windows.Forms.RadioButton();
            this.indexRadioButton = new System.Windows.Forms.RadioButton();
            this.uoUserTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.uoTabControl.SuspendLayout();
            this.indexTableTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uoDataGridView)).BeginInit();
            this.uoGroupBox.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uoTabControl
            // 
            this.uoTabControl.Controls.Add(this.indexTableTabPage);
            this.uoTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uoTabControl.Location = new System.Drawing.Point(0, 0);
            this.uoTabControl.Name = "uoTabControl";
            this.uoTabControl.SelectedIndex = 0;
            this.uoTabControl.Size = new System.Drawing.Size(861, 365);
            this.uoTabControl.TabIndex = 0;
            // 
            // indexTableTabPage
            // 
            this.indexTableTabPage.Controls.Add(this.uoDataGridView);
            this.indexTableTabPage.Location = new System.Drawing.Point(4, 22);
            this.indexTableTabPage.Name = "indexTableTabPage";
            this.indexTableTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.indexTableTabPage.Size = new System.Drawing.Size(853, 339);
            this.indexTableTabPage.TabIndex = 0;
            this.indexTableTabPage.Text = "索引或表";
            this.indexTableTabPage.UseVisualStyleBackColor = true;
            // 
            // uoDataGridView
            // 
            this.uoDataGridView.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.uoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uoDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uoDataGridView.Location = new System.Drawing.Point(3, 3);
            this.uoDataGridView.Name = "uoDataGridView";
            this.uoDataGridView.RowTemplate.Height = 23;
            this.uoDataGridView.Size = new System.Drawing.Size(847, 333);
            this.uoDataGridView.TabIndex = 0;
            // 
            // uoStatusStrip
            // 
            this.uoStatusStrip.Location = new System.Drawing.Point(0, 458);
            this.uoStatusStrip.Name = "uoStatusStrip";
            this.uoStatusStrip.Size = new System.Drawing.Size(861, 22);
            this.uoStatusStrip.TabIndex = 1;
            this.uoStatusStrip.Text = "statusStrip1";
            // 
            // namelabel
            // 
            this.namelabel.AutoSize = true;
            this.namelabel.Location = new System.Drawing.Point(227, 38);
            this.namelabel.Name = "namelabel";
            this.namelabel.Size = new System.Drawing.Size(53, 12);
            this.namelabel.TabIndex = 2;
            this.namelabel.Text = "用户名：";
            // 
            // uoGroupBox
            // 
            this.uoGroupBox.Controls.Add(this.typeLable);
            this.uoGroupBox.Controls.Add(this.typeComboBox);
            this.uoGroupBox.Controls.Add(this.userObjectDsnTextBox);
            this.uoGroupBox.Controls.Add(this.queryButton);
            this.uoGroupBox.Controls.Add(this.tableRadioButton);
            this.uoGroupBox.Controls.Add(this.indexRadioButton);
            this.uoGroupBox.Controls.Add(this.uoUserTextBox);
            this.uoGroupBox.Controls.Add(this.namelabel);
            this.uoGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uoGroupBox.Location = new System.Drawing.Point(0, 0);
            this.uoGroupBox.Name = "uoGroupBox";
            this.uoGroupBox.Size = new System.Drawing.Size(861, 89);
            this.uoGroupBox.TabIndex = 3;
            this.uoGroupBox.TabStop = false;
            // 
            // typeLable
            // 
            this.typeLable.AutoSize = true;
            this.typeLable.Location = new System.Drawing.Point(10, 38);
            this.typeLable.Name = "typeLable";
            this.typeLable.Size = new System.Drawing.Size(41, 12);
            this.typeLable.TabIndex = 8;
            this.typeLable.Text = "类型：";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "1.用户查询对象",
            "2.对象查所属表空间"});
            this.typeComboBox.Location = new System.Drawing.Point(57, 35);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(145, 20);
            this.typeComboBox.TabIndex = 7;
            this.typeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // userObjectDsnTextBox
            // 
            this.userObjectDsnTextBox.Location = new System.Drawing.Point(620, 34);
            this.userObjectDsnTextBox.Name = "userObjectDsnTextBox";
            this.userObjectDsnTextBox.Size = new System.Drawing.Size(83, 21);
            this.userObjectDsnTextBox.TabIndex = 4;
            this.userObjectDsnTextBox.Visible = false;
            // 
            // queryButton
            // 
            this.queryButton.Location = new System.Drawing.Point(733, 33);
            this.queryButton.Name = "queryButton";
            this.queryButton.Size = new System.Drawing.Size(75, 23);
            this.queryButton.TabIndex = 6;
            this.queryButton.Text = "查询";
            this.queryButton.UseVisualStyleBackColor = true;
            this.queryButton.Click += new System.EventHandler(this.queryButton_Click);
            // 
            // tableRadioButton
            // 
            this.tableRadioButton.AutoSize = true;
            this.tableRadioButton.Checked = true;
            this.tableRadioButton.Location = new System.Drawing.Point(499, 37);
            this.tableRadioButton.Name = "tableRadioButton";
            this.tableRadioButton.Size = new System.Drawing.Size(35, 16);
            this.tableRadioButton.TabIndex = 5;
            this.tableRadioButton.TabStop = true;
            this.tableRadioButton.Text = "表";
            this.tableRadioButton.UseVisualStyleBackColor = true;
            // 
            // indexRadioButton
            // 
            this.indexRadioButton.AutoSize = true;
            this.indexRadioButton.Location = new System.Drawing.Point(551, 37);
            this.indexRadioButton.Name = "indexRadioButton";
            this.indexRadioButton.Size = new System.Drawing.Size(47, 16);
            this.indexRadioButton.TabIndex = 4;
            this.indexRadioButton.TabStop = true;
            this.indexRadioButton.Text = "索引";
            this.indexRadioButton.UseVisualStyleBackColor = true;
            // 
            // uoUserTextBox
            // 
            this.uoUserTextBox.Location = new System.Drawing.Point(315, 35);
            this.uoUserTextBox.Name = "uoUserTextBox";
            this.uoUserTextBox.Size = new System.Drawing.Size(161, 21);
            this.uoUserTextBox.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.uoGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.uoTabControl);
            this.splitContainer1.Size = new System.Drawing.Size(861, 458);
            this.splitContainer1.SplitterDistance = 89;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 89);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // UserObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(861, 480);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.uoStatusStrip);
            this.Name = "UserObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserObjectQuery";
            this.Load += new System.EventHandler(this.UserObject_Load);
            this.uoTabControl.ResumeLayout(false);
            this.indexTableTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uoDataGridView)).EndInit();
            this.uoGroupBox.ResumeLayout(false);
            this.uoGroupBox.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl uoTabControl;
        private System.Windows.Forms.TabPage indexTableTabPage;
        private System.Windows.Forms.StatusStrip uoStatusStrip;
        private System.Windows.Forms.Label namelabel;
        private System.Windows.Forms.GroupBox uoGroupBox;
        private System.Windows.Forms.RadioButton tableRadioButton;
        private System.Windows.Forms.RadioButton indexRadioButton;
        private System.Windows.Forms.TextBox uoUserTextBox;
        private System.Windows.Forms.Button queryButton;
        private System.Windows.Forms.TextBox userObjectDsnTextBox;
        private System.Windows.Forms.DataGridView uoDataGridView;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Label typeLable;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Splitter splitter1;
    }
}