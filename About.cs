using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OPCT
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            copyrightLabel.Text = "Copyright:" + "2012-" + DateTime.Now.Year;
        }

        private void closeAboutButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
