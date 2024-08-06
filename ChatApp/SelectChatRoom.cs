using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class SelectChatRoom : Form
    {
        public SelectChatRoom()
        {
            InitializeComponent();
        }

        private void btnPrChat_Click(object sender, EventArgs e)
        {
            PrivateChat privateChat = new PrivateChat();
            this.Hide();
            privateChat.ShowDialog();
        }

        private void btnGrChat_Click(object sender, EventArgs e)
        {
            GroupChat groupChat = new GroupChat();
            this.Hide();
            groupChat.ShowDialog();
        }
    }
}
