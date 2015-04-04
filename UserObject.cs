using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace OPCT
{
    public partial class UserObject : Form
    {
        private string connOStr = "";
        public UserObject()
        {
            InitializeComponent();
        }

        public string ConnectionString
        {
            set
            {
                connOStr = value;
            }
            get
            {
                return connOStr;
            }
        }

        private void UserObject_Load(object sender, EventArgs e)
        {
            this.userObjectDsnTextBox.Text = connOStr;
            this.typeComboBox.SelectedIndex = 0;
        }

        private void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                string viewTx = "";
                if (this.uoUserTextBox.Text == "" || this.typeComboBox.Text == "")
                {
                    MessageBox.Show("请输入用户名或表名、索引名");
                }
                else
                {
                    OracleConnection conn = new OracleConnection(this.userObjectDsnTextBox.Text);
                    conn.Open();
                    if (this.tableRadioButton.Checked)
                    {
                        if (typeComboBox.Text.Substring(0, 1) == "1")
                            viewTx = "select t.* from all_tables t where t.owner = NLS_UPPER('" + uoUserTextBox.Text.Trim() + "')";
                        if (typeComboBox.Text.Substring(0, 1) == "2")
                            viewTx = "select t.* from all_tables t where t.table_name = NLS_UPPER('" + uoUserTextBox.Text.Trim() + "')";
                        DataSet ds = new DataSet();
                        OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                        da.Fill(ds, "viewTx");
                        uoDataGridView.AutoGenerateColumns = true;
                        uoDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                        uoDataGridView.ReadOnly = true;
                        uoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        uoDataGridView.ColumnHeadersVisible = true;
                        uoDataGridView.DataSource = ds;
                        uoDataGridView.DataMember = "viewTx";
                    }
                    if (this.indexRadioButton.Checked)
                    {
                        if (typeComboBox.Text.Substring(0, 1) == "1")
                            viewTx = "select t.* from all_indexes t where t.owner = NLS_UPPER('" + uoUserTextBox.Text.Trim() + "')";
                        if (typeComboBox.Text.Substring(0, 1) == "2")
                            viewTx = "select t.* from all_indexes t where t.index_name = NLS_UPPER('" + uoUserTextBox.Text.Trim() + "')";
                        DataSet ds = new DataSet();
                        OracleDataAdapter da = new OracleDataAdapter(viewTx, conn);
                        da.Fill(ds, "viewTx");
                        uoDataGridView.AutoGenerateColumns = true;
                        uoDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Raised;
                        uoDataGridView.ReadOnly = true;
                        uoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        uoDataGridView.ColumnHeadersVisible = true;
                        uoDataGridView.DataSource = ds;
                        uoDataGridView.DataMember = "viewTx";
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                  MessageBox.Show(ex.Message);
            }
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (typeComboBox.Text.Substring(0, 1) == "1")
                {
                    this.tableRadioButton.Enabled = true;
                    this.tableRadioButton.Checked = true;
                    this.indexRadioButton.Enabled = true;
                    this.indexRadioButton.Checked = false;
                    this.namelabel.Text = "用户名：";
                }
                else if(typeComboBox.Text.Substring(0, 1) == "2")
                {
                    //this.tableRadioButton.Enabled = false;
                    //this.tableRadioButton.Checked = false;
                    //this.indexRadioButton.Enabled = false;
                    //this.indexRadioButton.Checked = false;
                    this.namelabel.Text = "表名或索引名：";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
