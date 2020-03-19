using StoryBlog_X.Models;
using StoryBlog_X.HelperCls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.MyFiles.SettingFiles
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdatePassword : ContentPage
	{

        public UpdatePassword()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.btnSave.IsEnabled = false;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            Models.UserInfo ui = new Models.UserInfo();
            ui.Account = OptionText_Helper.ReadAllText("Account");
            ui.PassWord = this.etyNewPwd.Text.Trim();

            List<Models.UserInfo> list = new List<Models.UserInfo>();
            string url = $"/{Version_Helper.versionNumber}/user_/update?Option=PassWord&Account={ui.Account}";

            list.Add(ui);

            var result = await WebApiService_Helper.PostConnectHelperAsync(url, list);

            if (result[0].Flag)
            {
                OptionText_Helper.WriteText("PassWord", ui.PassWord);
                await DisplayAlert("提示！", "修改成功！", "确认");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("提示！", "修改失败！", "确认");
            }

        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }


        private void EtyOldPwd_Unfocused(object sender, FocusEventArgs e)
        {
            string OldPwd = this.etyOldPwd.Text;
            string PassWord = OptionText_Helper.ReadAllText("PassWord");

            if (OldPwd == null)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "此项不能为空！";
                return;
            }

            if (OldPwd != PassWord)
            {
                this.lblTips.Text = "输入与原来的密码不一致！";
            }
            else
            {
                this.lblTips.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTips.Text = "√";
            }
        }

        private void EtyNewPwd_Unfocused(object sender, FocusEventArgs e)
        {
            string OldPwd = this.etyOldPwd.Text;

            string NewPwd = this.etyNewPwd.Text;

            if (NewPwd == null)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "此项不能为空！";
                return;
            }

            if (NewPwd == OldPwd)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "与原来的密码一致了！";
                return;
            }


            if (NewPwd.Length < 6)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "输入的新密码长度不够（6—16）！";
            }
            else
            {
                this.lblTips.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTips.Text = "√√";
            }
        }

        private void EtyReNewPwd_Unfocused(object sender, FocusEventArgs e)
        {
            string NewPwd = this.etyNewPwd.Text;
            string ReNewPwd = this.etyReNewPwd.Text;

            if (ReNewPwd == null)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "此项不能为空！";
                return;
            }

            if (ReNewPwd != NewPwd)
            {
                this.lblTips.TextColor = System.Drawing.Color.Red;
                this.lblTips.Text = "输入与新的密码不一致！";
            }
            else
            {
                this.lblTips.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTips.Text = "√√√";
                this.btnSave.BackgroundColor = System.Drawing.Color.YellowGreen;
                this.btnSave.IsEnabled = true;
            }

        }


    }
}