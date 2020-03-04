using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Client
{

	class UDP_Client
	{
		//Hardcoded IP - could be an input argument.
		string ipAddress = "10.0.0.1";
        
		public UDP_Client(string[] args)
		{
        try
			{
				using (var client = new UdpClient())
				{
					//Creating and connecting to UDP endpoint
					IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 9000);
					client.Connect(endPoint);
                    //Sending input argument
					byte[] toSend = Encoding.ASCII.GetBytes(args[0]);
					client.Send(toSend, toSend.Length);
					//Receiving and printing data from endpoint (Server)
					byte[] recv = new byte[1024];
                    recv = client.Receive(ref endPoint);
					Console.WriteLine(Encoding.ASCII.GetString(recv));
					//Closing connecting.
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
				new UDP_Client(args);
			}
		}
	}
}
