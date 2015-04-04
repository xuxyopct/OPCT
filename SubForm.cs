using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;

namespace OPCT
{
    public partial class SubForm : Form
    {
        private string connStr = "";
        public SubForm()
        {
            InitializeComponent();
            this.genDataSouceList();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void genDataSouceList()
        {
            try
            {
                int i = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load("app.config");
                XmlNode root = doc.SelectSingleNode("//configuration");
                //XmlNode node = root.SelectSingleNode("//appSettings/add[@name='ConnectionString']");
                XmlNodeList xe = root.SelectNodes("//appSettings/add[@name='ConnectionString']");
                int count = xe.Count;
                IEnumerator ieNum = xe.GetEnumerator();
                while (ieNum.MoveNext())
                {
                    i++;
                    RadioButton rb = new RadioButton();
                    XmlElement el = ieNum.Current as XmlElement;
                    rb.Text = el.GetAttribute("connectionString");
                    rb.AutoSize = true;
                    rb.Location = new System.Drawing.Point(40, 5 + i * 25);
                    panel1.Controls.Add(rb);
                    //dataSourceGroupBox.Controls.Add(rb);
                    //dataSourceGroupBox.AutoSize = true;
                }
            }
            catch(XmlException ex)
            {
                MessageBox.Show(ex.Message);  
            }
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RadioButton rb in panel1.Controls)
                {
                    if (rb.Checked)
                    {
                        connStr = rb.Text;
                        break;
                    }
                }
                if (connStr.Length > 0)
                {
                    OPCT opct = new OPCT();
                    opct.ConnectionString = connStr;
                    opct.Show();
                }
                else
                {
                    MessageBox.Show("未选择数据源");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //public string subFormConnStr
        //{
        //    get
        //    {
        //        return connStr;
        //    }
        //}

        private void ExitXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank You!");
        }
    }
}
