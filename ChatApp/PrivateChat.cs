using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
//Thái push code backgroundWorker1_DoWork và backgroundWorker2_DoWork

namespace ChatApp
{
    public partial class PrivateChat : Form
    {
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;
        public bool isConnected = false;

        public PrivateChat()
        {
            InitializeComponent();
            yPort.Text = User.PrivatePort.ToString();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (txtMessage.Text != "")
            {
                TextToSend = User.UserName + ": " + txtMessage.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            txtMessage.Text = "";
        }


        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    receive = STR.ReadLine();
                    this.listTextMessages.Invoke(new MethodInvoker(delegate ()
                    {
                        listTextMessages.AppendText(receive + "\n");
                    }));
                    receive = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
           
        }

        
        private void PrivateChat_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
        }
    }
}
