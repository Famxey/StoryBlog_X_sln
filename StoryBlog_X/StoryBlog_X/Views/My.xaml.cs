using Plugin.Media.Abstractions;
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
    public partial class My : ContentPage
    {
        public My()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            string Account = OptionText_Helper.ReadAllText("Account");

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingMy(" + "\"" + Account + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            WebView_My.Source = htmlSource;

        }

        private async void ImgBtnSettings_Clicked()
        {
            await Navigation.PushAsync(new MyFiles.Setting());
        }

        private async void ImgBtnUserPicture_Clicked()
        {
            await Navigation.PushAsync(new MyFiles.SettingFiles.UserInfo());
        }

        private async void ImgBtnBGP_Clicked()
        {
            OptionImages_Helper Option = new OptionImages_Helper();

            MediaFile mfile = await Option.TakeImageAction();

            if (mfile == null)
            {
                return;
            }
            await Navigation.PushAsync(new MyFiles.SettingFiles.UpdateBGPicture(mfile.GetStream(), mfile.Path));

        }

        private async void ImgBtnBlogs_Clicked()
        {
            await Navigation.PushAsync(new MyFiles.Blogs());
        }

        private async void ImgBtnPictures_Clicked()
        {
            await Navigation.PushAsync(new MyFiles.Pictures());
        }

        private void WebView_My_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }

        private async void WebView_My_Navigating(object sender, WebNavigatingEventArgs e)
        {

            if (e.Url.Contains("BtnBGP.html"))
            {
                ImgBtnBGP_Clicked();
            }
            else if (e.Url.Contains("Settings.html"))
            {
                ImgBtnSettings_Clicked();
            }
            else if (e.Url.Contains("UserPic.html"))
            {
                ImgBtnUserPicture_Clicked();
            }
            else if (e.Url.Contains("Blogs.html"))
            {
                ImgBtnBlogs_Clicked();
            }
            else if (e.Url.Contains("Pictures.html"))
            {
                ImgBtnPictures_Clicked();
            }
            else if (e.Url.Contains("Back.html"))
            {
                await Navigation.PopAsync();
            }

        }
    }
}