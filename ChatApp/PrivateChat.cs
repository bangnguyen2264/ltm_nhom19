using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace ChatApp
{
    public partial class PrivateChat : Form
    {
        private TcpClient client;
        private TcpListener listener;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;
        private OpenFileDialog openFileDialog;
        Dictionary<int, string> fileMessages = new Dictionary<int, string>(); // Track file messages

        public PrivateChat()
        {
            InitializeComponent();
            yPort.Text = User.PrivatePort.ToString();
            openFileDialog = new OpenFileDialog();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                string message = User.UserName + ": " + ThayTheBangIcon(txtMessage.Text);
                SendMessage(message);
            }
            txtMessage.Text = "";
        }

        private string ThayTheBangIcon(string text)
        {
            foreach (var icon in Icons.IconMap)
            {
                text = text.Replace(icon.Key, icon.Value);
            }
            return text;
        }

        //Xử lý dữ liệu nhận được
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                try
                {
                    NetworkStream networkStream = client.GetStream();
                    BinaryReader reader = new BinaryReader(networkStream);

                    string dataType = reader.ReadString();

                    if (dataType == "message")
                    {
                        string message = reader.ReadString();
                        this.listTextMessages.Invoke(new MethodInvoker(delegate ()
                        {
                            listTextMessages.AppendText(message + "\n");
                        }));
                    }
                    else if (dataType == "file")
                    {
                        string fileName = reader.ReadString(); // Get file name with extension
                        int fileLength = reader.ReadInt32();
                        byte[] fileBytes = reader.ReadBytes(fileLength);

                        // Save the file with the correct name and extension
                        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);
                        File.WriteAllBytes(filePath, fileBytes);

                        string message = $"{User.UserName} sent file: {fileName}";
                        this.listTextMessages.Invoke(new MethodInvoker(delegate ()
                        {
                            int start = listTextMessages.TextLength;
                            listTextMessages.AppendText(message + "\n");
                            int end = listTextMessages.TextLength;

                            // Store the file path associated with the message
                            fileMessages.Add(start, filePath);

                            // Highlight the file message (optional)
                            listTextMessages.Select(start, end - start);
                            listTextMessages.SelectionColor = Color.Blue;
                            listTextMessages.SelectionFont = new Font(listTextMessages.Font, FontStyle.Underline);
                        }));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving data: " + ex.Message);
                    client.Close();
                }
            }
        }

        //Xử lý dữ liệu gửi đi
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SendMessage(TextToSend);
        }

        private void SendMessage(string message)
        {
            if (client != null && client.Connected)
            {
                try
                {
                    NetworkStream networkStream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(networkStream);

                    writer.Write("message");
                    writer.Write(message);

                    this.listTextMessages.Invoke(new MethodInvoker(delegate ()
                    {
                        listTextMessages.AppendText(message + "\n");
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending message: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Send failed!");
            }
        }

        private void SendFile(string filePath)
        {
            if (client != null && client.Connected)
            {
                try
                {
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string fileName = Path.GetFileName(filePath); // Get the file name with extension
                    NetworkStream networkStream = client.GetStream();
                    BinaryWriter writer = new BinaryWriter(networkStream);

                    // Send file info
                    writer.Write("file");
                    writer.Write(fileName); // Send file name with extension
                    writer.Write(fileBytes.Length);
                    writer.Write(fileBytes);

                    // Send a message about the file
                    TextToSend = $"{User.UserName} sent file: {fileName}";
                    SendMessage(TextToSend);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error sending file: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Cannot send file. Connection is closed.");
            }
        }

        private void listTextMessages_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listTextMessages.GetCharIndexFromPosition(e.Location);

            foreach (var kvp in fileMessages)
            {
                if (index >= kvp.Key && index < kvp.Key + kvp.Value.Length)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        FileName = Path.GetFileName(kvp.Value),
                        Filter = "All Files (*.*)|*.*"
                    };

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.Copy(kvp.Value, saveFileDialog.FileName, true);
                        MessageBox.Show("File downloaded successfully!", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                }
            }
        }

        void StartServer()
        {
            listener = new TcpListener(IPAddress.Any, int.Parse(yPort.Text));
            listener.Start();
            Task.Run(() =>
            {
                try
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            });
            Trace.WriteLine("Server started with port " + User.PrivatePort);
            btnStart.Text = "Stop";
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (btnConnect.Text == "Connect")
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
            if (yPort.Text == clPort.Text)
            {
                MessageBox.Show("Port number must be different!");
                return;
            }
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(clPort.Text));

            try
            {
                client.Connect(IP_End);
                TextToSend = User.UserName + " joined the chat";
                SendMessage(TextToSend);
                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            listTextMessages.AppendText("Connected to server\n");
            btnConnect.Text = "Disconnect";
        }

        void Disconnect()
        {
            TextToSend = User.UserName + " left the chat";
            SendMessage(TextToSend);
            client.Close();
            STR.Close();
            STW.Close();
            listTextMessages.Clear();
            btnConnect.Text = "Connect";
        }

        void StopServer()
        {
            listener.Stop();
            if (client != null)
            {
                TextToSend = User.UserName + " left the chat";
                SendMessage(TextToSend);
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
            btnStart.Text = "Start";
        }

        private void PrivateChat_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                SendFile(filePath);
            }
        }
    }

    public static class Icons
    {
        public static Dictionary<string, string> IconMap = new Dictionary<string, string>
        {
            { ":)", "😊" },
            { ":(", "😞" },
            { ":D", "😄" },
            { ";)", "😉" }
        };
    }
}
