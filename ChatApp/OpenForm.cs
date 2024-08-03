using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class OpenForm : Form
    {
        public OpenForm()
        {
            InitializeComponent();
        }

        private void btnComeIn_Click(object sender, EventArgs e)
        {
            User.UserName = txtUsername.Text;
            User.PrivatePort = GenerateRandomPort();
            Trace.WriteLine("User with username: "+User.UserName);
            Trace.WriteLine("User with private port: "+User.PrivatePort);
            SelectChatRoom selectChatRoom = new SelectChatRoom();
            this.Hide();
            selectChatRoom.ShowDialog();
        }

        int GenerateRandomPort()
        {
            Random random = new Random();
            return random.Next(1000, 9999);
        }   
    }
}
