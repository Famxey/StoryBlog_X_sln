using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoryBlog_X.Models
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(InfiniteListView));
        public static readonly BindableProperty CanLoadMoreProperty = BindableProperty.Create(nameof(CanLoadMore), typeof(bool), typeof(InfiniteListView), true);

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public bool CanLoadMore
        {
            get
            {
                return (bool)GetValue(CanLoadMoreProperty);
            }
            set
            {
                SetValue(CanLoadMoreProperty, value);
            }
        }

        public InfiniteListView()
        {
            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (!CanLoadMore)
            {
                return;
            }
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }
    }
}
