using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Eden
{
    class mediaPlayer
    {
        public mediaPlayer(string file)
        {
            filename = file;
            mp_checker = true;
            LaunchPlayer();
        }

        public bool getMpSuc()
        {
            return mp_checker;
        }

        public void LaunchPlayer()
        {
            string set_path = "mploc.ini";
            if (File.Exists(set_path))
            {
                string loc_d = File.ReadAllText(set_path);
                string[] loc_s = loc_d.Split('=');
                string mp_loc = loc_s[1].Replace("\r\n", "");
                try
                {
                    string base_folder = Eden.appdir;
                    string location = mp_loc;
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.Arguments = "\"" + base_folder + @"\\" + filename + "\"";
                    start.FileName = location;
                    Process mediaplayer = Process.Start(start);

                }
                catch
                {
                    mp_checker = false;
                }
            }
            else
            {
                try
                {
                    string base_folder = Eden.appdir;
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.Arguments = "\"" + base_folder + @"\" + filename + "\"";
                    start.FileName = @"C:\Program Files (x86)\MPC-HC\mpc-hc.exe";
                    Process mediaplayer = Process.Start(start);
                }
                catch
                {
                    mp_checker = false;
                }
            }
        }
        private string filename;
        private bool mp_checker = true;
    }
}
