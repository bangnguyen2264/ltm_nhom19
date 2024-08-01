using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;

namespace ChatApp
{
    public partial class PrivateChat : Form
    {
        private TcpClient client;
        private TcpListener listener;
        private NetworkStream stream;
        private Thread listenerThread;


        public PrivateChat()
        {
            InitializeComponent();
            ConnectToServer();
        }

        void ConnectToServer()
        {
            ip = new IPEndPoint(IPAddress.Any, 5000);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Enable address reuse
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            try
            {
                server.Bind(ip);
            }
            catch (SocketException ex)
            {
                Trace.WriteLine($"Error binding socket: {ex.Message}");
                MessageBox.Show($"Error binding socket: {ex.Message}", "Socket Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(client);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Server error: {ex.Message}", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ResetServer();
                }
            })
            {
                IsBackground = true
            };
            listen.Start();
        }

        void ResetServer()
        {
            ip = new IPEndPoint(IPAddress.Any, 5000);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        }

        void ConnectToClient()
        {
            ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(ipConnect.Text));
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(ip);
                Thread receive = new Thread(Receive)
                {
                    IsBackground = true
                };
                receive.Start(client);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to connect to client: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Close()
        {
            server?.Close();
            client?.Close();
        }

        void Send()
        {
            if (!string.IsNullOrEmpty(txtMessage.Text))
            {
                client.Send(Serialize(txtMessage.Text));
            }
        }

        void Receive(object obj)
        {
            Socket client = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                    AddMessage(message);
                }
            }
            catch
            {
                client?.Close();
            }
        }

        void AddMessage(string message)
        {
            listTextMessages.Invoke(new Action(() => listTextMessages.Text += message + Environment.NewLine));
        }

        byte[] Serialize(object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        object Deserialize(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }

        private void PrivateChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToClient();
        }


        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
