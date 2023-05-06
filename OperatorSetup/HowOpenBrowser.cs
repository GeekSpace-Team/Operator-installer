using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperatorSetup
{
    public partial class HowOpenBrowser : Form
    {
        public string address = "";
        public HowOpenBrowser()
        {
            InitializeComponent();
        }

        private void HowOpenBrowser_Load(object sender, EventArgs e)
        {
            ipAddress.Text = address;
        }
    }
}
