using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
	class file_client
	{

		const int PORT = 9000;

		const int BUFSIZE = 1000;
        
		private file_client (string[] args)
		{
			TcpClient client = null;
			NetworkStream nStream = null;

			try
			{
				//Creating connection to server.
				client = new TcpClient(args[0], PORT);
				Console.WriteLine("Connected to server");
                
				nStream = client.GetStream();

                //Sending file request.
				LIB.writeTextTCP(nStream, args[1]);

				Console.WriteLine("Getting file");

                //Getting filesize.
				long fileSize = getLength(nStream);
				if (fileSize > 0)
				{
					//Downloading file.
					receiveFile(args[1], nStream, fileSize);
				}
				else
				{
					Console.WriteLine("File doesn't exist");
				}
                
                               
			}
            catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}
            finally
			{
				nStream.Close();
				client.Close();
			}
			Console.WriteLine("Closing socket connection");
			client.Close();
		}

        private long getLength(NetworkStream io)
		{
			int recievedBytes;
			byte[] data = new byte[BUFSIZE];

			recievedBytes = io.Read(data, 0, data.Length);
            long size = long.Parse(Encoding.ASCII.GetString(data));
            string format = "B";
            double fileSize = 0;
            //Getting size in B Kb or Mb depending on size.
            if (size <= 1000)
            {
                format = "B";
				fileSize = size;
            }
            else if (1001 <= size && size <= (1e6 - 1))
            {
                format = "Kb";
				fileSize = size/1000;
            }
            else
            {
                format = "Mb";
				fileSize = size / 1e6;
            }
			Console.WriteLine("Size of file: {0} {1}", fileSize, format);
			return size;
}


		private void receiveFile (String fileName, NetworkStream io, long fileSize)
		{
			string path = "/root/Desktop/Downloads/" + LIB.extractFileName(fileName);
			int recievedBytes;
	
			byte[] data = new byte[fileSize];
            
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			int i = 0;
            //Getting chunks and write to file based on filesize.
			while(i != (fileSize/1000) + 1)
			{
				recievedBytes = io.Read(data, 0, data.Length);
				fs.Write(data, 0, recievedBytes);
				i++;
			} 
			Console.WriteLine("closing filestream");
            fs.Close();
         }
              
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
