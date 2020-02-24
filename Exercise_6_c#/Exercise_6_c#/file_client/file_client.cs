using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcp
{
	class file_client
	{
		/// <summary>
		/// The PORT.
		/// </summary>
		const int PORT = 9000;
		/// <summary>
		/// The BUFSIZE.
		/// </summary>
		const int BUFSIZE = 1000;

		/// <summary>
		/// Initializes a new instance of the <see cref="file_client"/> class.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments. First ip-adress of the server. Second the filename
		/// </param>
		private file_client (string[] args)
		{
			byte[] fileReq = new byte[BUFSIZE];

			TcpClient client = null;
			NetworkStream nStream = null;

			try
			{
				client = new TcpClient(args[0], PORT);
				Console.WriteLine("Connected to server");
                
				nStream = client.GetStream();

				LIB.writeTextTCP(nStream, args[1]);

				Console.WriteLine("Getting file");

				long fileSize = getLength(nStream);
				if (fileSize > 0)
				{
					receiveFile(args[1], nStream, fileSize);
				}
				else
				{
					Console.WriteLine("File doesn't exist");
				}

				string identical = COMPARE.Compare(args[1], args[1]);
				Console.WriteLine(identical);
                               
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

		/// <summary>
		/// Receives the file.
		/// </summary>
		/// <param name='fileName'>
		/// File name.
		/// </param>
		/// <param name='io'>
		/// Network stream for reading from the server
		/// </param>
		private void receiveFile (String fileName, NetworkStream io, long fileSize)
		{
			string path = "/root/Desktop/" + LIB.extractFileName(fileName);
			int recievedBytes;
	
			byte[] data = new byte[fileSize];
            
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			int i = 0;
			while(i != (fileSize/1000) + 1)
			{
				recievedBytes = io.Read(data, 0, data.Length);
				fs.Write(data, 0, recievedBytes);
				i++;
			} 
			Console.WriteLine("closing filestream");
            //fs.Close();
         }

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name='args'>
		/// The command-line arguments.
		/// </param>
		public static void Main (string[] args)
		{
			Console.WriteLine ("Client starts...");
			new file_client(args);
		}
	}
}
