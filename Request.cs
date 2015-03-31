using System;
using System.Windows.Forms;

namespace pkBV
{
    public partial class Request : Form
    {
        public Request()
        {
            InitializeComponent();
        }

        private void B_Request_Click(object sender, EventArgs e)
        {
            string address = TB_Web.Text + TB_File;
            Util.Alert(String.Format("Could not retrieve from {0}", address), "Feature is not implemented");
        }
        private void B_Convert_Click(object sender, EventArgs e)
        {
            if (TB_Shared.Text.Length > 0)
                TB_File.Text = convertCode(TB_Shared.Text);
        }
        private string convertCode(string BVCode)
        {
            return "00010087124-00001";
        }
    }
}
