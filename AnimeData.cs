using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;

namespace Eden
{
    class AnimeData
    {
       
        public event EventHandler OnBotRetrieveFinish;
        public event EventHandler OnEpisodesRetrieveFinish;

        public AnimeData()
        {

        }

        public AnimeData(string animename)
        {

            AnimeName = animename;

        }

        public void setBotName(string botname){
            Botname = botname;
        }

        public void getAnimeInfo()
        {
            malApi mal = new malApi(Eden.username, Eden.password);
            mal.GetAnimeXML(AnimeName, "Currently Airing");
            string[] AnimeInfo = mal.GetAnimeInfo(AnimeName, "Currently Airing");
            string htmlpart1 = String.Empty;

            if (AnimeInfo[0] != "false")
            {
                string animeDataJs = String.Empty;
                if (AnimeInfo[1] != "" || AnimeInfo[1] != null || AnimeInfo[1] != String.Empty)
                {
                    animeDataJs = AnimeInfo[1].Replace("'", "\'");
                    animeDataJs = animeDataJs.Replace("'", "\'");
                    string[] animeInfoParts = animeDataJs.Split('#');
                    string Title = animeInfoParts[0].Replace("'", "\'");
                    string CoverUrl = animeInfoParts[5];
                    string Synopsis = animeInfoParts[4];
                    string ImgUrl = mal.DownloadCover(Title, CoverUrl, @"Search\cover");
                    htmlpart1 = "<div class=\"row\"> <div class=\"small-12 columns\" style=\"margin-top: 10%; text-align: center\"> <div style=\"margin-top: 20px; width: 100%;\"></div> <h2> " + Title + " </h2> </div> </div> </div> <div class=\"row\"> <div class=\"small-12 medium-4 large-2 columns\"> <img id=\"1\" src=\"" + CoverUrl + "\" style=\"height: 300px; float: left;\" /> </div> <div class=\"small-12 medium-8 large-10 columns\"> <h5> " + Synopsis + " </h5> </div> </div>";
                }

            }
            else
            {
                htmlpart1 = "<div class=\"row\"> <div class=\"large-12 columns\" style=\"margin-top: 10%; text-align: center\"> <div style=\"margin-top: 20px; width: 100%;\"></div> <h3> Failed to retrieve this anime, look it up using the search button above! </h3> </div></div></div>";
            }
           
            string htmlcontent;
            using (StreamReader html = new StreamReader(@"html\animeuneditted.html"))
            {
                htmlcontent = html.ReadToEnd();
            }

            string newhtml = htmlcontent.Replace("<replace>", htmlpart1);
            
            File.WriteAllText(@"html\anime.html", newhtml, Encoding.UTF8);


             retrieveBots = new Thread(new ThreadStart(this.retrieveBotsThread));
             retrieveBots.Start();
            
        }

    

