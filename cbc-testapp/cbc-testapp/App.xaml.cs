using cbc_testapp.Services;
using cbc_testapp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace cbc_testapp
{
    public partial class App : Application
    {
        public static bool IsDebugMode = true;

        public static bool IsUserLoggedIn
        {
            get
            {
                bool finalValue = false;
                if (Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
                {
                    string value = Application.Current.Properties["IsUserLoggedIn"]?.ToString();
                    if (string.IsNullOrEmpty(value) == false && bool.TryParse(value, out finalValue))
                    {
                        return finalValue;
                    }
                }

                return finalValue;

            }
            set { Application.Current.Properties["IsUserLoggedIn"] = value; }
        }

        public static string UserSessionId
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("UserSessionId"))
                {
                    return Application.Current.Properties["UserSessionId"]?.ToString();

                }
                return null;
            }
            set { Application.Current.Properties["UserSessionId"] = value; }
        }
        public static string UserId
        {
            get
            {
                if (Application.Current.Properties.ContainsKey("UserId"))
                {
                    return Application.Current.Properties["UserId"]?.ToString();
                }
                return null;
            }
            set { Application.Current.Properties["UserId"] = value; }
        }

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<WebAPIService>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
