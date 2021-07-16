using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistCompPiMain
{
    class Program
    {
        class Node
        {
            public Socket socket;
            public byte[] data;
            public int currentTask;
            public int id;

            public Node(Socket socket, int id)
            {
                this.socket = socket;
                this.data = new byte[256];
                this.currentTask = -1;
                this.id = id;
            }
        }
        static int highestNodeID;

        static Queue<byte> toProcess = new Queue<byte>();
        static Queue<Node> idleNodes = new Queue<Node>();

        static AutoResetEvent waitForWork = new AutoResetEvent(false);


        static long totalSuccess = 0;
        static long totalTrials = 0;


        static void Main(string[] args)
        {
            //setup the endpoint that receives node connections
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 44422);  //note that this port number must be the same for server and node
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

            new Task(DistributeWork).Start();
            ProcessCLI();
        }

        private static void OnAccept(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            //accept connection
            Socket handler = server.EndAccept(ar);
            Console.WriteLine("{0} connected", handler.LocalEndPoint);
            idleNodes.Enqueue(new Node(handler, highestNodeID++));

            waitForWork.Set();

            //start accepting more connections
            server.BeginAccept(OnAccept, server);
        }

        private static void DistributeWork()
        {
            int taskID = 0;
            while (true)
            {
                while (toProcess.Count > 0 && idleNodes.Count > 0)
                {
                    byte work = toProcess.Peek();
                    Node node = idleNodes.Peek();
                    node.currentTask = taskID;

                    //make sure node is connected
                    try
                    {
                        node.socket.BeginSend(new byte[] { work }, 0, 1, SocketFlags.None, null, null);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine("Node {0} disconnected ({1}: {2})", node.id, e.SocketErrorCode, e.Message);
                        idleNodes.Dequeue();
                        continue;
                    }
                    node.socket.BeginReceive(node.data, 0, node.data.Length, SocketFlags.None, OnReceive, node);

                    Console.WriteLine("Distributed task {0} ({1} sec) to node {2}", taskID, work, node.id);
                    toProcess.Dequeue();
                    idleNodes.Dequeue();

                    taskID++;
                }
                waitForWork.WaitOne();
            }
        }

        private static void OnReceive(IAsyncResult ar)
        {
            Node node = (Node)ar.AsyncState;
            Console.WriteLine("Node {0} finished task {1}", node.id, node.currentTask);
            int numBytes = node.socket.EndReceive(ar);
            if (numBytes == 16)
            {
                long success, trials;
                totalSuccess += success = BitConverter.ToInt64(node.data, 0);
                totalTrials += trials = BitConverter.ToInt64(node.data, 8);
                double pi = (double)totalSuccess / totalTrials * 4;
                Console.WriteLine("{0}/{1}; total: {2}/{3} ({4})", success, trials, totalSuccess, totalTrials, pi);
            }
            else
                Console.WriteLine("Incorrect number of bytes returned ({0}); skipping", numBytes);
            node.currentTask = -1;
            idleNodes.Enqueue(node);  //add back to idle nodes queue
            waitForWork.Set();  //check if more data to process
        }

        private static void ProcessCLI()
        {
            Console.WriteLine("Ready to take inputs");
            while (true)
            {
                string input = Console.ReadLine();
                if (byte.TryParse(input, out byte num))
                    toProcess.Enqueue(num);
                else
                    Console.WriteLine("Can't parse");
                waitForWork.Set();
            }
        }
    }
}
