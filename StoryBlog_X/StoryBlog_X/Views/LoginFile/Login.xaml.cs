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

        public Login(string option)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            judge = option;

        }

        public Login(string option, string para)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            if (option == "register")
            {
                this.etyAccount.Text = para;
            }
            judge = option;

        }


        async private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            if (this.etyAccount.Text == "" || this.etyAccount.Text == null ||
                this.etyPassWord.Text == "" || this.etyPassWord.Text == null)
            {
                this.lblTips.IsVisible = true;
                this.lblTips.Text = "账号或密码不能为空！";
                return;
            }


            UserInfo user = new UserInfo();
            user.Account = this.etyAccount.Text;
            user.PassWord = MD5_Helper.MD5Encrypt16(this.etyPassWord.Text);

            List<UserInfo> list = new List<UserInfo>();
            list.Add(user);

            string url = $"/{Version_Helper.versionNumber}/user_/verification";

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
                    case "register":
                        await Navigation.PushAsync(new MainPage(""));
                        break;
                    default:
                        await Navigation.PushAsync(new MainPage(""));
                        break;
                }

            }

        }

        async private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginFile.Register());

        }

    }
}