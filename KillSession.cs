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
    public partial class KillSession : Form
    {
        private string connOStr = "";
        public KillSession()
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

        private void KillSession_Load(object sender, EventArgs e)
        {
            this.killSessionDsnTextBox.Text = connOStr;
        }

        private void killSessionButton_Click(object sender, EventArgs e)
        {
            string sidStr = "", sidCount="",msgError = "",msgSucc = "";
            int returnCount = 0;
            try
            {
                if (this.killSessionTextBox.Text == "")
                {
                    MessageBox.Show("请输入会话ID");
                }
                else
                {
                    OracleConnection conn = new OracleConnection(this.killSessionDsnTextBox.Text);
                    conn.Open();
                    foreach (string linestr in killSessionTextBox.Lines)
                    {
                        sidStr = linestr.Substring(0, linestr.IndexOf(","));
                        OracleCommand connCount = conn.CreateCommand();
                        connCount.CommandText = "select count(*) from v$session a where  a.sid = '" + sidStr + "'";
                        sidCount = Convert.ToString(connCount.ExecuteOracleScalar()).Trim();
                        if (sidCount == "" || sidCount == "0")
                        {
                            MessageBox.Show("没有找到用户会话或用户已退出");
                        }
                        else if (sidCount == "1")
                        {
                            OracleCommand killSql = conn.CreateCommand();
                            killSql.CommandText = "alter system kill session '" + this.killSessionTextBox.Text + "'";
                            returnCount = killSql.ExecuteNonQuery();
                            if (returnCount == 0)
                            {
                                msgSucc += linestr + " KILL用户会话成功" + "\r\n"; 
                                //MessageBox.Show("KILL用户会话成功");
                            }
                            else
                            {
                                msgError += linestr + " KILL用户会话失败" + "\r\n"; 
                                //MessageBox.Show("KILL用户会话失败");
                            }
                        }
                    }
                    conn.Close();
                    if (msgSucc.Length > 0)
                    {
                        MessageBox.Show(msgSucc); 
                    }
                    if (msgError.Length > 0)
                    {
                        MessageBox.Show(msgError); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
