using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        class SocketStateObject
        {
            public Socket socket;
            public byte[] data;

            public SocketStateObject(Socket socket, byte[] data)
            {
                this.socket = socket;
                this.data = data;
            }
        }

        static void Main(string[] args)
        {
            //SynchronousDemo();
            AsynchronousDemo();
        }

        static void SynchronousDemo()
        {
            //set the server endpoint
            Console.WriteLine("Enter IP Address to connect to:");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(Console.ReadLine()), 32121);  //note that this port number must be the same for server and client
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);

            //connect
            Console.WriteLine("Connecting...");
            client.Connect(serverEndPoint);

            //send data
            Console.WriteLine("Enter text to send to server:");
            client.Send(Encoding.UTF8.GetBytes(Console.ReadLine()));
            Console.WriteLine("Sent");

            //disconnect
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }

        static void AsynchronousDemo()
        {
            //set the server endpoint
            Console.WriteLine("Enter IP Address to connect to:");
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(Console.ReadLine()), 32121);  //note that this port number must be the same for server and client
            Socket client = new Socket(SocketType.Stream, ProtocolType.Tcp);

            //connect
            Console.WriteLine("Connecting...");
            client.Connect(serverEndPoint);

            //receive data
            SocketStateObject stateObject = new SocketStateObject(client, new byte[100]);
            client.BeginReceive(stateObject.data, 0, stateObject.data.Length, SocketFlags.None, OnReceive, stateObject);

            //send data
            while (true)
            {
                Console.WriteLine("Enter text to send to server:");
                byte[] s = Encoding.UTF8.GetBytes(Console.ReadLine());
                client.BeginSend(s, 0, s.Length, SocketFlags.None, null, null);
                Console.WriteLine("Sent");
            }
        }

        static void OnReceive(IAsyncResult ar)
        {
            SocketStateObject stateObject = ar.AsyncState as SocketStateObject;

            //check if client disconnected
            if (!stateObject.socket.Connected)
            {
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                Console.WriteLine("Disconnected");
                Environment.Exit(0);
            }

            //receive and output the data
            int numBytes = 0;
            try
            {
                numBytes = stateObject.socket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                Console.WriteLine("socket failed: {0}: {1}", e.SocketErrorCode, e.Message);
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                Environment.Exit(e.ErrorCode);
            }
            //check if 0 bytes received (client's send channel is closed)
            if (numBytes == 0)
            {
                Console.WriteLine("Closed connection");
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                Environment.Exit(0);
            }
            Console.WriteLine("Server sent \"{0}\"", Encoding.UTF8.GetString(stateObject.data, 0, numBytes));

            //continue receiving data
            stateObject.socket.BeginReceive(stateObject.data, 0, stateObject.data.Length, SocketFlags.None, OnReceive, stateObject);
        }
    }
}
