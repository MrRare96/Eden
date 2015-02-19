using Microsoft.Win32;
using mshtml;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Eden
{
    public partial class Eden : Form
    {

        public Eden()
        {
            WebBrowserVersionEmulation();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             

             if (!File.Exists(@"cur_anime.txt"))
             {
                 redirectWebUI(@"html\loading.html");
             }
             else
             {
                 redirectWebUI(@"html\index.html");
             }
             
        }

        void get_OnIndexCompletion(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate { redirectWebUI(@"html\index.html"); }));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {

            try
            {
                if (LaunchIRC.worker.IsAlive && LaunchIRC.worker != null)
                {
                    LaunchIRC.irc.quitIrc();
                    LaunchIRC.worker.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if (Pinger.pingSender.IsAlive && Pinger.pingSender != null)
                {
                    Pinger.pingSender.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if (AnimeData.retrieveBots.IsAlive && AnimeData.retrieveBots != null)
                {
                    AnimeData.retrieveBots.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if (AnimeData.retrieveEpisodes.IsAlive && AnimeData.retrieveEpisodes != null)
                {
                    AnimeData.retrieveEpisodes.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if (searchAnime.retrieveSearch.IsAlive && searchAnime.retrieveSearch != null)
                {
                    searchAnime.retrieveSearch.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if (GetCurrentAnime.retrieveAiring.IsAlive && GetCurrentAnime.retrieveAiring != null)
                {
                    GetCurrentAnime.retrieveAiring.Abort();
                }
            }
            catch
            {

            }

            try
            {
                if ( DCCClient.downloader.IsAlive && DCCClient.downloader != null)
                {
                    DCCClient.downloader.Abort();
                }
            }
            catch
            {

            }

        }

        //all variables public to every function/void/returner
        public static string filename = String.Empty;
        public static string test_mp_loc = String.Empty;
        public static string search_input = String.Empty;
        public static string html_content = String.Empty;
        public static string username = String.Empty;
        public static string password = String.Empty;
        public static String appdir = Path.GetDirectoryName(Application.ExecutablePath);
        public static string check_selected = "Select Your Prefered XDCC Bot here!";
        public static string check_selected2 = "Select Your Episode Here";
        public static string html_search = String.Empty;
        public static string Anime = String.Empty;
        public static string Bot = String.Empty;
        public static string AnimeFile = "none";
        public static string mp_loc = String.Empty;
        public static string text = String.Empty;
        public static string id = String.Empty;
        public static string packnum = String.Empty;
        public static bool mp_checker = false;
        public static bool First_Call = true;
        public static bool Login = false;

        private void add_sysnonim_Click(object sender, MouseEventArgs e)
        {
            
            
            
            string path = "animesyn.txt";

            
            
            if (!File.Exists(path))
            {
                // Create a file to write to. 
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(Anime);
                    sw.WriteLine("|");
                    sw.WriteLine("^");
                }
                
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(Anime);
                    sw.WriteLine("|");
                    sw.WriteLine("^");
                }
            }
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
       
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

     
       
        private void PlayerLoc()
        {
            OpenFileDialog playerLocation = new OpenFileDialog();
            playerLocation.Filter = "programs|*.exe";
            playerLocation.Title = "Select Your Standard Mediaplayer";

            string set_path = "mploc.ini";

            if (playerLocation.ShowDialog() == DialogResult.OK)
            {
                string PlayerLocation = playerLocation.FileName;
                MessageBox.Show("player set: " + PlayerLocation);
                insertIntoHtml("", "nothing", "close", "noMP");
                if (!File.Exists(set_path))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(set_path))
                    {
                        sw.WriteLine("MPLOC =" + PlayerLocation);
                        
                    }
                    
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(set_path, false))
                    {
                        sw.WriteLine("MPLOC =" + PlayerLocation);                      
                        
                    }
                    
                }
            }

        }

        public void notLoggedIn()
        {
            if (username == "" || username == String.Empty)
            {
                insertIntoHtml("", "nothing", "open");
            }
        }

        public void noMpWarning()
        {
            if (!File.Exists(@"mploc.ini"))
            {
                insertIntoHtml("", "nothing", "open", "noMP");
            }
            else
            {
                using (StreamReader reader = new StreamReader(@"mploc.ini"))
                {
                    string content = reader.ReadToEnd();
                    if (content == "" || content == String.Empty)
                    {
                        insertIntoHtml("", "nothing", "open", "noMP");
                    }
                }
            }
            
        }
      
        public void insertIntoHtml(string textin, string idin, string modal = "close", string modalName = "lModal")
        {
            text = textin;
            id = idin;
            HtmlElement head = webUI.Document.GetElementsByTagName("head")[0];
            HtmlElement scriptEl = webUI.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            string js = "function setText() { document.getElementById('" + id + "').innerHTML= '" + text + "'; } $('#" + modalName + "').foundation('reveal', '" + modal + "');";
            element.text = js;
            head.AppendChild(scriptEl);
            webUI.Document.InvokeScript("setText");
            
        }

        private void webUI_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (username != String.Empty)
            {
                insertIntoHtml(username, "show");
            }

            if (First_Call)
            {
                webUI.Document.Click += new HtmlElementEventHandler(Document_Click);
                First_Call = false;
            }


            if (!Login)
            {
                notLoggedIn();
            }
            
        }

        void Document_Click(object sender, HtmlElementEventArgs e)
        {
           

            if (webUI.Document.ActiveElement.TagName == "A" || webUI.Document.ActiveElement.TagName == "BUTTON" || webUI.Document.ActiveElement.TagName == "SELECT")
            {
                
                switch (webUI.Document.ActiveElement.Id)
                {
                    case "login":
                        var username_html = (IHTMLInputElement)webUI.Document.GetElementById("username").DomElement;
                        var password_html = (IHTMLInputElement)webUI.Document.GetElementById("password").DomElement;
                        username = username_html.value;
                        password = password_html.value;
                        Login = malApi.Login();
                        if(Login){
                            insertIntoHtml(username_html.value, "show");
                            noMpWarning();
                            LaunchIRC newIrc = new LaunchIRC(username_html.value);
                            MessageBox.Show("Succesfull login");
                            if (!File.Exists(@"cur_anime.txt"))
                            {
                                
                                GetCurrentAnime get = new GetCurrentAnime();
                                get.RetrieveCurrentAnime();
                                get.OnIndexCompletion += get_OnIndexCompletion;
                            }
                        } else {
                            MessageBox.Show("Could not login, try again!");
                        }
                        break;
                    case "retrieve_a":
                        if (File.Exists(@"cur_anime.txt"))
                        {
                            File.Delete(@"cur_anime.txt");
                        }
                        redirectWebUI(@"html\loading.html");
                        GetCurrentAnime newget = new GetCurrentAnime();
                        newget.RetrieveCurrentAnime();
                        newget.OnIndexCompletion += newget_OnIndexCompletion;
                        
                      
                        break;
                    case "search_submit":
                        var searchstring = (IHTMLInputElement)webUI.Document.GetElementById("sstring").DomElement;
                        searchAnime search = new searchAnime(searchstring.value);

                        redirectWebUI(@"html\loading.html");
                        search.searchAnimeStart();
                        
                        search.OnSearchFinish += search_OnSearchFinish;
                       
                        break;
                    case "bots":
                        var element = webUI.Document.GetElementById("bots");
                        dynamic dom = element.DomElement;
                        int index = (int)dom.selectedIndex();
                        string botname = "none";
                        if (index != -1)
                        {
                          botname = element.Children[index].InnerText;
                          Bot = botname;
                        }
                        if (botname != check_selected)
                        {
                            AnimeData ndata = new AnimeData(Anime);
                            ndata.setBotName(botname);
                            ndata.retrieveEpisodesFromBot();
                            ndata.OnEpisodesRetrieveFinish += ndata_OnEpisodesRetrieveFinish;
                            Bot = botname;
                            check_selected = botname;
                        }
                        
                        break;
                    case "settingsSet":
                        break;
                    case "mediaPlayer":
                        PlayerLoc();
                        break;
                    case "Home":
                        break;
                    case "search":
                        break;
                    case "settingsbutton":
                        break;
                    case "Login":
                        break;
                    default:
                        try
                        {
                            string content = webUI.Document.ActiveElement.Id.ToString();
                            if (content.Contains("Packnum:"))
                            {
                                packnum = content.Split(':')[1];
                                textBox1.Text = "EPisode clicked, packnum: " + packnum + " bot: " + Bot;
                                try
                                {
                                    DCCClient.downloader.Abort();
                                    LaunchIRC.sendXdcc(Bot, packnum);
                                    textBox3.Text = "DCCClient started downloading file: " + AnimeFile;
                                }
                                catch
                                {
                                    LaunchIRC.sendXdcc(Bot, packnum);
                                }
                            }
                            else
                            {
                                Anime = content;
                                textBox1.Text = "Anime Picture was clicked for anime: " + Anime;
                                AnimeData data = new AnimeData(Anime);
                                data.getAnimeInfo();
                                redirectWebUI(@"html\anime.html");
                                data.OnBotRetrieveFinish += data_OnBotRetrieveFinish;
                            }
                        }
                        catch
                        {

                        }
                        
                        
                        break;
                }
            }
        }
        void newget_OnIndexCompletion(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate { redirectWebUI(@"html\index.html"); }));
        }

        void ndata_OnEpisodesRetrieveFinish(object sender, EventArgs e)
        {
            string epSelector = AnimeData.epselector;
            Invoke(new MethodInvoker(delegate { insertIntoHtml(epSelector, "episodes"); }));
        }

        void data_OnBotRetrieveFinish(object sender, EventArgs e)
        {
            string botSelector = AnimeData.botselector;
            Invoke(new MethodInvoker(delegate { insertIntoHtml(botSelector, "botselector"); })); 
        }

        void search_OnSearchFinish(object sender, EventArgs e)
        {
            html_search = searchAnime.search_html;
            Invoke(new MethodInvoker(delegate { redirectWebUI(@"html\result.html"); }));
        }

        private static void WebBrowserVersionEmulation()
        {
            const string BROWSER_EMULATION_KEY =
            @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            //
            // app.exe and app.vshost.exe
            String appname = Process.GetCurrentProcess().ProcessName + ".exe";
            //
            // Webpages are displayed in IE9 Standards mode, regardless of the !DOCTYPE directive.
            const int browserEmulationMode = 9999;

            RegistryKey browserEmulationKey =
                Registry.CurrentUser.OpenSubKey(BROWSER_EMULATION_KEY, RegistryKeyPermissionCheck.ReadWriteSubTree) ??
                Registry.CurrentUser.CreateSubKey(BROWSER_EMULATION_KEY);

            if (browserEmulationKey != null)
            {
                browserEmulationKey.SetValue(appname, browserEmulationMode, RegistryValueKind.DWord);
                browserEmulationKey.Close();
            }
        }

        public void redirectWebUI(string url)
        {
            redirectUrl = url;
           
            String myfile = Path.Combine(appdir, redirectUrl);
            if (webUI.IsBusy)
            {

                while (true)
                {
                    if (!webUI.IsBusy)
                    {
                        webUI.Navigate(new Uri(myfile));
                        redirectUrl = String.Empty; 
                        break; 
                    }
                }

            } else {

                webUI.Navigate(new Uri(myfile));
                redirectUrl = String.Empty;    
            }
                 
        }

        public static string redirectUrl;

        
    }
}
