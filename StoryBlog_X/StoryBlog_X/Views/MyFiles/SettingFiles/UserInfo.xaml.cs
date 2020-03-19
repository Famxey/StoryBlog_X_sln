using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoryBlog_X.HelperCls;
using StoryBlog_X.Models;
using Plugin.Media.Abstractions;

namespace StoryBlog_X.Views.MyFiles.SettingFiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfo : ContentPage
    {
        public UserInfo()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            #region
            string url = WebApiService_Helper.HttpBaseAddress + OptionText_Helper.ReadAllText("Picture");

            string NickName = OptionText_Helper.ReadAllText("NickName");

            string Gender = OptionText_Helper.ReadAllText("Gender");

            string Age = OptionText_Helper.ReadAllText("Age");

            string Birthday = OptionText_Helper.ReadAllText("Birthday");

            string Phone = OptionText_Helper.ReadAllText("Phone");

            string Introduce = OptionText_Helper.ReadAllText("Introduce");

            string Describe = OptionText_Helper.ReadAllText("Describe");

            this.imgUser.Source = url;
            this.lblNickName.Text = NickName.Trim();
            #endregion


            if (Gender != null)
            {
                this.lblGender.Text = Gender.Trim();
            }
            else
            {
                this.lblGender.Text = "未设置";
            }


            if (Age != "" && Age != "0")
                this.lblAge.Text = Age.ToString();
            else
                this.lblAge.Text = (DateTime.Now.Year - Convert.ToDateTime(Birthday).Year).ToString();

            this.lblBirthday.Text = Convert.ToDateTime(Birthday).ToString("yyyy/MM/dd");
            this.lblPhone.Text = Phone;
            this.lblIntroduce.Text = Introduce;
            this.lblDescribe.Text = Describe;

        }

        private async void VcUserImg_Tapped(object sender, EventArgs e)
        {

            OptionImages_Helper Option = new OptionImages_Helper();

            MediaFile mfile = await Option.TakeImageAction();

            if (mfile == null)
            {
                return;
            }
            await Navigation.PushAsync(new SettingFiles.UploadHeadPicture(mfile.GetStream(), mfile.Path));

        }


        private async void VcNickName_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UserInfoEdit("NickName"));
        }

        private async void VcGender_Tapped(object sender, EventArgs e)
        {
            string Gender = await DisplayActionSheet("请选择您的性别:", "取消", null, "男", "女", "保密");

            if (Gender != "取消" && Gender != null)
            {
                this.lblGender.Text = Gender;

                Models.UserInfo ui = new Models.UserInfo();
                ui.Account = OptionText_Helper.ReadAllText("Account");
                ui.Gender = Gender;

                List<Models.UserInfo> list = new List<Models.UserInfo>();
                string url = $"/{Version_Helper.versionNumber}/user_/update?Option=Gender&Account={ui.Account}";

                list.Add(ui);

                var result = await WebApiService_Helper.PostConnectHelperAsync(url, list);

                if (result[0].Flag)
                {
                    OptionText_Helper.WriteText("Gender", Gender);
                }
                else
                {
                    await DisplayAlert("提示！", "保存失败！", "确认");
                }

            }

        }


        private async void VcBirthday_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UserInfoEdit("Birthday"));
        }

        private async void VcPhone_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UserInfoEdit("Phone"));
        }


        private async void VcIntroduce_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UserInfoEdit("Introduce"));
        }

        private async void VcDescribe_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingFiles.UserInfoEdit("Describe"));
        }


        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}