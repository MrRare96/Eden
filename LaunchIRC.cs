using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;

namespace AnimeXDCCWatcher
{
    class LaunchIRC
    {

       

        public LaunchIRC(string username)
        {
            irc.setUsername(username);
            bool checkConnect = irc.Connect();
            if (checkConnect)
            {

               
                //Console.WriteLine("succesful connected to the irc server!");
                string ircData = "";
                worker = new Thread(() =>
                {
                    
                    while ((ircData = IrcConnect.reader.ReadLine()) != null)
                    {


                        if (ircData.Contains("DCC SEND") && ircData.Contains(irc.getUsername()))
                        {
                            MessageBox.Show("download message received!");
                            DCCClient dcc = new DCCClient(ircData);
                            dcc.getDccParts();
                            dcc.setDccValues();
                            dcc.Downloader();
                            animeFile = dcc.getFileName();
                            //MessageBox.Show("Anime episode: " + animeFile);   
                            break;
                        }
                    }
                });
                worker.Start();
            }
            else
            {
                MessageBox.Show("Failed to connect to IRC server: irc.rizon.net channel: " + irc.getChannel());
            }
           
        }

        public static void sendXdcc(string BotName, string PackNum)
        {
            string ircXdccCommand = "PRIVMSG " + BotName + " :XDCC SEND #" + PackNum;
            IrcConnect.writeIrc("PRIVMSG " + BotName + " :XDCC CANCEL");
            IrcConnect.writeIrc(ircXdccCommand);
        }

        
       
        private string animeFile;
        public static Thread worker;
        public static IrcConnect irc = new IrcConnect();
        
    }
}
