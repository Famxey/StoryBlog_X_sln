using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {

        public MainPage()
        {
            InitializeComponent();
            //去除顶部导航栏
            NavigationPage.SetHasNavigationBar(this, false);
        }

        public MainPage(string para)
        {
            InitializeComponent();
            //去除顶部导航栏
            NavigationPage.SetHasNavigationBar(this, false);

            //if (para == "SettingPage")
            //{
            //    //CurrentPage设置选项卡选择页面
            //    this.CurrentPage = Children[4];
            //}
        }

    }
}