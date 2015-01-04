using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace AnimeXDCCWatcher
{
    class searchAnime
    {

        public event EventHandler OnSearchFinish;

        public searchAnime()
        {
            Search = "";
        }

        public searchAnime(string search)
        {
            Search = search;
        }

        public void searchAnimeStart()
        {


            retrieveSearch = new Thread(new ThreadStart(retrieveSearchThread));

            retrieveSearch.Start();
        }

        public bool getSucces()
        {
            return succes;
            
        }

        void retrieveSearchThread()
        {
            {
                
                bool check = mal.GetAnimeXML(Search, "Search");

                retrieveSearchCompleted(check);

            }
        }

        void retrieveSearchCompleted(bool succesCheck)
        {

            succes = succesCheck;
            if (succes)
            {
                search_html = processSearch();
            }
            else
            {
                search_html = "<center> <h3> Failed to retrieve this anime, look it up using the search button above! </h3> </center>";
            }
            
            OnSearchFinish(this, new EventArgs());
        }

        public string processSearch()
        {
            string[] animeInfo = mal.GetAnimeInfo(Search, "Search");
            string htmlline = String.Empty;
            if (animeInfo[0] != "false")
            {
                foreach (string animeData in animeInfo)
                {
                    if (animeData != "" || animeData != String.Empty || animeData.Length > 10)
                    {
                        string animeDataJs = String.Empty;
                        animeDataJs = animeData.Replace("'", "\'");
                        animeDataJs = animeDataJs.Replace("'", "\'");
                        string[] animeInfoParts = animeDataJs.Split('#');
                        string Title = animeInfoParts[0].Replace("'", "\'");
                        string CoverUrl = animeInfoParts[5];
                        string ImgUrl = mal.DownloadCover(Title, CoverUrl, @"Search\cover");
                        htmlline = htmlline + "<div style=\"position: relative; z-index: -1; float: left; width: 250px; height: 350px; margin-left: 5px; margin-bottom: 10px; \"><a href=\"#1\" id=\"" + animeInfoParts[0] + "\"><img src=\"../search/cover/" + ImgUrl + "\" style=\"height: 300px; min-width: 220px; max-width: 220px;\"><br>" + animeInfoParts[0] + "</a></li></div>";
                    }

                }

                string htmlcontent;
                using (StreamReader html = new StreamReader(@"html\resultuneditted.html"))
                {
                    htmlcontent = html.ReadToEnd();
                }

                string[] splithtml = htmlcontent.Split(new string[] { "<result>" }, StringSplitOptions.None);
                string newhtml = splithtml[0] + htmlline + splithtml[1];
                File.WriteAllText(@"html\result.html", newhtml, Encoding.UTF8);
                return htmlline;
            }
            return "<center> <h3> Failed to retrieve this anime, look it up using the search button above! </h3> </center>";
        }

       


        //member vars
        private string Search;
        private String appdir = Path.GetDirectoryName(Application.ExecutablePath);
        private bool succes = false;
        private malApi mal = new malApi(AnimeXDCCWatcher.username, AnimeXDCCWatcher.password);
        public static Thread retrieveSearch;
        public static string search_html;
    }
}
