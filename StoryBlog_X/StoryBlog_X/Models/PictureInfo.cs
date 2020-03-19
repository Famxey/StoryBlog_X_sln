﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog_X.Models
{
    public class PictureInfo : ConnectHelper
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string picClsTitle { get; set; }
        public string ImgFile { get; set; }
        public System.DateTime picCreateTime { get; set; }
        public Nullable<int> picHot { get; set; }
        public int PicClsID { get; set; }
        public string uAccount { get; set; }
        public string picDescribe { get; set; }

        public string picCreateTimeA
        {
            get { return picCreateTime.ToString("yyyy/MM/dd/HH:mm:ss"); }
        }
    }
}
