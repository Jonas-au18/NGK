using System;
using System.Diagnostics;
namespace tcp
{
    public class COMPARE
    {
        private COMPARE()
		{}
        
        public static string Compare(string path, string filename)
        {
			Process process = new Process();
			process.StartInfo.FileName = "/bin/bash";
			process.StartInfo.Arguments = "-c\" diff <(/usr/bin/ssh root@10.0.0.1:~/Desktop/Exercise_6_c#/Exercise_6_c#/file_server/bin/Debug/" + path + ")> <(root@10.0.0.1:~/Desktop" + filename + ")>\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
            return process.StandardOutput.ToString();
        }
    }
}
