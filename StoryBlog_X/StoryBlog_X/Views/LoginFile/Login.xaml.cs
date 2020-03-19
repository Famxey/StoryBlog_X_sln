using StoryBlog_X.Models;
using StoryBlog_X.HelperCls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.LoginFile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public ObservableCollection<UserInfo> TopSeries { get; set; }
        private static string judge;

        public Login()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

        }

        public Login(string para)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            judge = para;

        }

        async private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (this.etyUserName.Text == "" || this.etyUserName.Text == null ||
                this.etyPassWord.Text == "" || this.etyPassWord.Text == null)
            {
                this.lblTips.IsVisible = true;
                this.lblTips.Text = "账号或密码不能为空！";
                return;
            }


            UserInfo u = new UserInfo();
            u.Account = this.etyUserName.Text;
            u.PassWord = this.etyPassWord.Text;

            List<UserInfo> list = new List<UserInfo>();
            list.Add(u);

            string url = $"/{Version_Helper.versionNumber}/login/verification";

            var result = await WebApiService_Helper.PostConnectHelperAsync(url, list);

            TopSeries = new ObservableCollection<UserInfo>(result);

            Debug.WriteLine(TopSeries[0].Flag);

            if (TopSeries[0].Flag)
            {
                OptionText_Helper.WriteAllText(result[0]);

                switch (judge)
                {
                    case "SettingPage":
                        await Navigation.PopAsync();
                        //await Navigation.PushAsync(new MainPage(judge));
                        break;
                    default:
                        await Navigation.PushAsync(new MainPage(""));
                        break;
                }

            }

        }
    }
}