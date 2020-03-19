using StoryBlog_X.HelperCls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.MyFiles.BlogsFiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditBlogs : ContentPage
    {
        private static string artNo_G;
        public EditBlogs(string artNo)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            artNo_G = artNo;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingBlogsEdit(" + "\"" + artNo_G + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            artWebView_Edit.Source = htmlSource;

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void artWebView_Edit_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);
            this.skltLoading.IsVisible = false;
        }

        private async void artWebView_Edit_Navigating(object sender, WebNavigatingEventArgs e)
        {
            this.skltLoading.IsVisible = true;

            Debug.WriteLine(e.Url);
            if (e.Url.Contains("Success.html"))
            {
                await Navigation.PushAsync(new MyFiles.Blogs());
            }

        }

    }
}