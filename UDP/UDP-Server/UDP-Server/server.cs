using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;



namespace UDPServer
{
	class UDP_Server
	{
    
		int listenPort = 9000;
		public UDP_Server()
		{
			int recv;
			byte[] data = new byte[1024];
			IPEndPoint ipep = new IPEndPoint(IPAddress.Any, listenPort);

			Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			mySocket.Bind(ipep);
			Console.WriteLine("Waiting on a client...");
			IPEndPoint sender = new IPEndPoint(IPAddress.Any, 9000);
			EndPoint remote = (EndPoint)(sender);

			while (true)
			{
				data = new byte[1024];
				recv = mySocket.ReceiveFrom(data, ref remote);



				recv = mySocket.ReceiveFrom(data, ref remote);
				string recieved = Encoding.ASCII.GetString(data);
                
				if (string.Compare(recieved,"u") == 0 || string.Compare(recieved,"U") == 0){
					
                    byte[] toSend = new byte[1024];
					FileStream fs = new FileStream("/proc/uptime", FileMode.Open, FileAccess.Read);
                    int read = fs.Read(toSend, 0, toSend.Length);
					mySocket.SendTo(toSend, toSend.Length, SocketFlags.None, remote);

                    fs.Flush();
					fs.Close();
			}



				else if(string.Compare(recieved, "l") == 0 || string.Compare(recieved, "L") == 0)			
						{

							byte[] toSend = new byte[1024];
                             FileStream fs = new FileStream("/proc/loadavg", FileMode.Open, FileAccess.Read);
                            int read = fs.Read(toSend, 0, toSend.Length);

					mySocket.SendTo(toSend, toSend.Length, SocketFlags.None, remote);
	
					fs.Flush();
					fs.Close();
				
						}
				else{

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

    