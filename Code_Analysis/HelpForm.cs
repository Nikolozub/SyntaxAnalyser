using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Code_Analysis
{
    public partial class HelpForm : Form
    {
        string helpFile;
        public HelpForm(string helpFile)
        {
            InitializeComponent();
            this.helpFile = helpFile;
        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            this.webBrowser1.Url = new Uri(this.helpFile);
        }
    }
}
