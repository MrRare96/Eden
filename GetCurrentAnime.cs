using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Eden
{
    class GetCurrentAnime
    {
        public event EventHandler OnIndexCompletion;

        //default constructor
        public GetCurrentAnime()
        {
           
        }

        public void RetrieveCurrentAnime()
        {
            retrieveAiring = new Thread(new ThreadStart(retrieveAiringThread));
            retrieveAiring.Start();
        }

        void retrieveAiringThread()
        {
            if (!File.Exists(@"cur_anime.txt"))
            {
                StringBuilder animeContent = new StringBuilder();

                using (WebClient client = new WebClient())
                {
                    string jsonfull = client.DownloadString("http://test.animeadvice.me/api/v1/animelist/?filters=%7B%22%24and%22%3A%5B%7B%22_id.t%22%3A%7B%22%24in%22%3A%5B%22TV%22%5D%7D%7D%2C%7B%22status%22%3A%7B%22%24in%22%3A%5B%22Currently+Airing%22%5D%7D%7D%5D%7D&fields=%5B%22_id.t%22%2C%22title%22%2C%22image%22%2C%22status%22%5D&limit=0&query_limit=0&group_limit=100&sort_type=max&skip=0&sort=%5B%5B%22weight%22%2C-1%5D%5D&group_sort=%5B%5B%22aired_from%22%2C1%5D%5D&format=json");
                    string path = @"cur_anime.txt";
                    bool check = true;
                    int x = 0;

                    malApi mal = new malApi(Eden.username, Eden.password);

                    string[] jsonar = jsonfull.Split(new string[] { "{\"_" }, StringSplitOptions.None);
                    try
                    {
                        while (jsonar[x] != null)
                        {
                            string json = "[{\"_" + jsonar[x + 1] + "]";

                            dynamic array = JsonConvert.DeserializeObject(json);
                            foreach (var item in array)
                            {
                                string AnimeTitle = mal.stringStrip(item.title.ToString());
                                string cover = mal.DownloadCover(AnimeTitle, item.image.ToString());
                                mal.GetAnimeXML(item.title.ToString(), "Currently Airing");

                                if (!File.Exists(path))
                                {
                                    // Create a file to write to. 
                                    using (StreamWriter sw = File.CreateText(path))
                                    {
                                        if (check)
                                        {
                                            sw.Write(x + "|Currently Airing|" + cover + "|" + AnimeTitle + "{}" + Environment.NewLine);
                                        }
                                        else
                                        {
                                            sw.Write(x + "|Mal: not found|" + cover + "|" + AnimeTitle + "{}" + Environment.NewLine);
                                        }
                                    }
                                }

                                try
                                {
                                    using (StreamWriter sw = File.AppendText(path))
                                    {
                                        if (check)
                                        {
                                            sw.Write(x + "|Currently Airing|" + cover + "|" + AnimeTitle + "{}" + Environment.NewLine);
                                        }
                                        else
                                        {
                                            sw.Write(x + "|Mal: not found|" + cover + "|" + AnimeTitle + "{}" + Environment.NewLine);
                                        }
                                    }
                                }
                                catch
                                {

                                }

                            }
                            x++;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            retrieveCover();
        }

        void retrieveCover()
        {

            StreamReader animes = new StreamReader(@"cur_anime.txt");
            string CurAnime = animes.ReadToEnd();

            string[] GetAnimeCon = CurAnime.Split(new string[] { "{}" }, StringSplitOptions.None);

            StringBuilder htmlBuild = new StringBuilder();

            foreach (string AnimeCont in GetAnimeCon)
            {
                string[] AnimeSplit = AnimeCont.Split('|');
                try
                {


                    string htmlline = "<div style=\"position: relative; z-index: -1; float: left; width: 250px; height: 350px; margin-left: 5px; margin-bottom: 10px; \"><a href=\"#1\" id=\"" + AnimeSplit[3] + "\"><img src=\"../cover/" + AnimeSplit[2] + "\" style=\"height: 300px; min-width: 220px; max-width: 220px;\"><br>" + AnimeSplit[3] + "</a></li></div>";
                    
                    if(htmlline.Contains("a href")){                  
                        htmlBuild.Append(htmlline);
                    }
 
                }
                catch
                {
                    if (AnimeSplit[0] == null)
                    {
                        break;
                    }
                }
                
            }

            string insert = htmlBuild.ToString();


            StreamReader html = new StreamReader(@"html\indexuneditted.html");
            string htmlcontent = html.ReadToEnd();



            string newhtmlcontent = htmlcontent.Replace("<replace>", insert);

            File.WriteAllText(@"html\index.html", newhtmlcontent, Encoding.UTF8);

            OnIndexCompletion(this, new EventArgs());
        }

        public static Thread retrieveAiring;
    }
}
