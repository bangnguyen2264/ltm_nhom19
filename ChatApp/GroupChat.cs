using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System;


namespace ChatApp
{
    public partial class GroupChat : Form
    {

        public GroupChat()
        {
            InitializeComponent();
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
        }

        // Đóng kết nối khi form đóng
        private void GroupChat_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
