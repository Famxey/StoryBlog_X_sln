using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace StoryBlog_X.Models
{
   public class ConnectHelper : INotifyPropertyChanged
    {
        public bool Flag { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
