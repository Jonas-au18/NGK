using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPServer
{
	class UDP_Server
	{
    
		int listenPort = 9000;
		public UDP_Server()
		{
			int recv;
			byte[] data = new byte[1024];
            //Setting up listener and binding it.
			IPEndPoint ipep = new IPEndPoint(IPAddress.Any, listenPort);
            Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			mySocket.Bind(ipep);
			Console.WriteLine("Waiting on a client...");
            //Creating endpoint sender when connection established.
			IPEndPoint sender = new IPEndPoint(IPAddress.Any, 9000);
			EndPoint remote = (EndPoint)(sender);

			while (true)
			{
				data = new byte[1024];
                //Reading input from client
				recv = mySocket.ReceiveFrom(data, ref remote);
				string recieved = Encoding.ASCII.GetString(data);
				Console.WriteLine("Got input {0}", recieved);
                //Checks if uptime request
				if (string.Compare(recieved, "u") == 0 || string.Compare(recieved, "U") == 0)
				{
                    //Sending current uptime.
					byte[] toSend = new byte[1024];
					FileStream fs = new FileStream("/proc/uptime", FileMode.Open, FileAccess.Read);
					int read = fs.Read(toSend, 0, toSend.Length);
					mySocket.SendTo(toSend, toSend.Length, SocketFlags.None, remote);
					Console.WriteLine("Info send: {0}", Encoding.ASCII.GetString(toSend));
					fs.Flush();
					fs.Close();
				}


                //Check if load requested
				else if (string.Compare(recieved, "l") == 0 || string.Compare(recieved, "L") == 0)
				{
                    //Sending load
					byte[] toSend = new byte[1024];
					FileStream fs = new FileStream("/proc/loadavg", FileMode.Open, FileAccess.Read);
					int read = fs.Read(toSend, 0, toSend.Length);

					mySocket.SendTo(toSend, toSend.Length, SocketFlags.None, remote);
					Console.WriteLine("Info send: {0}", Encoding.ASCII.GetString(toSend));

					fs.Flush();
					fs.Close();

				}
                //Any other combination provokes an error.
				else
				{
					byte[] toSend = new byte[1024];
					toSend = Encoding.ASCII.GetBytes("Wrong input, only takes U/u and L/l");
					mySocket.SendTo(toSend, toSend.Length, SocketFlags.None,remote);
				}

			}
        
	    }

    }



       class MainClass
		{
			public static void Main(string[] args)
			{
				Console.WriteLine("I haz become sentient");
				new UDP_Server();
			}
		}
	}

    