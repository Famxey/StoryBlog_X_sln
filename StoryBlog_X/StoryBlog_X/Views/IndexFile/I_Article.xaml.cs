using StoryBlog_X.Models;
using StoryBlog_X.HelperCls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.IndexFile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class I_Article : ContentPage
    {
        private ObservableCollection<ArticleInfo> TopSeries { get; set; }

        private static readonly int count = 10;

        public I_Article()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 页面加载呈现事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void LoadingEvent(object sender, EventArgs args)
        {
            int page = 1;
            int i = 1;
            TopSeries = await GetListViewData(page, count);
            //为标题热度排行值
            foreach (var item in TopSeries) 
                item.TopNumber = i++;
            this.art_listView.ItemsSource = TopSeries;
            
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

        private async Task<ObservableCollection<ArticleInfo>> GetListViewData(int page, int count)
        {
            string url = $"/{Version_Helper.versionNumber}/article_/hot?page={page}&count={count}";

            var result = await WebApiService_Helper.GetConnectHelperAsync2<List<ArticleInfo>>(url);
            return new ObservableCollection<ArticleInfo>(result);
        }

        private static IList<ArticleInfo> items = new List<ArticleInfo>();

        private void Art_listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

            if (this.btnLoadMore.Text == "没有更多数据了！")
            {
                return;
            }

            ArticleInfo art = e.Item as ArticleInfo;

            items.Add(art);

            if (items != null && items.Count == count && e.Item == items[items.Count - 1])
            {

                this.skltLoadMore.IsVisible = true;

                items.Clear();
            }

        }

        private void Art_listView_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;

            //处理完关闭刷新状态
            list.IsRefreshing = false;
        }

        private void Art_listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //强制转换成要获取数据的类
            ArticleInfo art = e.Item as ArticleInfo;
            DisplayAlert("已点击：", art.artNo.ToString(), "确定");
        }

        private async void BtnLoadMore_Clicked(object sender, EventArgs e)
        {
            if (this.btnLoadMore.Text == "点击隐藏！")
            {
                this.skltLoadMore.IsVisible = false;
                this.btnShrink.IsVisible = true;
                return;
            }

            if (this.btnLoadMore.Text == "没有更多数据了！")
            {
                this.btnLoadMore.Text = "点击隐藏！";
                return;
            }

            LoadingStart();

            double itemNumber = TopSeries.Count / count + 1;

            int page = (int)Math.Ceiling(itemNumber);


            ObservableCollection<ArticleInfo> Series = await GetListViewData(page, count);


            int i = TopSeries.Count + 1;
            foreach (var item in Series)
            {
                item.TopNumber = i++;
                TopSeries.Add(item);
            }

            this.art_listView.ItemsSource = TopSeries;

            art_listView.ScrollTo(TopSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);

            LoadingEnd();

            if (Series.Count < count)
            {
                this.btnLoadMore.Text = "没有更多数据了！";

            }

        }

        private void BtnReturnTop_Clicked(object sender, EventArgs e)
        {

            art_listView.ScrollTo(TopSeries.FirstOrDefault(), position: ScrollToPosition.Start, animated: true);
        }

        private void BtnReturnBottom_Clicked(object sender, EventArgs e)
        {
            art_listView.ScrollTo(TopSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);
        }

        private void BtnShrink_Clicked(object sender, EventArgs e)
        {
            this.skltLoadMore.IsVisible = true;
            this.btnShrink.IsVisible = false;
        }
    }
}