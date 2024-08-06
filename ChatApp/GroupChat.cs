using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
using System;

//Tuấn push code connect và disconnect

namespace ChatApp
{
    public partial class GroupChat : Form
    {
        IPEndPoint ip;
        Socket client;
        Thread listen;

        public GroupChat()
        {
            InitializeComponent();
            CheckBox.CheckForIllegalCrossThreadCalls = false;
            Connect();
        }

        // Kết nối client tới server
        public void Connect()
        {
            // tạo 1 endpoint để kết nối tới server
            ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            // tạo 1 client socket để kết nối tới server
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // kết nối tới server
                client.Connect(ip);
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error: " + e);
                MessageBox.Show("Không thể kết nối tới server", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // tạo 1 luồng để lắng nghe dữ liệu từ server
            listen = new Thread(Receive);
            // đặt luồng đó là luồng nền
            listen.IsBackground = true;
            // bắt đầu luồng
            listen.Start();
            client.Send(Serialize(User.UserName + " đã tham gia phòng chat."));
        }
        //feat: add connection to server
        // đóng kết nối hiện thời
        void CloseConnection()
        {
            try
            {
                if (client != null && client.Connected)
                {
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Error closing connection: " + e);
            }

            if (listen != null && listen.IsAlive)
            {
                listen.Abort();
            }
        }

        void Send()
        {
            if (client.Connected && !string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                string message = User.UserName + ": " + txtMessage.Text;
                try
                {
                    // gửi tin nhắn tới client
                    client.Send(Serialize(message));
                    AddMessage(message);
                }
                catch (ObjectDisposedException)
                {
                    MessageBox.Show("Connection has been closed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Lỗi gửi tin nhắn: " + e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    // nhận dữ liệu từ server
                    int bytesReceived = client.Receive(data);
                    if (bytesReceived > 0)
                    {
                        string message = (string)Deserialize(data.Take(bytesReceived).ToArray());
                        AddMessage(message);
                    }
                }
            }
            catch (SocketException e)
            {
                Trace.WriteLine("Socket error: " + e);
                MessageBox.Show("Mất kết nối tới server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseConnection();
            }
            catch (Exception e)
            {
                Trace.WriteLine("Receive error: " + e);
                CloseConnection();
            }
        }

        // thêm tin nhắn vào listview
        void AddMessage(string message)
        {
            listTextMessages.Text += message + "\n";
            txtMessage.Clear();
        }

        byte[] Serialize(object obj)
        {
            // chuyển dữ liệu từ kiểu dữ liệu của mình sang kiểu byte
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // chuyển dữ liệu từ kiểu dữ liệu của mình sang kiểu byte
                formatter.Serialize(stream, obj);
                // trả về mảng byte
                return stream.ToArray();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        object Deserialize(byte[] data)
        {
            // chuyển dữ liệu từ kiểu byte sang kiểu dữ liệu của mình
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                // chuyển dữ liệu từ kiểu byte sang kiểu dữ liệu của mình
                return formatter.Deserialize(stream);
            }
        }

        // Đóng kết nối khi form đóng
        private void GroupChat_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseConnection();
        }
    }
}
