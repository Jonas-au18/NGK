using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;

namespace UDP_Client
{

	class UDP_Client
	{
		string ipAddress = "10.0.0.1";
        
		public UDP_Client(string[] args)
		{
        try
			{
				using (var client = new UdpClient())
				{
					IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 9000);
					client.Connect(endPoint);
					byte[] handshake = Encoding.ASCII.GetBytes("a");
					client.Send(handshake, handshake.Length);
					Console.WriteLine(args[0]);
					byte[] toSend = Encoding.ASCII.GetBytes(args[0]);
					client.Send(toSend, toSend.Length);
                    byte[] recv = new byte[1024];

                 
						recv = client.Receive(ref endPoint);
						Console.WriteLine(Encoding.ASCII.GetString(recv));
					
					Console.WriteLine("Closing");
					client.Close();
				}

			}
            catch(Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
 
 }


		class MainClass
		{
			public static void Main(string[] args)
			{
				Console.WriteLine(args[0]);

				new UDP_Client(args);
			}
		}
	}
}
