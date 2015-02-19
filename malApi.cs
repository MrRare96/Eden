using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;


namespace Eden
{
    class malApi
    {

        //default construct
        public malApi()
        {
            Username = "";
            authInfo = "";
            login = false;
            data = "";
            using (StreamWriter log = File.AppendText("log.log"))
            {
                log.WriteLine("Line 25: no username or password found!");
            }
        }

        //overload construct
        public malApi(string username, string password)
        {
            Username = username;
            authInfo = username + ":" + password;
            using (StreamWriter log = File.AppendText("log.log"))
            {
                log.WriteLine("Line 33: Username and Password set, authInfo set");
            }

            try
            {
                WebRequest req = HttpWebRequest.Create("http://myanimelist.net/api/anime/search.xml?q=");
                req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
                req.Method = "GET";
               
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    data = reader.ReadToEnd();
                }
                login = true;
                using (StreamWriter log = File.AppendText("log.log"))
                {
                    log.WriteLine("Line 47: Login == true");
                }
            }
            catch
            {
                login = false;
                using (StreamWriter log = File.AppendText("log.log"))
                {
                    log.WriteLine("Line 52: Login == false");
                }
                MessageBox.Show("Failed to login into Mal" + Environment.NewLine + "Create a account on myanimelist.net or use different Password/Username!");
            }
        }
        //var retriever
        public void SetSAnimeName(string name)
        {
            SAnimeName = stringStrip(name);
        }

        //var sender
        public bool GetLogin()
        {
            return login;
        }

        public string GetUsername()
        {
            return Username;
        }

        public string GetSAnimeName()
        {
            return SAnimeName;
        }

        public static bool Login()
        {
            WebRequest req = HttpWebRequest.Create("http://myanimelist.net/api/anime/search.xml?q=naruto");
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(Eden.username + ":" + Eden.password));
            req.Method = "GET";
            try { 
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    string response = reader.ReadToEnd();
                    if (response != "" || response != String.Empty)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public bool GetAnimeXML(string AnimeName, string FileFolder, bool Continue = true)
        {
            try
            {
                Directory.CreateDirectory(base_folder + @"\" + FileFolder);
                using (StreamWriter log = File.AppendText("log.log"))
                {
                    log.WriteLine("Line 73: create directory " + base_folder + @"\" + FileFolder + " if not exist");
                }
                string search = stringStrip(AnimeName);
                string path = base_folder + @"\" + FileFolder + @"\" + search + ".xml";
                using (StreamWriter log = File.AppendText("log.log"))
                {
                    log.WriteLine("Line 78: redo is false");

                    log.WriteLine("Line 80: xml bestand: " + path + " does not exist");
                }
                WebRequest req = HttpWebRequest.Create("http://myanimelist.net/api/anime/search.xml?q=" + AnimeName);
                req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(authInfo));
                req.Method = "GET";
                using (StreamWriter log = File.AppendText("log.log"))
                {
                    log.WriteLine("Line 85: succesfull connect to MAL api and retrieving AnimeName unstripped");
                }
                bool checking = false;

                // Create a file to write to. 
                using (StreamReader reader = new StreamReader(req.GetResponse().GetResponseStream()))
                using (StreamWriter sw = new StreamWriter(path))
                {
                    string xmldata = reader.ReadToEnd();
                    if (xmldata.Contains("<entry>"))
                    {
                        using (StreamWriter log = File.AppendText("log.log"))
                        {
                            log.WriteLine("Line 95: xml content is not empty, writing file");
                        }
                        sw.Write(xmldata);
                        checking = true;
                        sw.Close();

                    }
                    else
                    {
                        using (StreamWriter log = File.AppendText("log.log"))
                        {
                            log.WriteLine("Line 101: xml content is empty, redoing search with stripped AnimeName");
                        }
                        checking = GetAnimeXML(search, FileFolder);
                        
                    }

                }
               
                return checking;
            }
            catch
            {
                return false;
            }
               
                              
        }

        public string[] GetAnimeInfo(string name, string FileFolder)
        {
            
                
                string File = base_folder + @"\" + FileFolder + @"\" + stringStrip(name) + ".xml";

                string XmlString = System.IO.File.ReadAllText(File);



                string[] anime_data = XmlString.Split(new string[] { "<entry>" }, StringSplitOptions.None);

                int x = 0;

                foreach (string anime in anime_data)
                {
                    if (anime.Contains("<status>"))
                    {
                        string anime_info_data = anime;

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

                        int pos_pic_start = anime_info_data.IndexOf("<image>");
                        string pic = anime_info_data.Substring(pos_pic_start + 7);
                        int pos_pic_end = pic.IndexOf("</image>");
                        pic = pic.Substring(0, pos_pic_end);

                        int pos_synonyms_start = anime_info_data.IndexOf("<synonyms>");
                        string synonyms = anime_info_data.Substring(pos_synonyms_start + 10);
                        int pos_synonyms_end = synonyms.IndexOf("</synonyms>");
                        synonyms = synonyms.Substring(0, pos_synonyms_end);


                        AnimeData = AnimeData + "}" + aaname + "#" + synonyms + "#" + episodes + "#" + score + "#" + synopsis + "#" + pic + "#" + status;



                        x++;
                    }
                }





                try
                {
                    AnimeDataAr = AnimeData.Split('}');
                    return AnimeDataAr;
                }
                catch
                {
                    string[] falses = {"false"};
                    return falses;
                }
               
            
            
            
        }

        public string DownloadCover(string animename, string url, string FileFolder = "cover")
        {
            try
            {
                string file = stringStrip(animename) + ".jpg";
                string folder_path = base_folder + @"\" + FileFolder + @"\";
                if (!Directory.Exists(folder_path))
                {
                    Directory.CreateDirectory(folder_path);
                }
                
                    WebClient Dllr = new WebClient();
                    Dllr.DownloadFile(url, folder_path + "\\" + file);
                    return file;
               
            }
            catch
            {
                return "noimg.jpg";
            }
            
        }

        public string stringStrip(string input)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string str = rgx.Replace(input, " ");
            
            str = str.Replace("  ", " ");
            using (StreamWriter log = File.AppendText("log.log"))
            {
                log.WriteLine("Line 233: stripString executed converting: " + input + " -> " + str);
            }  
            return str;
        }

        public string[] stringSplit(string input)
        {
            string[] stringSplit = input.Split(' ');
            using (StreamWriter log = File.AppendText("log.log"))
            {
                log.WriteLine("Line 240: splitting search string: " + input);
            }
            return stringSplit;
        }

      

        //Private Vars | Member Vars       
        private string authInfo;
        private string data;
        private string AnimeData;
        private string SAnimeName;

        //Public Vars
        public bool login;
        public string Username;
        public string base_folder = Directory.GetCurrentDirectory();
        public string[] AnimeDataAr;
    }
}
