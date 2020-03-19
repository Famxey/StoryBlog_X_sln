using StoryBlog_X.HelperCls;
using StoryBlog_X.Models;
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
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            this.etyAccount.TextChanged += EtyAccount_TextChanged;
            this.etyNickName.TextChanged += EtyNickName_TextChanged;
            this.etyPassWord.TextChanged += EtyPassWord_TextChanged;
            this.etyRePassWord.TextChanged += EtyRePassWord_TextChanged;
        }

        private void EtyRePassWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rePassWord = this.etyRePassWord.Text;
            if (rePassWord != this.etyPassWord.Text)
            {
                this.lblTipsRePassWord.Text = "两次密码不一致！";
                this.lblTipsRePassWord.IsVisible = true;
            }
            else
            {
                this.lblTipsRePassWord.IsVisible = false;
            }
        }

        private void EtyPassWord_TextChanged(object sender, TextChangedEventArgs e)
        {
            string passWord = this.etyPassWord.Text;
            if (!IsNatural_Number(passWord))
            {
                this.lblTipsPassWord.Text = "密码要英文或数字！";
                this.lblTipsPassWord.IsVisible = true;
            }
            else if (passWord.Length < 6)
            {
                this.lblTipsPassWord.Text = "账号字段不能少于6个！";
                this.lblTipsPassWord.IsVisible = true;
            }
            else if (passWord.Length > 16)
            {
                this.lblTipsPassWord.Text = "账号字段不能大于16个！";
                this.lblTipsPassWord.IsVisible = true;
            }
            else if (passWord.Length <= 16 && passWord.Length >= 6)
            {
                this.lblTipsPassWord.IsVisible = false;
            }

        }

        private void EtyNickName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nickName = this.etyNickName.Text;

            if (nickName.Length < 2)
            {
                this.lblTipsNickName.Text = "昵称数字不能少于2个！";
                this.lblTipsNickName.IsVisible = true;
            }
            else if (nickName.Length > 8)
            {
                this.lblTipsNickName.Text = "昵称数字不能大于8个！";
                this.lblTipsNickName.IsVisible = true;
            }
            else if (nickName.Length <= 8 && nickName.Length >= 2)
            {
                this.lblTipsNickName.IsVisible = false;
            }

        }

        async private void EtyAccount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string account = this.etyAccount.Text;

            if (!IsNatural_Number(account))
            {
                this.lblTipsAccount.Text = "账号要英文或数字！";
                this.lblTipsAccount.IsVisible = true;

            }
            else if (account.Length < 6)
            {
                this.lblTipsAccount.Text = "账号字段不能少于6个！";
                this.lblTipsAccount.IsVisible = true;
            }
            else if (account.Length > 16)
            {
                this.lblTipsAccount.Text = "账号字段不能大于16个！";
                this.lblTipsAccount.IsVisible = true;
            }
            else if (account.Length <= 16 && account.Length >= 6)
            {
                if (await AccountValid(account))
                {
                    this.lblTipsAccount.Text = "账号已经被占用，请换一个！";
                    this.lblTipsAccount.IsVisible = true;
                }
                else
                {
                    this.lblTipsAccount.IsVisible = false;
                }

            }

        }

        async private void BtnRegister_Clicked(object sender, EventArgs e)
        {
            string account = this.etyAccount.Text;
            string nickeName = this.etyNickName.Text;
            string passWord = this.etyPassWord.Text;
            string rePassWord = this.etyRePassWord.Text;

            bool Valid = this.lblTipsAccount.IsVisible || this.lblTipsNickName.IsVisible || this.lblTipsPassWord.IsVisible || this.lblTipsNickName.IsVisible;

            if (Valid)
            {
                return;
            }

            if (!EmptyValid(account, nickeName, passWord, rePassWord))
            {
                return;
            }

            UserInfo user = new UserInfo();
            user.Account = account;
            user.NickName = nickeName;
            user.PassWord = MD5_Helper.MD5Encrypt16(passWord);

            List<UserInfo> list = new List<UserInfo>();
            list.Add(user);

            string url = $"/{Version_Helper.versionNumber}/user_/add";

            var result = await WebApiService_Helper.PostConnectHelperAsync(url, list);

            ObservableCollection<UserInfo> Series = new ObservableCollection<UserInfo>(result);

            Debug.WriteLine(Series[0].Flag);

            if (Series[0].Flag)
            {
                await Navigation.PushAsync(new LoginFile.Login("register",account));
            }

        }


        /// <summary>
        /// 字段非空验证
        /// </summary>
        /// <param name="account"></param>
        /// <param name="nickeName"></param>
        /// <param name="passWord"></param>
        /// <param name="rePassWord"></param>
        /// <returns></returns>
        private bool EmptyValid(string account, string nickeName, string passWord, string rePassWord)
        {
            if (account == string.Empty || account == null)
            {
                this.lblTipsAccount.IsVisible = true;
                this.lblTipsAccount.Text = "账号不能为空！";
                return false;
            }
            else if (nickeName == string.Empty || nickeName == null)
            {
                this.lblTipsNickName.IsVisible = true;
                this.lblTipsNickName.Text = "昵称不能为空！";
                return false;
            }
            else if (passWord == string.Empty || passWord == null)
            {
                this.lblTipsPassWord.IsVisible = true;
                this.lblTipsPassWord.Text = "密码不能为空！";
                return false;
            }
            else if (rePassWord == string.Empty || rePassWord == null)
            {
                this.lblTipsRePassWord.IsVisible = true;
                this.lblTipsRePassWord.Text = "账号不能为空！";
                return false;
            }
            else
            {
                return true;
            }

        }

        async private Task<bool> AccountValid(string account)
        {

            string url = $"/{Version_Helper.versionNumber}/user_/account?account={account}";

            var result = await WebApiService_Helper.GetConnectHelperAsync2<List<FlagHelper>>(url);

            return result[0].Flag;
        }

        /// <summary>
        /// 判断字符串是否是英文或数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNatural_Number(string str)
        {
            System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z0-9]+$");
            return reg1.IsMatch(str);
        }

    }
}