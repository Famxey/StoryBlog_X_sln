using StoryBlog_X.HelperCls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Publish : ContentPage
	{
		public Publish ()
		{
			InitializeComponent ();
		}

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            string Account = OptionText_Helper.ReadAllText("Account");

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingPublish(" + "\"" + Account + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            WebView_Publish.Source = htmlSource;
        }

        private void WebView_Publish_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }

        private void WebView_Publish_Navigating(object sender, WebNavigatingEventArgs e)
        {
             if (e.Url.Contains("Blogs.html"))
            {
                
            }
            else if (e.Url.Contains("Pictures.html"))
            {
                
            }


        }

       
    }
}