using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoryBlog_X.Models;
using StoryBlog_X.HelperCls;

namespace StoryBlog_X.Views.HomeFiles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArticleRandom : ContentPage
    {
        public ArticleRandom()
        {
            InitializeComponent();
        }
        //从服务器请求到的随机数据集合,缓存起来
        private List<ArticleInfo> RandomSeries { get; set; }
        //取随机数据集合，用于显示列表的本地数据集合
        private ObservableCollection<ArticleInfo> StoreSeries { get; set; }
        //显示一列的数据组
        private static readonly int count = 10;
        //请求服务器的记录次数
        private static int Times = 1;

        private static int page;

        /// <summary>
        /// 页面加载呈现事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void LoadingEvent(object sender, EventArgs args)
        {
            this.skltLoadMore.IsVisible = false;
            this.btnShrink.IsVisible = false;

            string Account = OptionText_Helper.ReadAllText("Account");

            Times = 1;

            RandomSeries = await GetListViewData(Account, Times);

            StoreSeries = new ObservableCollection<ArticleInfo>(Paging(1, count, RandomSeries));

            this.art_listView.ItemsSource = StoreSeries;

            this.btnLoadMore.Text = "点击加载更多...";
        }

        private List<ArticleInfo> Paging(int page, int count, List<ArticleInfo> series)
        {

            return series.Skip((page - 1) * count).Take(count).ToList();
        }

        private void LoadingStart()
        {

            this.skltLoadMore.IsVisible = false;
            this.skltLoading.IsVisible = true;

            //this.actLoading.IsRunning = true;
        }

        private async void LoadingEnd()
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);

            this.skltLoading.IsVisible = false;
            this.skltLoadMore.IsVisible = true;

        }

        private async Task<List<ArticleInfo>> GetListViewData(string Account, int Times)
        {
            string url = $"/{Version_Helper.versionNumber}/article_/random?Account={Account}&Times={Times}";

            return await WebApiService_Helper.GetConnectHelperAsync2<List<ArticleInfo>>(url);
        }

        private void Art_listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

            if (this.btnLoadMore.Text == "没有更多数据了！")
            {
                return;
            }

            if (this.btnLoadMore.Text == "点击隐藏！")
            {
                return;
            }


            if (StoreSeries.Count!=0&& e.Item== StoreSeries[StoreSeries.Count - 1])
            {
                this.skltLoadMore.IsVisible = true;
            }         

        }

        private void Art_listView_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            LoadingEvent(sender, e);
            //处理完关闭刷新状态
            list.IsRefreshing = false;
        }

        private async void Art_listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //强制转换成要获取数据的类
            ArticleInfo art = e.Item as ArticleInfo;
            //DisplayAlert("已点击：", art.artNo.ToString(), "确定");
            await Navigation.PushAsync(new MyFiles.BlogsFiles.ReadBlogs(art.artNo.ToString(),"random"));

        }

        private async void BtnLoadMore_Clicked(object sender, EventArgs e)
        {
            if (this.btnLoadMore.Text == "点击隐藏！")
            {
                this.skltLoadMore.IsVisible = false;
                this.btnShrink.IsVisible = true;
                art_listView.ScrollTo(StoreSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);
                return;
            }

            if (this.btnLoadMore.Text == "没有更多数据了！")
            {
                this.btnLoadMore.Text = "点击隐藏！";
                art_listView.ScrollTo(StoreSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);
                return;
            }

            LoadingStart();

            double itemNumber = StoreSeries.Count / count + 1;

            page = (int)Math.Ceiling(itemNumber);

            StoreSeries = new ObservableCollection<ArticleInfo>(StoreSeries.Union(Paging(page, count, RandomSeries)));

            this.art_listView.ItemsSource = StoreSeries;


            if (Paging(page, count, RandomSeries).Count < count)
            {
                this.btnLoadMore.Text = "加载服务器数据中...";

                string Account = OptionText_Helper.ReadAllText("Account");

                Times++;

                var q = await GetListViewData(Account, Times);

                if (q.Count == 0)
                {
                    this.btnLoadMore.Text = "没有更多数据了！";

                    art_listView.ScrollTo(StoreSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);

                    LoadingEnd();
                    return;
                }

                foreach (var item in q)
                {
                    RandomSeries.Add(item);
                }

                this.btnLoadMore.Text = "点击加载更多...";

                BtnLoadMore_Clicked(sender, e);

            }

            art_listView.ScrollTo(StoreSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);

            LoadingEnd();

        }

        private void BtnReturnTop_Clicked(object sender, EventArgs e)
        {

            art_listView.ScrollTo(StoreSeries.FirstOrDefault(), position: ScrollToPosition.Start, animated: true);
        }

        private void BtnReturnBottom_Clicked(object sender, EventArgs e)
        {
            art_listView.ScrollTo(StoreSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);
        }

        private void BtnShrink_Clicked(object sender, EventArgs e)
        {
            this.skltLoadMore.IsVisible = true;
            this.btnShrink.IsVisible = false;
        }

        private void BtnRefresh_Clicked(object sender, EventArgs e)
        {
            LoadingEvent(sender, e);           
        }
    }
}