using StoryBlog_X.HelperCls;
using StoryBlog_X.UWP;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl))]
namespace StoryBlog_X.UWP
{
    public class BaseUrl : IBaseUrl
    {
        public string Get()
        {
            return "ms-appx-web:///WebFiles/";
        }
    }
}
