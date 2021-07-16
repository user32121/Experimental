using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DistCompPiNode
{
    class Program
    {
        class Node
        {
            public Socket socket;
            public byte[] data;
            public int currentTask;

            public Node(Socket socket)
            {
                this.socket = socket;
                this.data = new byte[256];
                this.currentTask = -1;
            }
        }

        static void Main(string[] args)
        {
            //connect
            IPAddress ip = IPAddress.None;
            for (int i = 0; i < Dns.GetHostEntry(Dns.GetHostName()).AddressList.Length; i++)
            {
                ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList[i];
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    break;
            }
            Console.WriteLine("Connecting to " + ip);
            IPEndPoint serverEndPoint = new IPEndPoint(ip, 44422);  //note that this port number must be the same for server and node
            Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(serverEndPoint);
                Console.WriteLine("Connected. Awaiting work");
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Environment.Exit(1);
            }

            while (true)
            {
                //wait for command
                byte[] buf = new byte[256];
                int numBytes = socket.Receive(buf);
                if (numBytes != 1)
                {
                    Console.Write("Unknown response, press enter to continue...");
                    Console.ReadLine();
                }
                Console.Write("Received {0} seconds of work...", buf[0]);

                //process
                int timeMS = buf[0] * 1000;
                long success = 0;
                long total = 0;
                Random rnd = new Random();
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < timeMS)
                {
                    double x = rnd.NextDouble();
                    double y = rnd.NextDouble();
                    if (x * x + y * y <= 1)
                        success++;
                    total++;
                }
                Console.WriteLine("Finished processing");

                //return result
                buf = BitConverter.GetBytes(success).Concat(BitConverter.GetBytes(total)).ToArray();
                socket.Send(buf); 
            }
        }
    }
}
