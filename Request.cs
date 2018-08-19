using System;
using System.Windows.Forms;
using PKHeX.Core;

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
            MessageBox.Show($"Could not retrieve from {address}", "Feature is not implemented");
        }

        private void B_Convert_Click(object sender, EventArgs e)
        {
            if (TB_Shared.Text.Length > 0)
                TB_File.Text = ConvertCode(TB_Shared.Text);
        }

        private static string ConvertCode(string code)
        {
            return BVRequestUtil.StrToU64(code, out bool _).ToString("D11")+"-00001";
        }
    }
}
