using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace AnimeXDCCWatcher
{
    class DCCClient
    {
       

        //default constructor
        public DCCClient()
        {
            newDccString = String.Empty;
        }

        //overload constructor
        public DCCClient(string dccString)
        {
            newDccString = dccString;
        }

        //Accessor functions
        public string getFileName()
        {
            return newFileName;
        }

        public long getFileSize()
        {
            return newFileSize;
        }

        public int getPortNum()
        {
            return newPortNum;
        }

        public string getIp()
        {
            return newIp;
        }

        //program functions

        //strips the response by a dcc server/bot into parts
        public void getDccParts()
        {
            /*
             * :bot PRIVMSG nickname :DCC SEND \"filename\" ip_networkbyteorder port filesize
            */
            
            int posStart = newDccString.IndexOf("\"");
            string dccstringp1 = newDccString.Substring(posStart + 1);
            int posEnd = dccstringp1.IndexOf(".");
            newFileName = dccstringp1.Substring(0, posEnd + 4);

            int PosStart = dccstringp1.IndexOf("\"");
            string dccstring = dccstringp1.Substring(PosStart + 1);

            dccparts = dccstring.Split(' ');
        }

        //sets all the variables needed for a dcc transfer, parts are from getDccParts()
        public void setDccValues()
        {
            
                if (IrcConnect.newChannel == "RareIRC")
                {
                    try
                    {
                        Continue = true;
                        newFileName = dccparts[5];
                        newIpAddress = Convert.ToInt64(dccparts[6]);
                        newPortNum = Convert.ToInt32(dccparts[7]);

                        IPEndPoint hostIPEndPoint = new IPEndPoint(newIpAddress, newPortNum);
                        string[] ipadressinfoparts = hostIPEndPoint.ToString().Split(':');
                        string[] ipnumbers = ipadressinfoparts[0].Split('.');
                        string ip = ipnumbers[3] + "." + ipnumbers[2] + "." + ipnumbers[1] + "." + ipnumbers[0];
                        newIp = ip;
                    }
                    catch
                    {
                        MessageBox.Show("Could not set values: ERROR NO IP ADDRESS RETRIEVED!");
                        Continue = false;
                    }     
                }
                else
                {
                    //Continue = true;
                    try
                    {
                        newIpAddress = Convert.ToInt64(dccparts[1]);
                        newPortNum = Convert.ToInt32(dccparts[2]);
                        IPEndPoint hostIPEndPoint = new IPEndPoint(newIpAddress, newPortNum);
                        string[] ipadressinfoparts = hostIPEndPoint.ToString().Split(':');
                        string[] ipnumbers = ipadressinfoparts[0].Split('.');
                        string ip = ipnumbers[3] + "." + ipnumbers[2] + "." + ipnumbers[1] + "." + ipnumbers[0];
                        newIp = ip;
                        Continue = true;
                        int charlength = dccparts[3].Length;
                        string filesizes = dccparts[3].Substring(0, charlength - 1);
                        newFileSize = Convert.ToInt64(filesizes);
                        MessageBox.Show("Filename: " + newFileName);   
                    }
                    catch
                    {
                        string content = "";
                        foreach (string part in dccparts)
                        {
                            content = content + " " + part; 
                        }
                        MessageBox.Show("Could not set values: ERROR NO IP ADDRESS RETRIEVED!" + Environment.NewLine + content);
                        Continue = false;
                    }
                    
                }     
        }

        //starts the download thread, thread needed to keep cmd window responsive
        public void StartDownload()
        {
            try
            {
                if (Continue)
                {
                    downloader = new Thread(new ThreadStart(this.Downloader));
                }
                else
                {
                    MessageBox.Show("Could not start downloader: false given on continue!");
                }
            }
            catch
            {
                MessageBox.Show("Could not start downloader: false given on continue!");
            }             
        }

        //creates a tcp socket connection for the retrieved ip/port from the dcc ctcp by the dcc bot/server
        public void Downloader()
        {
            MessageBox.Show("Download started, Your Anime will start playing soon! \r\n Anime episode: " + newFileName);        

            TcpClient dltcp = new TcpClient(newIp, newPortNum);
            NetworkStream dlstream = dltcp.GetStream();

            Int64 bytesReceived = 0;
            int count;
            byte[] buffer = new byte[4096];
            if (!Directory.Exists(@"downloads"))
            {
                Directory.CreateDirectory(@"downloads");
            }
            FileStream writeStream = new FileStream(@"downloads\" + newFileName, FileMode.Append, FileAccess.Write);

            bool launchPlayer = false;

            while (true)
            {
                if (bytesReceived > 50000 && !launchPlayer)
                {
                    mediaPlayer mp = new mediaPlayer(@"downloads\" + newFileName);
                    bool suc = mp.getMpSuc();
                    if (!suc)
                    {
                        MessageBox.Show("Failed to open mediaplayer \r\n File: " + newFileName);
                    }
                    launchPlayer = true;
                }
                DateTime end = DateTime.Now;
                count = dlstream.Read(buffer, 0, buffer.Length);            
                writeStream.Write(buffer, 0, count);
                bytesReceived += count;
                if (count == 0)
                {
                    break;
                }
            }
           // Console.WriteLine("Creating File, while loop ended");   
        }


        //Member vars
        private string newDccString;
        public static string newFileName;
        private long newIpAddress;
        private int newPortNum;
        private long newFileSize;
        private string newIp;
        private string[] dccparts;
        private bool Continue;
        public static Thread downloader;
    }
}
