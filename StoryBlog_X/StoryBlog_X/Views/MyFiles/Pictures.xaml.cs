using StoryBlog_X.HelperCls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.MyFiles
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Pictures : ContentPage
	{
		public Pictures ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            string Account = OptionText_Helper.ReadAllText("Account");

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingPictures(" + "\"" + Account + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            WebView_Pictures.Source = htmlSource;

        }

        private async void WebView_Pictures_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);
            this.skltLoading.IsVisible = false;
        }

        private async void WebView_Pictures_Navigating(object sender, WebNavigatingEventArgs e)
        {
            this.skltLoading.IsVisible = true;

            Debug.WriteLine(e.Url);

            if (e.Url.Contains("Success.html"))
            {
                await Navigation.PushAsync(new MyFiles.Blogs());
            }
            else if(e.Url.Contains("Back.html"))
            {
                await Navigation.PopAsync();
            }

        }
    }
}