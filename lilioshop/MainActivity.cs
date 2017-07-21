using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using Android.Util;
using Android.Content.PM;
using Android.Net;
using Android.Content.Res;
using Android.Graphics;

namespace lilioshop
{
    [Activity(Label = "LILIO", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar", ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    public class MainActivity : Activity

    {
        bool isOnline = false;
        ImageView imageView;
        WebView webView;        
        TextView text_View;
        Button button;
        ProgressBar progressBar1;


        static ImageView _imageView;
        static WebView _webView;
        static TextView _text_View;
        static Button _button;
        static ProgressBar _progressBar1;

        public static String tag;
         
        public void disconnected()
        {
           
            webView.Visibility = ViewStates.Gone;
            button.Visibility = ViewStates.Visible;
            text_View.Visibility = ViewStates.Visible;
        }
        public static void disconnectedScreen()
        {
            _imageView.Visibility = ViewStates.Visible;
            _webView.Visibility = ViewStates.Gone;
            _button.Visibility = ViewStates.Visible;
            _text_View.Visibility = ViewStates.Visible;
        }
        /* public void connected()
         {

             imageView.Visibility = ViewStates.Visible;
             button.Visibility = ViewStates.Gone;
             text_View.Visibility = ViewStates.Gone;
         }    */
         public void hideNoCon()
        {
            button.Visibility = ViewStates.Gone;
            text_View.Visibility = ViewStates.Gone;
        }
        public static void hideBanner()
        {
          _imageView.Visibility = ViewStates.Gone;
          _progressBar1.Visibility = ViewStates.Gone;
          

        }
        public void pageLoad()
        {
            
            var cookies = UserInfo.CookieContainer.GetCookies(new System.Uri("http://lilio.pl"));
            try
            {
                if (isConnectionOn())
                {
                    
                    webView.LoadUrl("http://www.lilio.pl");
                    hideNoCon();
                    //  MyWebViewClient.setCheck(isConnectionOn());
                    // disconnnectedScreen();
                    //Todo  not connected screen to be written and shown


                    var cookieManager = CookieManager.Instance;
                    cookieManager.SetAcceptCookie(true);


                    for (var i = 0; i < cookies.Count; i++)
                    {

                        string cookieValue = cookies[i].Value;
                        string cookieDomain = cookies[i].Domain;
                        string cookieName = cookies[i].Name;
                        cookieManager.SetCookie(cookieDomain, cookieName + "=" + cookieValue);
                    }
                }
                
            }
            catch
            {
                disconnected();
            }

        }
        [Android.Runtime.Register("onPause", "()V", "GetOnPauseHandler")]
        protected override void OnPause()
        {
            base.OnPause();
            Log.Info(tag, "asdasdasdasdasdasdsdasddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
        }
        [Android.Runtime.Register("onResume", "()V", "GetOnResumeHandler")]
        protected override void OnResume() {
            base.OnResume();
            pageLoad();
            Log.Info(tag, "TUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEMTUJESTEM");
                
        }

       


        public class UserInfo
        {
            public static CookieContainer CookieContainer = new CookieContainer();
        }


        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);
            _imageView = imageView;
            
            webView = FindViewById<WebView>(Resource.Id.webView);
            button = FindViewById<Button>(Resource.Id.button);
            text_View = FindViewById<TextView>(Resource.Id.text_view);
            progressBar1 = FindViewById<ProgressBar>(Resource.Id.progressBar1);

            progressBar1.IndeterminateDrawable.SetColorFilter(Color.ParseColor("#63064c"), Android.Graphics.PorterDuff.Mode.Multiply);

            _progressBar1 = progressBar1;
            _webView = webView;
            _button = button;
            _text_View = text_View;
          






            webView.SetWebViewClient(new MyWebViewClient());

            WebSettings webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;
            webSettings.SetPluginState(WebSettings.PluginState.On);

            //  pageLoad();

            //    MyWebViewClient.setIsOnline(isConnectionOn());

            button.Click += delegate { buttonClick(); };
        }


        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back && webView.CanGoBack())
            {
                webView.GoBack();
                return true;
            }
            return base.OnKeyDown(keyCode, e);
        }

        public bool isConnectionOn()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
            isOnline = networkInfo.IsConnected;
            
            return isOnline;
        }
        
        public void buttonClick()
        {
            Intent intent = new Intent(Android.Provider.Settings.ActionSettings);
            StartActivity(intent);
        }
        /*
        public void buttonClick()
        {
            
            
            button.Click += (sender, e) => {
                Log.Info(tag, "testtesttesttesttesttesttesttesttesttesttesttesttesttest");
                Intent intent = new Intent(Android.Provider.Settings.ActionManageApplicationsSettings);
                StartActivity(intent);
            };
        }
        */

    }

  
    // NEW CLASS MYWEBVIEWCLINET //////////////////////////////////////////////////////////////////////////////////////////////

    public class MyWebViewClient : WebViewClient
    {
      static  bool check = false;
        static bool isOnline = false;
       
        public bool WebSiteIsAvailable(string url)
        {
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    myHttpWebResponse.Close();
                    
                    isOnline = true;
                }
            }
            
            catch (WebException ex)
            {
                isOnline = false;
            }

            return isOnline;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {

            if (url.Contains("google"))
                view.Settings.UserAgentString = "ua-string";
            else view.Settings.UserAgentString = "";
            if (!url.Contains("youtube"))
                view.LoadUrl(url);
            return true;
            
        }

        public override void OnPageStarted(WebView view, string url, Android.Graphics.Bitmap favicon)
        {
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);
            
            view.Visibility = ViewStates.Visible;
            MainActivity.hideBanner();
            check = true;
            



        }

        public static bool getCheck()
        {
            return check;
        }
      public static void setIsOnline(bool x)
        {
            isOnline = x;
        }


    }




}