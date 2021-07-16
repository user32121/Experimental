using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
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

        static List<Socket> clientHandlers = new List<Socket>();

        static void Main(string[] args)
        {
            //SynchronousDemo();
            AsynchronousDemo();
        }

        static void SynchronousDemo()
        {
            //setup the endpoint that receives client connections
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 32121);  //note that this port number must be the same for server and client
            Socket server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(serverEndPoint);

            //display server endpoints that can be connected to
            Console.WriteLine("Available ip addresses:");
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    Console.WriteLine(ip);

            //start listening for connections
            Console.WriteLine("Listening for connections...");
            server.Listen(100);

            while (true)
            {
                Socket client = server.Accept();
                Console.WriteLine("{0} connected", client.LocalEndPoint);
                byte[] data = new byte[100];
                int numBytes = client.Receive(data);
                Console.WriteLine("{0} sent \"{1}\"", client.LocalEndPoint, Encoding.UTF8.GetString(data, 0, numBytes));
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        static void AsynchronousDemo()
        {
            //setup the endpoint that receives client connections
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 32121);  //note that this port number must be the same for server and client
            Socket server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Bind(serverEndPoint);

            //display server endpoints that can be connected to
            Console.WriteLine("Available ip addresses:");
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    Console.WriteLine(ip);

            //start listening for connections
            Console.WriteLine("Listening for connections...");
            server.Listen(100);

            server.BeginAccept(OnAccept, server);

            while (true)
            {
                byte[] s = Encoding.UTF8.GetBytes(Console.ReadLine());
                foreach (Socket handler in clientHandlers)
                    handler.BeginSend(s, 0, s.Length, SocketFlags.None, null, null);
            }
        }

        private static void OnAccept(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            //accept connection
            Socket clientHandler = server.EndAccept(ar);
            Console.WriteLine("{0} connected", clientHandler.LocalEndPoint);
            clientHandlers.Add(clientHandler);

            //create object to store handler info
            SocketStateObject stateObject = new SocketStateObject(clientHandler, new byte[100]);
            //start receiving data
            clientHandler.BeginReceive(stateObject.data, 0, stateObject.data.Length, SocketFlags.None, OnReceive, stateObject);

            //start accepting more connections
            server.BeginAccept(OnAccept, server);
        }

        private static void OnReceive(IAsyncResult ar)
        {
            SocketStateObject stateObject = ar.AsyncState as SocketStateObject;

            //check if client closed connection
            if (!stateObject.socket.Connected)
            {
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                Console.WriteLine("{0} disconnected", stateObject.socket.LocalEndPoint);
                clientHandlers.Remove(stateObject.socket);
                return;
            }

            //receive and output the data
            int numBytes;
            try
            {
                numBytes = stateObject.socket.EndReceive(ar);
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0} socket failed: {1}: {2}", stateObject.socket.LocalEndPoint, e.SocketErrorCode, e.Message);
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                clientHandlers.Remove(stateObject.socket);
                return;
            }
            //check if 0 bytes received (client's send channel is closed)
            if (numBytes == 0)
            {
                Console.WriteLine("{0} closed connection", stateObject.socket.LocalEndPoint);
                stateObject.socket.Shutdown(SocketShutdown.Both);
                stateObject.socket.Close();
                clientHandlers.Remove(stateObject.socket);
                return;
            }
            Console.WriteLine("{0} sent \"{1}\"", stateObject.socket.LocalEndPoint, Encoding.UTF8.GetString(stateObject.data, 0, numBytes));

            //continue receiving data
            stateObject.socket.BeginReceive(stateObject.data, 0, stateObject.data.Length, SocketFlags.None, OnReceive, stateObject);
        }
    }
}
