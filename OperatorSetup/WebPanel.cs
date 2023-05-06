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
    public partial class WebPanel : Form
    {
        public string source = "";
        public WebPanel()
        {
            InitializeComponent();
        }

        private void WebPanel_Load(object sender, EventArgs e)
        {
            webView21.Source = new Uri(source);
        }
    }
}
