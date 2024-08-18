using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class PrivateChat : Form
    {
        private TcpClient client;
        private TcpListener listener;
        private StreamReader STR;
        private StreamWriter STW;
        private string receive;
        private string TextToSend;
        private CancellationTokenSource cancellationTokenSource;

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
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        listTextMessages.AppendText(receive + "\n");
                    }));
                    receive = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (client != null && client.Connected)
            {
                STW.WriteLine(TextToSend);
                this.Invoke(new MethodInvoker(delegate ()
                {
                    listTextMessages.AppendText(TextToSend + "\n");
                }));
            }
            else
            {
                MessageBox.Show("Send failed!");
                return;
            }
            backgroundWorker2.CancelAsync();
        }

        void StartServer()
        {
            cancellationTokenSource = new CancellationTokenSource();
            listener = new TcpListener(IPAddress.Any, int.Parse(yPort.Text));
            listener.Start();

            Task.Run(() =>
            {
                try
                {
                    while (!cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        client = listener.AcceptTcpClient();
                        STR = new StreamReader(client.GetStream());
                        STW = new StreamWriter(client.GetStream());
                        STW.AutoFlush = true;
                        backgroundWorker1.RunWorkerAsync();
                        backgroundWorker2.WorkerSupportsCancellation = true;
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            listTextMessages.AppendText("Client connected\n");
                        }));
                    }
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.Interrupted)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }, cancellationTokenSource.Token);

            Trace.WriteLine("Server started with port " + User.PrivatePort);
            listTextMessages.AppendText("Server started with port " + User.PrivatePort + "\n");
            btnStart.Text = "Stop";
        }

        void StopServer()
        {
            try
            {
                cancellationTokenSource.Cancel();

                if (listener != null)
                {
                    listener.Stop();
                    listener = null;
                }

                if (client != null && client.Connected)
                {
                    STR.Close();
                    STW.Close();
                    client.Close();
                }

                listTextMessages.Invoke(new MethodInvoker(delegate ()
                {
                    listTextMessages.AppendText("Server stopped\n");
                    listTextMessages.Clear();
                }));

                btnStart.Text = "Start";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                ConnectClient();
            }
            else
            {
                Disconnect();
            }
        }

        void ConnectClient()
        {
            try
            {
                client = new TcpClient();
                IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(clPort.Text));
                client.Connect(IP_End);
                TextToSend = User.UserName + " joined the chat";
                backgroundWorker2.RunWorkerAsync();
                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;

                listTextMessages.AppendText("Connected to server\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Disconnect()
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                }

                if (STR != null)
                {
                    STR.Close();
                }

                if (STW != null)
                {
                    STW.Close();
                }

                listTextMessages.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrivateChat_FormClosed(object sender, FormClosedEventArgs e)
        {
            StopServer();
            Disconnect();
            SelectChatRoom selectChatRoom = new SelectChatRoom();
            selectChatRoom.Show();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnStart.Text == "Stop")
            {
                StopServer();
            }
            else
            {
                StartServer();
            }
        }
    }
}