        public void retrieveBotsCompleted()
        {

            string[] bots_array = botstring.Split('^');
            int bots_array_length = bots_array.Length;

            if (bots_array_length > 1)
            {

                botselector = "<select id=\"bots\" style=\"text-indent: 50%;\"><option>Select Your Prefered XDCC Bot here!</option>";
                string bot_options = "";
                foreach (string bot in bots_array)
                {
                    //every bot is outputted here, so prop html here
                    bot_options = bot_options + "<option value=\"" + bot + "\">" + bot + "</option>";
                }

                botselector = botselector + bot_options + "</select>";
                OnBotRetrieveFinish(this, new EventArgs());
                retrieveBots.Abort();
            }
            else if (syn_not_found)
            {
               //html stuff no bots found
                botselector = "<select id=\"bots\" style=\"text-indent: 50%;\"><option>'No bots found unfotunately!</option></select>";
                OnBotRetrieveFinish(this, new EventArgs());
                retrieveBots.Abort();
            }
            else
            {
                
                use_synonym = true;
                retrieveBotsThread();
            }

        }

       
        public void retrieveBotsThread()
        {
            
            StringBuilder bots = new StringBuilder();
            string path = @"animesyn.txt";
            int x = 0;
            int b = 0;

            if (File.Exists(path))
            {
                
                string syn_all = File.ReadAllText(path);

                if (syn_all != "")
                {
                    string[] syn_data = syn_all.Split('^');

                    foreach (string syn_anime in syn_data)
                    {
                     


                        string data_syn = syn_anime.Replace("\r\n", "");


                        if (data_syn != "")
                        {
                            string[] syn_split = data_syn.Split('|');
                            string syno = String.Empty;
                            string syno_f_ani = String.Empty;

                            syno = syn_split[1];
                            syno_f_ani = syn_split[0];


                            if (AnimeName.Contains(syno_f_ani))
                            {
                                string search = syno.Replace(" ", "+");
                                string data = readIntel("http://nibl.co.uk/bots.php?search=" + search);

                                while (true)
                                {
                                    int bot_name_pos_s = data.IndexOf("<td class=\"botname\">");
                                    if (bot_name_pos_s < 1)
                                    {
                                        if (x < 2)
                                        {
                                            syn_not_found = true;
                                        }
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
                                        b++;
                                    }
                                    x++;
                                }
                            }
                        }
                    }
                    
                }
                string search2 = AnimeName.Replace(" ", "+");
                string data2 = readIntel("http://nibl.co.uk/bots.php?search=" + search2);
                x = 0;
                while (true)
                {
                    int bot_name_pos_s = data2.IndexOf("<td class=\"botname\">");
                    if (bot_name_pos_s < 1)
                    {
                        if (x < 2)
                        {
                            syn_not_found = true;
                        }
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
                        b++;
                    }
                    x++;
                }
            }
            else
            {
                string search2 = AnimeName.Replace(" ", "+");
                string data2 = readIntel("http://nibl.co.uk/bots.php?search=" + search2);
                x = 0;
                while (true)
                {
                    int bot_name_pos_s = data2.IndexOf("<td class=\"botname\">");
                    if (bot_name_pos_s < 1)
                    {
                        if (x < 2)
                        {
                            syn_not_found = true;
                        }
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
                        botstring = bots.ToString();;
                        b++;
                    }
                    x++;
                }
            }

            retrieveBotsCompleted();
        }

        public void retrieveEpisodesFromBot()
        {
            retrieveEpisodes = new Thread(new ThreadStart(this.retrieveEpisodesThread));
            retrieveEpisodes.Start();
        }

        
        public void retrieveEpisodesThread()
        {

            int x = 0;
            int d = 0;
            StringBuilder animepack = new StringBuilder();
            string path = "animesyn.txt";
            string syn_data = "";
            if (File.Exists(path))
            {
                syn_data = File.ReadAllText(path);
            }

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

                        if (AnimeName.Contains(anime_f_name))
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
                                if (pack_info.Contains(Botname))
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
                                    d++;
                                }

                                anime_pack_string = animepack.ToString();
                                x++;
                            }
                        }
                    }
                }
                string search2 = AnimeName.Replace(" ", "+");
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
                    if (pack_info.Contains(Botname))
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

                        if (AnimeName.Contains(anime_f_name))
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
                                if (pack_info.Contains(Botname))
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
                                    d++;
                                }

                                anime_pack_string = animepack.ToString();
                                x++;
                            }
                        }
                        
                    }

                }
            }

            retrieveEpisodesCompleted();
        }


        public void retrieveEpisodesCompleted()
        {
            
            string[] anipack_split = anime_pack_string.Split('^');
            int amount_of_eps = anipack_split.Length;



            string episodesOptions = "";
            foreach(string anipacks in anipack_split){
                if (anipacks != "" || anipacks != String.Empty)
                {
                    string[] packsplit = anipacks.Split('|');
                
                    if (packsplit[1] != "" || packsplit[1] != null || packsplit[1] != String.Empty)
                    {
                        episodesOptions = episodesOptions + "<center><button class=\"button\" id=\"Packnum:" + packsplit[1] + "\">" + packsplit[0] + "</button></center><br>";
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
                    //html code shit
                   

            }

            epselector = episodesOptions;
            OnEpisodesRetrieveFinish(this, new EventArgs());
        }


        public static string readIntel(string url)
        {


            var webClient = new WebClient();

            string intel = webClient.DownloadString(url);


            return intel;
        }

       


        private string AnimeName;
        public static string botselector = String.Empty;
        private string botstring = String.Empty;
        public static string epselector = String.Empty;
        private string Botname = String.Empty;
        private string anime_pack_string;
        public static Thread retrieveBots;
        public static Thread retrieveEpisodes;
        private bool syn_not_found;
        private bool use_synonym;
    }
}
