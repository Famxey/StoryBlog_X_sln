using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoryBlog_X.Models;
using StoryBlog_X.HelperCls;
using System.Collections.ObjectModel;

namespace StoryBlog_X.Views.IndexFile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class I_Picture : ContentPage
	{
        private ObservableCollection<PictureInfo> TopSeries { get; set; }
        private static readonly int count=10;

        public I_Picture ()
		{
			InitializeComponent ();
		}

        /// <summary>
        /// 页面加载呈现事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void LoadingEvent(object sender, EventArgs args)
        {

            int page = 1;
            TopSeries = await GetListViewData(page, count);

            foreach (var item in TopSeries)
                item.ImgFile = WebApiService_Helper.HttpBaseAddress+ item.ImgFile;
            this.pic_listView.ItemsSource = TopSeries;
        }

        private async Task<ObservableCollection<PictureInfo>> GetListViewData(int page, int count)
        {
            string url = $"/{Version_Helper.versionNumber}/picture_/hot?page={page}&count={count}";

            var result = await WebApiService_Helper.GetConnectHelperAsync2<List<PictureInfo>>(url);
            return new ObservableCollection<PictureInfo>(result);
        }

        private void LoadingStart()
        {
            this.skltLoadMore.IsVisible = false;
            this.skltLoading.IsVisible = true;
        }
        private async void LoadingEnd()
        {
            this.actLoading.Progress = .3;
            await this.actLoading.ProgressTo(1, 1000, Easing.Linear);

            this.skltLoading.IsVisible = false;
            this.skltLoadMore.IsVisible = true;

        }

        private static IList<PictureInfo> items = new List<PictureInfo>();

        private void Pic_listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (this.btnLoadMore.Text == "没有更多数据了！")
            {
                return;
            }

            PictureInfo art = e.Item as PictureInfo;

            items.Add(art);

            if (items != null && items.Count == count && e.Item == items[items.Count - 1])
            {

                this.skltLoadMore.IsVisible = true;

                items.Clear();
            }
        }

        private void Pic_listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //强制转换成要获取数据的类
            PictureInfo art = e.Item as PictureInfo;
            DisplayAlert("已点击：", art.ID.ToString(), "确定");
        }

        private void Pic_listView_Refreshing(object sender, EventArgs e)
        {
            var list = (ListView)sender;

            Debug.WriteLine(list);

            var itemList = TopSeries.Reverse().ToList();
            TopSeries.Clear();
            Debug.WriteLine(itemList);
            foreach (var s in itemList)
            {

                TopSeries.Add(s);
            }
            //处理完关闭刷新状态
            list.IsRefreshing = false;
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

            ObservableCollection<PictureInfo> Series = await GetListViewData(page, count);

            foreach (var item in TopSeries)
                item.ImgFile = WebApiService_Helper.HttpBaseAddress + item.ImgFile;

            this.pic_listView.ItemsSource = TopSeries;

            pic_listView.ScrollTo(TopSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);

            LoadingEnd();

            if (Series.Count < count)
            {
                this.btnLoadMore.Text = "没有更多数据了！";

            }

        }

        private void BtnReturnTop_Clicked(object sender, EventArgs e)
        {

            pic_listView.ScrollTo(TopSeries.FirstOrDefault(), position: ScrollToPosition.Start, animated: true);
        }

        private void BtnReturnBottom_Clicked(object sender, EventArgs e)
        {
            pic_listView.ScrollTo(TopSeries.LastOrDefault(), position: ScrollToPosition.End, animated: true);
        }

        private void BtnShrink_Clicked(object sender, EventArgs e)
        {
            this.skltLoadMore.IsVisible = true;
            this.btnShrink.IsVisible = false;
        }

    }
}