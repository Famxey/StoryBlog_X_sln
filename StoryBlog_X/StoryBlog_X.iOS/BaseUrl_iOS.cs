using Xamarin.Forms;
using StoryBlog_X.iOS;
using Foundation;
using StoryBlog_X.HelperCls;

[assembly: Dependency (typeof (BaseUrl_iOS))]

namespace StoryBlog_X.iOS
{
	public class BaseUrl_iOS : IBaseUrl 
	{
		public string Get () 
		{
			return NSBundle.MainBundle.BundlePath;
		}
	}
}