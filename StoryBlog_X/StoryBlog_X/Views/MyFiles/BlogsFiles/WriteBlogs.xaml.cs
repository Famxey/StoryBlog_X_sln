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
    public partial class WriteBlogs : ContentPage
    {
        public WriteBlogs()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            string Account = OptionText_Helper.ReadAllText("Account");

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingBlogsWrite(" + "\"" + Account + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            artWebView_Write.Source = htmlSource;

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void ArtWebView_Write_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);
            this.skltLoading.IsVisible = false;
        }

        private async void ArtWebView_Write_Navigating(object sender, WebNavigatingEventArgs e)
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