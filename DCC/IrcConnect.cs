using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace AnimeXDCCWatcher
{
    class IrcConnect
    {
        //Default Constructor - null state
        public IrcConnect()
        {
            newIP = "54.229.0.87";
            newPort = 6667;
            newUsername = "";
            newPassword = "";
            newChannel = "#news";
        }

        //Overload Constructor - safe way to get variables
        public IrcConnect(string IP, int Port, string Username, string Password, string Channel)
        {
            newIP = IP;
            newPort = Port;
            newUsername = Username;
            newPassword = Password;
            newChannel = Channel;
        }

        //Accessor Functions
        public string getIP()
        {
            return newIP;
        }

        public int getPort()
        {
            return newPort;
        }

        public string getUsername()
        {
            return newUsername;
        }

        public string getPassword()
        {
            return newPassword;
        }

        public string getChannel()
        {
            return newChannel;
        }

        //Mutator Functions
        public void setIP(string IP)
        {
            newIP = IP;
        }

        public void setPort(int Port)
        {
            newPort = Port;
        }

        public void setUsername(string Username)
        {
            newUsername = Username;
        }

        public void setPassword(string Password)
        {
            newPassword = Password;
        }

        public void setChannel(string Channel)
        {
            newChannel = Channel;
        }


        //main program for class

        //connects to irc server, gives a boolean back on succesfull connect etc
        public bool Connect()
        {
            try
            {
                irc = new TcpClient(newIP, newPort);
                stream = irc.GetStream();

                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);

                Pinger ping = new Pinger();
                ping.Start();

                writeIrc("USER " + newUsername + " 8 * : AnimeXDCCWatcher");
                writeIrc("NICK " + newUsername);
                writeIrc("JOIN " + newChannel);

                return true;
            }
            catch
            {
                return false;
            }
        }
        public void quitIrc()
        {
            writeIrc("QUIT");
            irc.Close();
        }

        //function to write to the irc server, bit easier to use and better looking
        public static void writeIrc(string input)
        {
            writer.WriteLine(input);
            writer.Flush();
        }

        //Member Variables
        
        private int newPort;
        private string newUsername;
        private string newPassword;
       

        //Accessable stuff
        public static StreamReader reader;
        public static StreamWriter writer;
        public static NetworkStream stream;
        public static TcpClient irc;

        public static string newIP;
        public static string newChannel;
        
        
    }
}
