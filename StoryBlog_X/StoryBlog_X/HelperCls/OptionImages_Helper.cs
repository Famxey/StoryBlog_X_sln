using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace StoryBlog_X.HelperCls
{
    public class OptionImages_Helper : Page
    {

        public async Task<MediaFile> TakeImageAction()
        {
            string action = await DisplayActionSheet(null, "取消", null, "拍照", "从相册选择");

            MediaFile pickFile = null;

            switch (action)
            {
                case "拍照":
                    //判断能否使用摄像头
                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert("错误提示", "未找到可用摄像头", "确认");
                    }

                    var filea = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                        //拍照后的文件夹名
                        Directory = "SjBIdrPics",
                        //拍照后的文件名
                        Name = "SjBIdr.jpg"
                    });

                    if (filea == null)
                        return null;

                    pickFile = filea;

                    break;
                case "从相册选择":
                    //判断能否读取图片
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("错误提示", "未获得权限读取相册", "确认");
                    }

                    //选择图片
                    var fileb = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                    });
                    if (fileb == null)
                        return null;
                    pickFile = fileb;
                    break;
                case "取消":
                    return pickFile;
                case null:
                    return pickFile;
            }

            return pickFile;
        }

        public static FileStream CopyImageGetStream(byte[] data)
        {

            string rdFileName = Guid.NewGuid().ToString().Substring(0, 6) + OptionText_Helper.ReadAllText("Account") + ".jpeg";

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), rdFileName);

            File.WriteAllBytes(filePath, data);

            FileStream inStream = new FileStream(filePath, FileMode.Open);

            return inStream;

        }

    }
}
