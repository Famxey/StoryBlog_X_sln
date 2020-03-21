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

            if (!(this.lblTipsOldPwd.Text== "√"&& this.lblTipsNewPwd.Text == "√" && this.lblTipsReNewPwd.Text == "√"))
            {
                return;
            }

            List<Models.UserInfo> list = new List<Models.UserInfo>();

            Models.UserInfo ui = new Models.UserInfo();
            ui.Account = OptionText_Helper.ReadAllText("Account");
            ui.PassWord = MD5_Helper.MD5Encrypt16(this.etyNewPwd.Text);

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
                this.lblTipsOldPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsOldPwd.Text = "此项不能为空！";
                return;
            }

            if (MD5_Helper.MD5Encrypt16(OldPwd )!= PassWord)
            {
                this.lblTipsOldPwd.Text = "输入与原来的密码不一致！";
            }
            else
            {
                this.lblTipsOldPwd.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTipsOldPwd.Text = "√";
            }
        }

        private void EtyNewPwd_Unfocused(object sender, FocusEventArgs e)
        {
            string OldPwd = this.etyOldPwd.Text;

            string NewPwd = this.etyNewPwd.Text;

            if (NewPwd == null)
            {
                this.lblTipsNewPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsNewPwd.Text = "此项不能为空！";
                return;
            }

            if (NewPwd == OldPwd)
            {
                this.lblTipsNewPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsNewPwd.Text = "与原来的密码一致了！";
                return;
            }


            if (NewPwd.Length < 6)
            {
                this.lblTipsNewPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsNewPwd.Text = "输入的新密码长度不够（6—16）！";
            }
            else
            {
                this.lblTipsNewPwd.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTipsNewPwd.Text = "√";
            }
        }

        private void EtyReNewPwd_Unfocused(object sender, FocusEventArgs e)
        {
            string NewPwd = this.etyNewPwd.Text;
            string ReNewPwd = this.etyReNewPwd.Text;

            if (ReNewPwd == null)
            {
                this.lblTipsReNewPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsReNewPwd.Text = "此项不能为空！";
                return;
            }

            if (ReNewPwd != NewPwd)
            {
                this.lblTipsReNewPwd.TextColor = System.Drawing.Color.Red;
                this.lblTipsReNewPwd.Text = "输入与新的密码不一致！";
            }
            else
            {
                this.lblTipsReNewPwd.TextColor = System.Drawing.Color.YellowGreen;
                this.lblTipsReNewPwd.Text = "√";
                this.btnSave.BackgroundColor = System.Drawing.Color.YellowGreen;
                this.btnSave.IsEnabled = true;
            }

        }


    }
}