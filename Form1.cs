using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace AnimeXDCCWatcher
{
    public partial class Form1 : Form
    {

        BackgroundWorker retrieveAnimes;
        BackgroundWorker retrieveBots;
        BackgroundWorker retrieveEpisodes;
        BackgroundWorker launchIrc;

        public Form1()
        {
            InitializeComponent();

            retrieveAnimes = new BackgroundWorker();

            retrieveAnimes.DoWork += new DoWorkEventHandler(retrieveAnimes_DoWork);
            retrieveAnimes.ProgressChanged += new ProgressChangedEventHandler(retrieveAnimes_ProgressChanged);
            retrieveAnimes.RunWorkerCompleted += new RunWorkerCompletedEventHandler(retrieveAnimes_RunWorkerCompleted);
            retrieveAnimes.WorkerReportsProgress = true;
            retrieveAnimes.WorkerSupportsCancellation = true;

            retrieveBots = new BackgroundWorker();

            retrieveBots.DoWork += new DoWorkEventHandler(retrieveBots_DoWork);
            retrieveBots.ProgressChanged += new ProgressChangedEventHandler(retrieveBots_ProgressChanged);
            retrieveBots.RunWorkerCompleted += new RunWorkerCompletedEventHandler(retrieveBots_RunWorkerCompleted);
            retrieveBots.WorkerReportsProgress = true;
            retrieveBots.WorkerSupportsCancellation = true;

            retrieveEpisodes = new BackgroundWorker();

            retrieveEpisodes.DoWork += new DoWorkEventHandler(retrieveEpisodes_DoWork);
            retrieveEpisodes.ProgressChanged += new ProgressChangedEventHandler(retrieveEpisodes_ProgressChanged);
            retrieveEpisodes.RunWorkerCompleted += new RunWorkerCompletedEventHandler(retrieveEpisodes_RunWorkerCompleted);
            retrieveEpisodes.WorkerReportsProgress = true;
            retrieveEpisodes.WorkerSupportsCancellation = true;

            launchIrc = new BackgroundWorker();

            launchIrc.DoWork += new DoWorkEventHandler(launchIrc_DoWork);
            launchIrc.ProgressChanged += new ProgressChangedEventHandler(launchIrc_ProgressChanged);
            launchIrc.RunWorkerCompleted += new RunWorkerCompletedEventHandler(launchIrc_RunWorkerCompleted);
            launchIrc.WorkerReportsProgress = true;
            launchIrc.WorkerSupportsCancellation = true;

           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            image_repeater();
        }

        //all variables public to every function/void/returner
        public static StringBuilder imglist = new StringBuilder();
        public static StringBuilder animenames = new StringBuilder();
        public static string img_urls = String.Empty;
        public static string anime_names = String.Empty;
        public static string anime_and_pack = String.Empty;
        public static string botstring = String.Empty;
        public static string logincredentials = String.Empty;
        public static string state_r_a = String.Empty;
        public static string search_forbots = String.Empty;
        public static string anime_pack_string = String.Empty;
        public static string botname = String.Empty;
        public static string anime_name_fp = String.Empty;
        public static string episode_Selected = String.Empty;
        public static string filename = String.Empty;
        public static int amount = 0;
        public static bool goone = false;
        public static bool homeVisible = true;
        public static bool botlist_visible = true;
        public static bool use_synonym = false;
        public static bool file_status = true;
        public static bool syn_not_found = false;

       
       
        //all background workers
        void launchIrc_DoWork(object sender, EventArgs e)
        {

            string base_folder = Directory.GetCurrentDirectory();
            string file = base_folder + @"\HydraIRC\Downloads\" + filename;

            if (File.Exists(file))
            {
                StartPlayer(filename);
            }
            else
            {
                try
                {

                    string location = base_folder + @"\HydraIrc\HydraIRC.exe";
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.CreateNoWindow = true;
                    start.FileName = location;
                    Process irc = Process.Start(start);
                    //textBox1.Text = "irc launched succesful!";
                    int x = 0;
                    long oldlength = 0;


                    System.Threading.Thread.Sleep(5000);
                    if (File.Exists(file))
                    {
                        StartPlayer(filename);
                    }
                    else
                    {
                        filename = filename.Replace("?", "â„");
                        file = base_folder + @"\HydraIRC\Downloads\" + filename;
                        if (File.Exists(file))
                        {
                            StartPlayer(filename);
                        }
                    }

                    System.Threading.Thread.Sleep(5000);
                    while (true)
                    {
                        System.Threading.Thread.Sleep(1000);
                        x++;
                        if (File.Exists(file))
                        {
                            FileInfo fi = new FileInfo(file);
                            bool exists = fi.Exists;
                            long length = fi.Length;

                            launchIrc.ReportProgress(x);

                            if (length == oldlength)
                            {
                                irc.Kill();
                                break;
                            }


                            oldlength = length;
                        }
                        else
                        {
                            irc.WaitForExit();
                            break;
                        }

                    }
                }
                catch
                {

                }
            
            }

           

        }

        void launchIrc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBox2.Text = "irc launched, will quit when file is finishd downloading!";
        }

        void launchIrc_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int total_found = e.ProgressPercentage;
            textBox1.Text = "cycles " + total_found;
        }


        void retrieveEpisodes_DoWork(object sender, EventArgs e)
        {

            int x = 0;
            int d = 0;
            StringBuilder animepack = new StringBuilder();
            string path = "animesyn.txt";
            string syn_data = File.ReadAllText(path);

            if (!use_synonym)
            {
                

                if (File.Exists(path) && syn_data != "")
                {
                    string[] splitter_syn = syn_data.Split('^');
                    foreach (string anime_syn in splitter_syn)
                    {
                        if (anime_syn == "\r\n")
                        {
                            break;
                        }

                        string[] split_anime_syn = anime_syn.Split('|');
                        string anime_f_name = split_anime_syn[0].Replace("\r\n", "");
                        string anime_syn_name = split_anime_syn[1].Replace("\r\n", "");

                        if (anime_name_fp.Contains(anime_f_name))
                        {
                            string search = anime_syn_name.Replace(" ", "+");
                            string data = readIntel("http://nibl.co.uk/bots.php?search=" + search);

                            while (true)
                            {
                                int pack_info_pos_s = data.IndexOf("<tr class=\"botlistitem");
                                if (pack_info_pos_s < 1)
                                {
                                    break;
                                }
                                string pack_info_unf = data.Substring(pack_info_pos_s + 21);
                                data = pack_info_unf;
                                int pack_info_pos_e = pack_info_unf.IndexOf("</tr>");
                                string pack_info = pack_info_unf.Substring(0, pack_info_pos_e);
                                if (pack_info.Contains(botname))
                                {
                                    int ep_name_pos_s = pack_info.IndexOf("<td class=\"filename\">");
                                    string ep_name_unf = pack_info.Substring(ep_name_pos_s + 21);
                                    int ep_name_pos_e = ep_name_unf.IndexOf("<a href=");
                                    string ep_name = ep_name_unf.Substring(0, ep_name_pos_e);
                                    ep_name = ep_name.Replace("\n", "");

                                    int ep_pack_pos_s = pack_info.IndexOf("<td class=\"packnumber\">");
                                    string ep_pack_unf = pack_info.Substring(ep_pack_pos_s + 23);
                                    int ep_pack_pos_e = ep_pack_unf.IndexOf("</td>");
                                    string ep_pack = ep_pack_unf.Substring(0, ep_pack_pos_e);
                                    animepack.Append(ep_name + "|" + ep_pack + "^");
                                    retrieveEpisodes.ReportProgress(d);
                                    d++;
                                }

                                anime_pack_string = animepack.ToString();
                                x++;
                            }
                        }
                    }
                }
                string search2 = anime_name_fp.Replace(" ", "+");
                string data2 = readIntel("http://nibl.co.uk/bots.php?search=" + search2);
               

                while (true)
                {
                    int pack_info_pos_s = data2.IndexOf("<tr class=\"botlistitem");
                    if (pack_info_pos_s < 1)
                    {
                        break;
                    }
                    string pack_info_unf = data2.Substring(pack_info_pos_s + 21);
                    data2 = pack_info_unf;
                    int pack_info_pos_e = pack_info_unf.IndexOf("</tr>");
                    string pack_info = pack_info_unf.Substring(0, pack_info_pos_e);
                    if (pack_info.Contains(botname))
                    {
                        int ep_name_pos_s = pack_info.IndexOf("<td class=\"filename\">");
                        string ep_name_unf = pack_info.Substring(ep_name_pos_s + 21);
                        int ep_name_pos_e = ep_name_unf.IndexOf("<a href=");
                        string ep_name = ep_name_unf.Substring(0, ep_name_pos_e);
                        ep_name = ep_name.Replace("\n", "");

                        int ep_pack_pos_s = pack_info.IndexOf("<td class=\"packnumber\">");
                        string ep_pack_unf = pack_info.Substring(ep_pack_pos_s + 23);
                        int ep_pack_pos_e = ep_pack_unf.IndexOf("</td>");
                        string ep_pack = ep_pack_unf.Substring(0, ep_pack_pos_e);
                        animepack.Append(ep_name + "|" + ep_pack + "^");
                        retrieveEpisodes.ReportProgress(d);
                        d++;
                    }

                    anime_pack_string = animepack.ToString();
                    x++;
                }
            }
            else
            {
               
                if (File.Exists(path) && syn_data != "")
                {
                    
                    string[] splitter_syn = syn_data.Split('^');

                    


                    foreach (string anime_syn in splitter_syn)
                    {
                        if (anime_syn == "\r\n")
                        {
                            break;
                        }

                        string[] split_anime_syn = anime_syn.Split('|');
                        string anime_f_name = split_anime_syn[0].Replace("\r\n", "");
                        string anime_syn_name = split_anime_syn[1].Replace("\r\n", "");

                        if (anime_name_fp.Contains(anime_f_name))
                        {
                            string search = anime_syn_name.Replace(" ", "+");
                            string data = readIntel("http://nibl.co.uk/bots.php?search=" + search);

                            while (true)
                            {
                                int pack_info_pos_s = data.IndexOf("<tr class=\"botlistitem");
                                if (pack_info_pos_s < 1)
                                {
                                    break;
                                }
                                string pack_info_unf = data.Substring(pack_info_pos_s + 21);
                                data = pack_info_unf;
                                int pack_info_pos_e = pack_info_unf.IndexOf("</tr>");
                                string pack_info = pack_info_unf.Substring(0, pack_info_pos_e);
                                if (pack_info.Contains(botname))
                                {
                                    int ep_name_pos_s = pack_info.IndexOf("<td class=\"filename\">");
                                    string ep_name_unf = pack_info.Substring(ep_name_pos_s + 21);
                                    int ep_name_pos_e = ep_name_unf.IndexOf("<a href=");
                                    string ep_name = ep_name_unf.Substring(0, ep_name_pos_e);
                                    ep_name = ep_name.Replace("\n", "");

                                    int ep_pack_pos_s = pack_info.IndexOf("<td class=\"packnumber\">");
                                    string ep_pack_unf = pack_info.Substring(ep_pack_pos_s + 23);
                                    int ep_pack_pos_e = ep_pack_unf.IndexOf("</td>");
                                    string ep_pack = ep_pack_unf.Substring(0, ep_pack_pos_e);
                                    animepack.Append(ep_name + "|" + ep_pack + "^");
                                    retrieveEpisodes.ReportProgress(d);
                                    d++;
                                }

                                anime_pack_string = animepack.ToString();
                                x++;
                            }
                        }
                        
                    }

                }
            }
        }

        void retrieveEpisodes_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int total_found = e.ProgressPercentage;
            textBox1.Text = "animes found" + total_found;
        }

        void retrieveEpisodes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBox4.Text = anime_pack_string;
            string[] anipack_split = anime_pack_string.Split('^');
            int amount_of_eps = anipack_split.Length;

            foreach (Control item in flowLayoutPanel1.Controls.OfType<Control>())
            {
                if (item.Name == "eplist")
                {
                    flowLayoutPanel1.Controls.Remove(item);
                }
                
            }

           

            ComboBox eplist = new ComboBox();
            eplist.Name = "eplist";
            eplist.Location = new Point(230, 410);
            eplist.Size = new Size(350, 15);
            eplist.DropDownHeight = 800;
            eplist.Visible = botlist_visible;
            eplist.Text = "choose your episode here and hit play!";
            eplist.SelectedIndexChanged += new EventHandler(RetrieveValueEps_SelectedIndexChanged);

            foreach(string anipacks in anipack_split){
                string[] packsplit = anipacks.Split('|');

                
                string epname_f = String.Empty;

                try
                {
                   
                    epname_f = packsplit[0];
                    eplist.Items.Add(epname_f);
                }
                catch
                {
                    
                    epname_f = "No more episodes found!";
                }       

            }

            flowLayoutPanel1.Controls.Add(eplist);

            foreach (Control item in flowLayoutPanel1.Controls.OfType<Control>())
            {
                if (item.Name == "play")
                {
                    flowLayoutPanel1.Controls.Remove(item);
                }

            }


            Button Play = new Button();
         
            Play.Name = "play";
            Play.Location = new Point(0, 0); // this never seems to work :(
            Play.Size = new Size(120, 25);
            Play.Visible = true;
            Play.Text = "Play Episode!";
            Play.MouseClick += new MouseEventHandler(Play_MouseClick);

            flowLayoutPanel1.Controls.Add(Play);
        }


        void retrieveBots_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            textBox2.Text = "worker finishd";
            string[] bots_array = botstring.Split('^');
            textBox3.Text = botstring;
            int bots_array_length = bots_array.Length;

            if (bots_array_length > 1) 
            {

                foreach (Control item in flowLayoutPanel1.Controls.OfType<Control>())
                {
                    if (item.Name == "botlist")
                    {
                        flowLayoutPanel1.Controls.Remove(item);
                    }
                }

                ComboBox botlist = new ComboBox();
                botlist.Name = "botlist";
                botlist.Location = new Point(230, 430);
                botlist.Size = new Size(350, 15);
                botlist.DropDownHeight = 800;
                botlist.Visible = botlist_visible;
                botlist.Text = "select IRC bot here!";
                botlist.SelectedIndexChanged += new EventHandler(RetrieveValueBots_SelectedIndexChanged);
                foreach (string bot in bots_array)
                {
                    botlist.Items.Add(bot);
                }
                flowLayoutPanel1.Controls.Add(botlist);
            }
            else if (!file_status || syn_not_found)
            {
                ComboBox botlist = new ComboBox();
                botlist.Name = "botlist";
                botlist.Location = new Point(230, 430);
                botlist.Size = new Size(350, 15);
                botlist.DropDownHeight = 800;
                botlist.Visible = botlist_visible;
                botlist.Text = "No bots found, use the textbar above to add synonym for search!";
                flowLayoutPanel1.Controls.Add(botlist);
            }
            else
            {
                textBox4.Text = "running retrievebots again, now using synonym";
                use_synonym = true;
                retrieveBots.RunWorkerAsync();
            }
            
        }

        void retrieveBots_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int total_found = e.ProgressPercentage;
            textBox1.Text = "bots found" + total_found;
        }

        void retrieveBots_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!use_synonym)
            {
                string path = "animesyn.txt";
                string syn_data = File.ReadAllText(path);
                StringBuilder bots = new StringBuilder();

                int x = 0;
                int b = 0;

                if (File.Exists(path) && syn_data != "")
                {
                    string[] splitter_syn = syn_data.Split('^');
                    foreach (string anime_syn in splitter_syn)
                    {
                        if (anime_syn == "\r\n")
                        {
                            break;
                        }

                        string[] split_anime_syn = anime_syn.Split('|');
                        string anime_f_name = split_anime_syn[0].Replace("\r\n", "");
                        string anime_syn_name = split_anime_syn[1].Replace("\r\n", "");

                        if (anime_name_fp.Contains(anime_f_name))
                        {
                            string search = anime_syn_name.Replace(" ", "+");
                            string data = readIntel("http://nibl.co.uk/bots.php?search=" + search);

                            botlist_visible = true;


                            while (true)
                            {
                                int bot_name_pos_s = data.IndexOf("<td class=\"botname\">");
                                if (bot_name_pos_s < 1)
                                {
                                    break;
                                }
                                string botname_unf = data.Substring(bot_name_pos_s + 20);
                                data = botname_unf;
                                int bot_name_pos_e = botname_unf.IndexOf("<a href=");
                                string botname = botname_unf.Substring(0, bot_name_pos_e);
                                if (botstring.Contains(botname))
                                {
                                    // Console.WriteLine("bot already added");
                                }
                                else
                                {
                                    bots.Append(botname + "^");
                                    botstring = bots.ToString();
                                    retrieveBots.ReportProgress(b);
                                    b++;
                                }
                            }
                        }
                    }
                    string search2 = search_forbots.Replace(" ", "+");
                    string data2 = readIntel("http://nibl.co.uk/bots.php?search=" + search2);

                    botlist_visible = true;


                    while (true)
                    {
                        int bot_name_pos_s = data2.IndexOf("<td class=\"botname\">");
                        if (bot_name_pos_s < 1)
                        {
                            break;
                        }
                        string botname_unf = data2.Substring(bot_name_pos_s + 20);
                        data2 = botname_unf;
                        int bot_name_pos_e = botname_unf.IndexOf("<a href=");
                        string botname = botname_unf.Substring(0, bot_name_pos_e);
                        if (botstring.Contains(botname))
                        {
                            // Console.WriteLine("bot already added");
                        }
                        else
                        {
                            bots.Append(botname + "^");
                            botstring = bots.ToString();
                            retrieveBots.ReportProgress(b);
                            b++;
                        }
                        x++;
                    }
                }
                else
                {

                    if (File.Exists(path) && syn_data != "")
                    {

                        string[] splitter_syn = syn_data.Split('^');
                        foreach (string anime_syn in splitter_syn)
                        {
                            if (anime_syn == "\r\n")
                            {
                                break;
                            }

                            string[] split_anime_syn = anime_syn.Split('|');
                            string anime_f_name = split_anime_syn[0].Replace("\r\n", "");
                            string anime_syn_name = split_anime_syn[1].Replace("\r\n", "");

                            if (anime_name_fp.Contains(anime_f_name))
                            {
                                string search = anime_syn_name.Replace(" ", "+");
                                string data = readIntel("http://nibl.co.uk/bots.php?search=" + search);

                                botlist_visible = true;


                                while (true)
                                {
                                    int bot_name_pos_s = data.IndexOf("<td class=\"botname\">");
                                    if (bot_name_pos_s < 1)
                                    {
                                        break;
                                    }
                                    string botname_unf = data.Substring(bot_name_pos_s + 20);
                                    data = botname_unf;
                                    int bot_name_pos_e = botname_unf.IndexOf("<a href=");
                                    string botname = botname_unf.Substring(0, bot_name_pos_e);
                                    if (botstring.Contains(botname))
                                    {
                                        // Console.WriteLine("bot already added");
                                    }
                                    else
                                    {
                                        bots.Append(botname + "^");
                                        botstring = bots.ToString();
                                        retrieveBots.ReportProgress(b);
                                        b++;
                                    }



                                    x++;
                                }
                            }

                            int botsnumber = botstring.Split('^').Length;
                            if (botsnumber < 2)
                            {
                                syn_not_found = true;
                            }

                        }
                    }
                    else
                    {
                        file_status = false;
                    }
                }
                retrieveBots.ReportProgress(100);
            }
        }


        void retrieveAnimes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                textBox4.Text = "Task cancelled";
            } else if (e.Error != null) {
                textBox4.Text = "Error while performing task";
            }
            else
            {
                textBox4.Text = "task completed";
                textBox2.Text = "finishd retrieving cover images and anime names, hit refresh if there are already cover images on screen!";
                reset_layoutpanel();
                homeVisible = true;
                image_repeater();
                
            }

            refresh.Enabled = true;
            login.Enabled = true;
        }

        void retrieveAnimes_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int total_retrieved = e.ProgressPercentage;
            textBox4.Text = "anime found: " + total_retrieved.ToString();

            if (total_retrieved > 1)
            {
                reset_layoutpanel();
                state_r_a = "Retrieving Animes | This Can Take Up To A Minute! | Anime Found: " + total_retrieved.ToString();
                status_text(); 
                textBox2.Text = "retrieving animes from haruhichan and mal, please wait!";
            }
        }

        void retrieveAnimes_DoWork(object sender, DoWorkEventArgs e)
        {
            string data = readIntel("http://haruhichan.com/page/search/anime.html");
            int start_of_content = data.IndexOf("<div id=\"animeWrap\">");
            string start_container_cut = data.Substring(start_of_content);
            int end_of_content = start_container_cut.IndexOf("<div id=\"bottomBar\">");
            string container_img = start_container_cut.Substring(0, end_of_content);

            int y = 0;
            int a = 0;

            while (true) {

              
                int anime_url_search_start = container_img.IndexOf("<div class=\"titleText\">");
                if (anime_url_search_start < 1)
                {
                    //textBox2.Text = "loop ended accoridng to plan";
                    break;
                }
                string part_of_url = container_img.Substring(anime_url_search_start + 23);
                int anime_url_search_end = part_of_url.IndexOf("</div>");

                


                string animesearch = part_of_url.Substring(0, anime_url_search_end);
                container_img = part_of_url.Substring(anime_url_search_end);

          


                //animesearch = Regex.Replace(animesearch, "[^0-9a-zA-Z]+", " ");
               // textBox3.Text = animesearch;

                string animedata = searchMAL(animesearch);
                if (animedata == "could not login")
                {
                    //textBox2.Text = animedata;
                    //textBox1.Text = animesearch;
                    break;
                }
                else
                {
                    string[] anime = animedata.Split(new string[] { "<entry>" }, StringSplitOptions.None);
                    int total_founds = anime.Length;
                    int x = 0;
                    while (x != total_founds - 1)
                    {

                        string anime_info_data = anime[x + 1];

                        int pos_status_start = anime_info_data.IndexOf("<status>");
                        string status = anime_info_data.Substring(pos_status_start + 8);
                        int pos_status_end = status.IndexOf("</status>");
                        string astatus = status.Substring(0, pos_status_end);

                        int pos_aname_start = anime_info_data.IndexOf("<title>");
                        string aname = anime_info_data.Substring(pos_aname_start + 7);
                        int pos_aname_end = aname.IndexOf("</title>");
                        string aaname = aname.Substring(0, pos_aname_end);

                        int pos_pic_start = anime_info_data.IndexOf("<image>");
                        string pic = anime_info_data.Substring(pos_pic_start + 7);
                        int pos_pic_end = pic.IndexOf("</image>");
                        pic = pic.Substring(0, pos_pic_end);

                        if (astatus == "Currently Airing")
                        {
                            //textBox3.Text = "found: " + aaname + " is currently airing";
                            //textBox2.Text = "pic is = " + pic;
                            try
                            {

                                DlImg(pic, aaname);

                                retrieveAnimes.ReportProgress(a);

                                a++;

                                break;
                                
                            }
                            catch
                            {

                            }
                            
                            
                        }

                        
                        x++;
                    }

                    y++;

                    if(y > 1000){
                        //textBox2.Text = "something went wrong while searching for animes";
                        break;
                    }

               


                    
                }
                img_urls = imglist.ToString();
                anime_names = animenames.ToString();
               // textBox1.Text = "img urls: " + img_urls;

                string path = "animeimganame.txt";
                if (!File.Exists(path))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(anime_names);
                        sw.WriteLine("^");
                        sw.WriteLine(img_urls);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(path, false))
                    {
                        sw.WriteLine(anime_names);
                        sw.WriteLine("^");
                        sw.WriteLine(img_urls);
                    }
                }

            }

            retrieveAnimes.ReportProgress(100);
        }

        //all dynamic created events
        private void Play_MouseClick(object sender, MouseEventArgs e)
        {
            string[] anipack_split = anime_pack_string.Split('^');
            string packnum = String.Empty;
            foreach (string anipack in anipack_split)
            {
                if (anipack.Contains(episode_Selected))
                {
                    if (logincredentials != "") 
                    { 
                        string[] split_anipack = anipack.Split('|');
                        packnum = split_anipack[1];
                        changeNickXML();
                        changeBotPackXML(botname, packnum);
                        filename = split_anipack[0];
                        textBox4.Text = filename;
                        launchIrc.RunWorkerAsync();
                    }
                    else
                    {
                        textBox2.Text = "YOU DID NOT LOGIN, YOU NEED TO LOGIN INTO MAL!";
                    }
                }
            }

            textBox3.Text = "packnum for episode " + episode_Selected + " = " + packnum;
        }

        private void RetrieveValueBots_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox) sender;
            string botselect = comboBox.Text;
            textBox2.Text = botselect;
            
            if (botselect != "") {
                botname = botselect;
                try 
                { 
                    retrieveEpisodes.RunWorkerAsync();
                }
                catch
                {
                    textBox2.Text = "program is currently retrieving episodes for bot, cannot change bot now";
                }
            }
        }

        private void RetrieveValueEps_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string epselect = comboBox.Text;
            textBox4.Text = epselect;
            episode_Selected = epselect;
        }

        private void animeScreen_Click(object sender, MouseEventArgs e)
        {
            homeVisible = false;
            use_synonym = false;

            PictureBox pictures = sender as PictureBox;
            string animeinfo = pictures.Name.ToString();
            string[] animepinfo = animeinfo.Split('|');


            anime_name_fp = animepinfo[0];
            string anime_pic_fp = animepinfo[1];

            textBox3.Text = "img you clicked belongs to anime: " + anime_name_fp;

            reset_layoutpanel();


            Label anime_header = new Label();
            anime_header.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            anime_header.Location = new Point(10, 10);
            anime_header.Size = new Size(this.Width, 80);
            anime_header.AutoSize = false;
            anime_header.Font = new Font("Arial", 38);
            anime_header.TextAlign = ContentAlignment.TopCenter;
            anime_header.Text = anime_name_fp;
            anime_header.Visible = true;
            flowLayoutPanel1.Controls.Add(anime_header);

            Panel buttonP = new Panel();
            buttonP.Name = "buttonpannel";
            buttonP.Location = new Point(207, 100);
            buttonP.Size = new Size(200, 340);
            buttonP.Visible = true;
            flowLayoutPanel1.Controls.Add(buttonP);


            PictureBox animePic = new PictureBox();
            animePic.Name = anime_name_fp;
            animePic.Location = new Point(0, 0);
            animePic.Size = new Size(200, 300);
            animePic.SizeMode = PictureBoxSizeMode.StretchImage;
            animePic.Load(anime_pic_fp);
            animePic.Visible = true;
            buttonP.Controls.Add(animePic);


            Button back = new Button();
            back.Name = "back";
            back.Text = "Back";
            back.Location = new Point(0, 310);
            back.Size = new Size(70, 30);
            back.Enabled = true;
            back.Visible = true;
            back.MouseClick += new MouseEventHandler(back_Click);
            buttonP.Controls.Add(back);

            Panel rTbox_Cbox_b = new Panel();
            rTbox_Cbox_b.Name = "buttonpannel";
            rTbox_Cbox_b.Location = new Point(230, 100);
            rTbox_Cbox_b.Size = new Size(750, 340);
            rTbox_Cbox_b.Visible = true;
            flowLayoutPanel1.Controls.Add(rTbox_Cbox_b);

            string anime_data = MalXMLParser(anime_name_fp);
            string synopsis_mal = String.Empty;
            string score = String.Empty;
            string amount_eps = String.Empty;
            string richtextout = "You did not login, so no information can be retrieved from MAL!";
            if (anime_data.Contains("false"))
            {
                textBox3.Text = "you did not login, so no anime data could be shown!";
                MessageBox.Show("You did not login, to watch animes you need to login into your MAL account, using the form below!");
            }
            else
            {
                string[] data_splitter = anime_data.Split('|');
                if (data_splitter.Length < 2)
                {
                    richtextout = "Anime is propably not airing yet, no data could be found on it";
                }
                else
                {
                    synopsis_mal = data_splitter[1];
                    amount_eps = data_splitter[2];
                    score = data_splitter[3];
                    synopsis_mal = WebUtility.HtmlDecode(synopsis_mal);
                    synopsis_mal = convertHtml(synopsis_mal);

                    richtextout = "Total Episodes: " + amount_eps + "\n" + "MAL Score: " + score + "\n" + "\n" + "\n" + synopsis_mal + "\n" + "\n" + "\n" + "______________________________" + "\n" + "\n" + "If there are no bots available, look for a subgroup manually and use the name they give their episodes for this anime as synonim(and remove all the _ - ! from the synonym!). All synonyms are saved so you only have to do it once!";

                    search_forbots = anime_name_fp;

                    retrieveBots.RunWorkerAsync();
                
                }
           }

            RichTextBox synopsis = new RichTextBox();
            synopsis.Name = "synopsis " + anime_name_fp;
            synopsis.Location = new Point(0,0);
            synopsis.Size = new Size(750, 300);
            synopsis.Text = richtextout;
            synopsis.Visible = true;
            rTbox_Cbox_b.Controls.Add(synopsis);

            TextBox synonims = new TextBox();
            synonims.Name = "synonims";
            synonims.Location = new Point(0, 310);
            synonims.Size = new Size(650, 20);
            synonims.Visible = true;
            rTbox_Cbox_b.Controls.Add(synonims);

            Button add_synonim = new Button();
            add_synonim.Name = "add_synonim";
            add_synonim.Location = new Point(660, 310);
            add_synonim.Size = new Size(90, 20);
            add_synonim.Text = "Add synonym!";
            add_synonim.MouseClick += new MouseEventHandler(add_sysnonim_Click);
            rTbox_Cbox_b.Controls.Add(add_synonim);


            textBox4.Text = "picture was clicked!";

        }

        private void back_Click(object sender, MouseEventArgs e)
        {
            botlist_visible = false;
            reset_layoutpanel();
            homeVisible = true;
            
            image_repeater();
        }

        private void add_sysnonim_Click(object sender, MouseEventArgs e)
        {
            
            TextBox retrieve_syn = (TextBox)flowLayoutPanel1.Controls.Find("synonims", true)[0];

            textBox3.Text = retrieve_syn.Text;
            string syn = retrieve_syn.Text;
            string path = "animesyn.txt";
            
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(anime_name_fp);
                    sw.WriteLine("|");
                    sw.WriteLine(syn);
                    sw.WriteLine("^");
                }
                retrieveBots.RunWorkerAsync();
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(anime_name_fp);
                    sw.WriteLine("|");
                    sw.WriteLine(syn);
                    sw.WriteLine("^");
                   
                }
                retrieveBots.RunWorkerAsync();
            }
        }

        //all functions that are either for use in background or by the ui thread
        public void status_text()
        {
            if (state_r_a == "")
            {
                state_r_a = "Login into MAL using the small login form blow!";
            }

            Label state = new Label();
            state.Anchor = AnchorStyles.Right | AnchorStyles.Left;
            state.Location = new Point(10, 10);
            state.Size = new Size(this.Width, this.Height);
            state.AutoSize = false;
            state.Font = new Font("Arial", 32);
            state.TextAlign = ContentAlignment.MiddleCenter;
            state.Text = state_r_a;
            state.Visible = true;
            flowLayoutPanel1.Controls.Add(state);

        }
        
        private void image_repeater()
        {

                string path = "animeimganame.txt";
                string[] urls = new string[] { "" };
                
                string[] names = new string[] { "" };

                botstring = String.Empty;

                if (File.Exists(path))
                {

                    try
                    {
                        string txtdata = File.ReadAllText(path);
                        string[] splitter = txtdata.Split('^');
                        urls = splitter[1].Split('|');
                        names = splitter[0].Split('|');
                        amount = urls.Length;
                    }
                    catch
                    {
                        textBox4.Text = "something went wrong while splitting stuff";
                    }

                    if (amount < 1)
                    {
                        status_text();

                        textBox1.Text = "there are no animes retrieved, yet, please login or refresh to retrieve current airing animes!";

                        
                    }
                    else 
                    {
                       reset_layoutpanel();
                       int x = 0;
                       PictureBox[] pictures = new PictureBox[amount];
                       Label[] labels = new Label[amount];
                       Panel[] panels = new Panel[amount];
                       string base_folder = Directory.GetCurrentDirectory();

                       while (x != amount - 1)
                       {
                           string file = base_folder + "\\cover\\" + urls[x];
                           file = file.Replace("\r\n", "");
                           string name = names[x];
                       
                           System.Drawing.Image img = System.Drawing.Image.FromFile(file);

                           panels[x] = new Panel();
                           panels[x].Name = "Panel" + x.ToString();
                           panels[x].Location = new Point(20 + (200 * x), 10);
                           panels[x].Size = new Size(200, 300 + 50);
                           panels[x].Padding = new Padding(10);



                           pictures[x] = new PictureBox();
                           pictures[x].Name = name + "|" + file;
                           //pictures[x].Location = new Point(10 + (img.Width * x), 10);
                           pictures[x].Size = new Size(200, 300);
                           pictures[x].SizeMode = PictureBoxSizeMode.StretchImage;
                           pictures[x].MouseClick += new MouseEventHandler(animeScreen_Click);
                           pictures[x].Load(file);
                           pictures[x].Visible = homeVisible;

                           labels[x] = new Label();
                           labels[x].Location = new Point(0, 10 + 300);
                           labels[x].Size = new Size(img.Width + 10, 40);
                           labels[x].Text = name;
                           labels[x].Visible = homeVisible;

                           panels[x].Controls.Add(pictures[x]);
                           panels[x].Controls.Add(labels[x]);
                           flowLayoutPanel1.Controls.Add(panels[x]);

                       

                           x++;
                       }
                       
                    }
                }
                else
                {
                    textBox1.Text = "something wen twrong while ouputting imgs, prop opening txt file";
                    if (logincredentials == "")
                    {
                        MessageBox.Show("Welcome, to start using this program you need to" + System.Environment.NewLine + "login into your MyAnimeList account." + System.Environment.NewLine + "After that, you need to click on Retrieve Anime to continue!");
                        state_r_a = "Log in using the form below!";
                        status_text();
                    }
                    else
                    {
                        retrieveAnimes.RunWorkerAsync();
                    }
                    
                }
        }

        public void reset_layoutpanel()
        {
            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; --ix)
            {
                var ctl = flowLayoutPanel1.Controls[ix];
                ctl.Dispose();
            }
        }

        public static string convertHtml(string input)
        {
            Regex regex = new Regex("\\<[^\\>]*\\>");
            string output = regex.Replace(input, String.Empty);
            return output;
        }

        public static string MalXMLParser(string animename)
        {
            string animedata = searchMAL(animename);
            string output = String.Empty;
            if (animedata == "could not login")
            {
                output = "login== false";
            }
            else
            {
                string[] anime = animedata.Split(new string[] { "<entry>" }, StringSplitOptions.None);
                int total_founds = anime.Length;
                int x = 0;
               
                while (x != total_founds - 1)
                {

                    string anime_info_data = anime[x + 1];

                    int pos_status_start = anime_info_data.IndexOf("<status>");
                    string status = anime_info_data.Substring(pos_status_start + 8);
                    int pos_status_end = status.IndexOf("</status>");
                    string astatus = status.Substring(0, pos_status_end);

                    int pos_aname_start = anime_info_data.IndexOf("<title>");
                    string aname = anime_info_data.Substring(pos_aname_start + 7);
                    int pos_aname_end = aname.IndexOf("</title>");
                    string aaname = aname.Substring(0, pos_aname_end);

                    int pos_syn_start = anime_info_data.IndexOf("<synopsis>");
                    string synopsis = anime_info_data.Substring(pos_syn_start + 10);
                    int pos_syn_end = synopsis.IndexOf("</synopsis>");
                    synopsis = synopsis.Substring(0, pos_syn_end);

                    int pos_score_start = anime_info_data.IndexOf("<score>");
                    string score = anime_info_data.Substring(pos_score_start + 7);
                    int pos_score_end = score.IndexOf("</score>");
                    score = score.Substring(0, pos_score_end);

                    int pos_episodes_start = anime_info_data.IndexOf("<episodes>");
                    string episodes = anime_info_data.Substring(pos_episodes_start + 10);
                    int pos_episodes_end = episodes.IndexOf("</episodes>");
                    episodes = episodes.Substring(0, pos_episodes_end);

                    if (astatus == "Currently Airing")
                    {
                        output = aaname + "|" + synopsis + "|" + episodes + "|" + score + "|login == true";
                        break;
                    }
                    else
                    {
                        output = "anime is not airing, seperate function needs to be written for this, if you see this contact rareamv on the forum!";
                    }
                    x++;
                }

               
            }
            return output;
        }

        public static string readIntel(string url)
        {


            var webClient = new WebClient();

            string intel = webClient.DownloadString(url);


            return intel;
        }

        public static void DlImg(string link, string animename)
        {

            string[] imgnames = link.Split('/');
            string img = imgnames[6];
            string file = animename + "_" + img;
            file = CleanFileName(file);
            imglist.Append(file + "|");
            animenames.Append(animename + "|");


            string base_folder = Directory.GetCurrentDirectory();
            string folder_path = base_folder + "\\cover\\";
            if (!Directory.Exists(folder_path))
            {
                Directory.CreateDirectory(folder_path);
            }

            WebClient Dllr = new WebClient();
            Dllr.DownloadFile(link, folder_path + "\\" + file);


        }

        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), ""));
        }

        public static string searchMAL(string search)
        {
            string authInfo = logincredentials;
            try
            {
                WebRequest req = HttpWebRequest.Create("http://myanimelist.net/api/anime/search.xml?q=" + search);
                req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));

                req.Method = "GET";

                string source;
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }

                return source;
            } catch {
                string source = "could not login";
                return source;
            }
        }

       

        public void changeNickXML()
        {
            try
            {
                string[] logincr = logincredentials.Split(':');
                string nick = logincr[0];
                string base_folder = Directory.GetCurrentDirectory();
                string path = base_folder + @"\HydraIRC\Profile_Default.xml";
                string data = File.ReadAllText(path);
                int pos_nickname_s = data.IndexOf("<identities>");
                string part_of_ident = data.Substring(pos_nickname_s + 12);
                int pos_nickname_e = part_of_ident.IndexOf("</identities>");
                string ident = part_of_ident.Substring(0, pos_nickname_e);
                data = data.Replace(ident, "<identity name=\"" + nick + "\" id=\"1\" realname=\"" + nick + "\" username=\"" + nick + "\">\n<nick name=\"" + nick + "\"/></identity>");
                File.WriteAllText(path, data);
                textBox2.Text = "succesfull written to profile xml file";
            }
            catch
            {
                textBox3.Text = "something went wrong while writing to pofile xml 1";
            }
        }

        public void StartPlayer(string file)
        {
            try
            {
                string base_folder = Directory.GetCurrentDirectory();
                string location = base_folder + @"\MPC\mpc-hc.exe";
                ProcessStartInfo start = new ProcessStartInfo();
                start.Arguments = "\"" + base_folder + @"\HydraIRC\Downloads\" + file + "\"";
                start.FileName = location;
                Process irc = Process.Start(start);
                //textBox1.Text = "irc launched succesful!";
            }
            catch
            {
                //textBox1.Text = "something went wrong while starting vlc?";
            }
        }

        public void changeBotPackXML(string bot, string packnumb)
        {
           
                string base_folder = Directory.GetCurrentDirectory();
                string path = base_folder + @"\HydraIRC\Profile_Default.xml";
                string data = File.ReadAllText(path);
                
                int pos_com_s = data.IndexOf("<commandprofile name=\"irc.rizon.net_OnLoggedIn\" commands=\"");
                string part_of_com = data.Substring(pos_com_s + 58);
                int pos_com_e = part_of_com.IndexOf("\"/>");
                string com = part_of_com.Substring(0, pos_com_e);

                data = data.Replace(com, "/join #horriblesubs\\015\\012\\015\\012/msg " + bot + "xdcc send #" + packnumb);
                File.WriteAllText(path, data);
                textBox2.Text = "succesfull written to profile xml file";
            
               
        }
        //all controls made by designer

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uname_TextChanged(object sender, EventArgs e)
        {

        }

        private void login_Click(object sender, EventArgs e)
        {
            logincredentials = uname.Text + ":" + password.Text;

            string test = searchMAL("bleach");
            reset_layoutpanel();
            if (test != "could not login")
            {
                textBox1.Text = "Succesful login on mal!";
               
                MessageBox.Show("You are logged in!");
                
                reset_layoutpanel();
                homeVisible = true;
                image_repeater();
            }
            else
            {
                textBox1.Text = "Something went wrong while logging into mal, try again!";
                state_r_a = "Something went wrong while logging into mal, try again!";
                status_text();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void refresh_Click(object sender, EventArgs e)
        {
            retrieveAnimes.RunWorkerAsync();
            login.Enabled = false;
            refresh.Enabled = false;
        }

        private void printimg_Click(object sender, EventArgs e)
        {
            reset_layoutpanel();
            homeVisible = true;
            image_repeater();
        }

        private void password_TextChanged(object sender, EventArgs e)
        {

        }

        private void DelCovers_Click(object sender, EventArgs e)
        {
            string path = "animeimganame.txt";
            File.Delete(path);
        }

       
       

    }
}
