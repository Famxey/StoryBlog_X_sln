using System;
using Xamarin.Forms;
using StoryBlog_X.Droid;
using StoryBlog_X.HelperCls;

[assembly: Dependency (typeof (BaseUrl_Android))]
namespace StoryBlog_X.Droid
{
	public class BaseUrl_Android : IBaseUrl 
	{
		public string Get () 
		{
			return "file:///android_asset/";
		}
	}
}