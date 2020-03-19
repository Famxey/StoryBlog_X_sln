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
    public partial class ReadBlogs : ContentPage
    {
        private static string artNo_Account;

        private static string artNo_G;

        public ReadBlogs(string artNo)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            string Account = OptionText_Helper.ReadAllText("Account");

            artNo_Account = artNo + "+" + Account;

            artNo_G = artNo;
        }

        public ReadBlogs(string artNo,string random)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            string Account = OptionText_Helper.ReadAllText("Account");

            artNo_Account = artNo + "+" + Account;

            if (random== "random")
            {
                ImgBtnDeleteBlog.IsVisible = false;
                ImgBtnEditBlog.IsVisible = false;
            }

        }



        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            var htmlSource = new HtmlWebViewSource();

            //转义符号很重要，不然参数太长不识别
            string Html = "<html><head><script src='Initial.js'></script></head><body onload='loadingBlogsRead(" + "\"" + artNo_Account + "\"" + ")'></body></html>";

            htmlSource.Html = Html;

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            artWebV.Source = htmlSource;

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private async void ImgBtnEditBlog_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BlogsFiles.EditBlogs(artNo_G));
        }

        private async void ArtWebV_Navigated(object sender, WebNavigatedEventArgs e)
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);
            this.skltLoading.IsVisible = false;
        }

        private void ArtWebV_Navigating(object sender, WebNavigatingEventArgs e)
        {
            this.skltLoading.IsVisible = true;

        }

        private async void ImgBtnDeleteBlog_Clicked(object sender, EventArgs e)
        {

            bool answer = await DisplayAlert("提示：", "确定要删除！", "是", "否");

            if (answer)
            {
                string uAccount = OptionText_Helper.ReadAllText("Account");

                string url = $"/{Version_Helper.versionNumber}/article_/delete?uAccount={uAccount}&artNo={artNo_G}";

                string result = await WebApiService_Helper.PostConnectHelperAsync(url, "");

                Debug.WriteLine(result);

                if (result.Contains("true"))
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("提示:", "删除失败!", "是");
                }
            }

        }

    }
}