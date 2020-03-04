using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace tcp
{
	class file_server
	{
		static Timer delay;
		const int PORT = 9000;
		const int BUFSIZE = 1000;
        
		private file_server ()
		{
			TcpListener server = null;
			try
			{
				server = new TcpListener(PORT);
                server.Start();
                TcpClient client = null;

				byte[] from = new byte[BUFSIZE];
				string fileReq = null;

				while (true)
				{
					
					Console.WriteLine("Waiting for connection");

					client = server.AcceptTcpClient();
					Console.WriteLine("Someone connected");
                   //resetting string for new request.
					fileReq = null;

					NetworkStream nStream = client.GetStream();
                    
                    //File request from client
					fileReq = LIB.readTextTCP(nStream);
					Console.WriteLine("got: {0}", fileReq);
                    
					/*Getting filename and then
                    Checking if file exist on system, els returns 0.*/
					string file = LIB.extractFileName(fileReq);
					long file_len = LIB.check_File_Exists(file);

					sendLength(file_len, nStream);
					if (file_len > 0)
					{
						//Delay to ensure nStream is ready.
						System.Threading.Thread.Sleep(10);
						sendFile(file, file_len, nStream);
					}
                    
					client.Close();
				}
			}
            catch(Exception e)
			{
				Console.WriteLine("Socket expection: {0}", e);
			}
            finally
			{
				server.Stop();
			}
            
        }
			
        
        void sendLength(long fileSize, NetworkStream io)
		{
            byte[] lenBuf = new byte[BUFSIZE];
            lenBuf = Encoding.ASCII.GetBytes(fileSize.ToString());
			io.Write(lenBuf, 0, lenBuf.Length);
		}


		private void sendFile (String fileName, long fileSize, NetworkStream io)
		{
			FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            int bytesRead = 0;
			int position = 0;
			int toRead = BUFSIZE;
			byte[] sendBuf = null;
            
			fs.Position = position;     
			while (true)
			{   //Done sending - exiting loop.
				if (fs.Position >= fileSize)
				{
					Console.WriteLine("Done sending");
					break;
                }
                         
				sendBuf = new byte[BUFSIZE];
				position += toRead;
                //Writing while chunk isn't full.
				while (toRead > 0 && (bytesRead = fs.Read(sendBuf, 0, toRead)) > 0)
				{
					io.Write(sendBuf, 0, bytesRead);
				}
				toRead = BUFSIZE;
			}
            //Closing filestream.
            fs.Close();
        }
        
		public static void Main (string[] args)
		{
			Console.WriteLine ("Server starts...");
			new file_server();
		}
	}
}
