using StoryBlog_X.Views;
using StoryBlog_X.Views.IndexFile;
using StoryBlog_X.Views.LoginFile;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.StyleSheets;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StoryBlog_X
{
    public partial class App : Application
    {

        public static double ScreenWidth;
        public static double ScreenHeight;
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage(""));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
