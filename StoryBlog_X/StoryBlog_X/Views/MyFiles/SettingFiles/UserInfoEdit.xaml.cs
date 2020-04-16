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
    public partial class UserInfoEdit : ContentPage
    {

        private static string judge;

        public UserInfoEdit()
        {

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

        }

        public UserInfoEdit(string para)
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            judge = para;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            switch (judge)
            {
                case "NickName":
                    this.lblUIEBars.Text = "更改名字";
                    this.edtString.Text = OptionText_Helper.ReadAllText("NickName");
                    break;
                case "Phone":
                    this.lblUIEBars.Text = "更改电话号码";
                    this.edtString.Keyboard = Keyboard.Telephone;
                    this.edtString.Text = OptionText_Helper.ReadAllText("Phone");
                    break;
                case "Introduce":
                    this.lblUIEBars.Text = "更改自我介绍";
                    this.edtString.Text = OptionText_Helper.ReadAllText("Introduce");
                    break;
                case "Describe":
                    this.lblUIEBars.Text = "更改自我描述";
                    this.edtString.Text = OptionText_Helper.ReadAllText("Describe");
                    break;
                case "Birthday":
                    this.lblUIEBars.Text = "更改出生日期";
                    this.edtString.IsVisible = false;
                    this.DPBirthday.IsVisible = true;
                    string Birthday = OptionText_Helper.ReadAllText("Birthday");
                    if (Birthday != "")
                    {
                        this.DPBirthday.Date = Convert.ToDateTime(Birthday);
                    }
                    else
                    {
                        this.DPBirthday.Date = DateTime.Now;
                    }
                    break;
                default:
                    break;
            }

            this.btnSave.BackgroundColor = System.Drawing.Color.FromArgb(33, 150, 243);
            this.btnSave.IsEnabled = false;

        }
        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            Models.UserInfo ui = new Models.UserInfo();
            #region
            ui.Account = OptionText_Helper.ReadAllText("Account");
            ui.Guid = OptionText_Helper.ReadAllText("Guid");
            ui.NickName = OptionText_Helper.ReadAllText("NickName");
            ui.Phone = OptionText_Helper.ReadAllText("Phone");
            ui.Describe = OptionText_Helper.ReadAllText("Describe");
            ui.Introduce = OptionText_Helper.ReadAllText("Introduce");
            string Birthday = OptionText_Helper.ReadAllText("Birthday");

            if (Birthday != "")
            {
                ui.Birthday = Convert.ToDateTime(Birthday);
            }
            else
            {
                ui.Birthday = DateTime.Now;
            }

            #endregion

            List<Models.UserInfo> list = new List<Models.UserInfo>();

            string url = $"/{Version_Helper.versionNumber}/user_/update?Option={judge}&Account={ui.Account}";

            switch (judge)
            {
                case "NickName":
                    ui.NickName = this.edtString.Text;
                    break;
                case "Phone":
                    ui.Phone = this.edtString.Text;
                    break;
                case "Introduce":
                    ui.Introduce = this.edtString.Text;
                    break;
                case "Describe":
                    ui.Describe = this.edtString.Text;
                    break;
                case "Birthday":
                    ui.Birthday = this.DPBirthday.Date;
                    ui.Age = DateTime.Now.Year - this.DPBirthday.Date.Year;
                    this.edtString.Text = this.DPBirthday.Date.ToString("yyyy/MM/dd");
                    break;
            }

            list.Add(ui);

            var result = await WebApiService_Helper.PostConnectHelperAsync(url, list);

            if (result[0].Flag)
            {
                //OptionText_Helper.WriteAllText(list[0]);

                OptionText_Helper.WriteText(judge, this.edtString.Text);

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("提示！", "保存失败！", "确认");
            }

        }

        private void EdtString_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.btnSave.BackgroundColor = System.Drawing.Color.YellowGreen;
            this.btnSave.IsEnabled = true;
        }

        private void DPBirthday_DateSelected(object sender, DateChangedEventArgs e)
        {
            this.btnSave.BackgroundColor = System.Drawing.Color.Pink;
            this.btnSave.IsEnabled = true;
        }

    }
}