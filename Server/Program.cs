using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
<<<<<<< HEAD

=======
>>>>>>> bang
class Server
{
    static Socket server;
    static List<Socket> clientList;
    static IPEndPoint ip;
    static object lockObj = new object();

    static void Main(string[] args)
    {
        clientList = new List<Socket>();
        // tạo 1 endpoint để kết nối tới server
        ip = new IPEndPoint(IPAddress.Any, 5000);
        // tạo 1 client socket để kết nối tới server
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            // tạo 1 luồng đợi client kết nối
            server.Bind(ip);

        }
        catch (SocketException e)
        {
            Console.WriteLine("Socket error: " + e.Message);
            return;
        }
        Console.WriteLine("Server started on " + ip);

        // tạo 1 luồng lắng nghe client kết nối
        Thread listen = new Thread(() =>
        {
            try
            {
                while (true)
                {
                    // Tạo hàng đợi chờ kết nối
                    server.Listen(100);
                    // khi có client kết nối thì thêm vào list
                    Socket client = server.Accept();
                    clientList.Add(client);
       
                    Console.WriteLine("Client connected from " + client.RemoteEndPoint);
                    // tạo 1 luồng để nhận dữ liệu từ client
                    Thread receive = new Thread(() => Receive(client));
                    receive.IsBackground = true;
                    receive.Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Server error: " + e.Message);
                // Khởi tạo lại server
                ip = new IPEndPoint(IPAddress.Any, 5000);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }
        });

        listen.IsBackground = true;
        // bắt đầu thread
        listen.Start();

        Console.ReadLine(); // Keep the console open
    }


    static void Receive(Socket client)
    {
        try
        {
            while (true)
            {
                byte[] data = new byte[1024 * 5000];
                client.Receive(data);
                foreach (var c in clientList)
                {
                    if (c != client)
                    {
                        c.Send(data);
                    }
                }
            }
        }
        catch
        {
            clientList.Remove(client);
            client.Close();
        }
    }

    static void Close()
    {
        foreach (var client in clientList)
        {
            try
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch { }
        }

        server.Close();
    }
    


}
