using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoryBlog_X.HelperCls;

namespace StoryBlog_X.Views.MyFiles
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Setting : ContentPage
	{
        public Setting()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            string Account = OptionText_Helper.ReadAllText("Account");

            string NickName = OptionText_Helper.ReadAllText("NickName");

            string url = WebApiService_Helper.HttpBaseAddress + OptionText_Helper.ReadAllText("Picture");

            if (Account == "" || NickName == "")
            {
                await Navigation.PushAsync(new LoginFile.Login("SettingPage"));
            }


            this.imgUser.Source = url;
            this.lblNickName.Text = "昵称：" + Account;
            this.lblAccount.Text = "账号：" + NickName;

        }

        private async void VcUserInfo_Tapped(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new SettingFiles.UserInfo());

        }

        private async void VcSafety_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UpdatePassword());
        }

        private async void VcSwitching_Tapped(object sender, EventArgs e)
        {
            bool flag = await DisplayAlert("提示！", "是否切换账号！", "确定", "取消");

            if (!flag)
                return;

            if (OptionText_Helper.DeleteAllText())
            {
                await Navigation.PushAsync(new LoginFile.Login("Switching"));
            }
        }

        private async void VcLogout_Tapped(object sender, EventArgs e)
        {
            bool flag = await DisplayAlert("提示！", "是否退出程序！", "确定", "取消");

            if (flag)
            {
                System.Environment.Exit(0);
            }


        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}