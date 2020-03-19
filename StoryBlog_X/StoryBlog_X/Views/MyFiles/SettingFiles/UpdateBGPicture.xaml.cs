using SkiaSharp;
using SkiaSharp.Views.Forms;
using StoryBlog_X.HelperCls;
using StoryBlog_X.HelperCls.SkiaSharpCls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Views.MyFiles.SettingFiles
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateBGPicture : ContentPage
	{

        Stream getStream;

        PhotoCropperCanvasView photoCropper;

        SKBitmap croppedBitmap;

        public UpdateBGPicture(Stream stream, string path)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            if (stream != null)
            {
                getStream = stream;
            }

        }


        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            SKBitmap bitmap = SKBitmap.Decode(getStream);

            photoCropper = new PhotoCropperCanvasView(bitmap);

            canvasViewHost.Children.Add(photoCropper);
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            croppedBitmap = photoCropper.CroppedBitmap;

            SKCanvasView canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;

            PhotoCropperCanvasView Cropper = new PhotoCropperCanvasView(croppedBitmap);
            canvasViewHost.Children.Add(Cropper);

            if (this.btnSave.Text == "使用")
            {
                using (MemoryStream memStream = new MemoryStream())
                using (SKManagedWStream wstream = new SKManagedWStream(memStream))
                {
                    croppedBitmap.Encode(wstream, SKEncodedImageFormat.Jpeg, 100);

                    byte[] data = memStream.ToArray();

                    if (data == null)
                    {
                        await DisplayAlert("提示", "Encode returned null", "确认");
                    }
                    else if (data.Length == 0)
                    {
                        await DisplayAlert("提示", "Encode returned empty array", "确认");
                    }
                    else
                    {
                        #region
                        //await DisplayAlert("data提示", data.Length.ToString(), "确认");

                        string url = $"/{Version_Helper.versionNumber}/user_/update-bgpicture?Account={OptionText_Helper.ReadAllText("Account")}";

                        var content = await WebApiService_Helper.PostUpLoadImageHelperAsync(url, OptionImages_Helper.CopyImageGetStream(data));

                        if (content != "no")
                        {
                            OptionText_Helper.WriteText("BGPicture", content);
                            await DisplayAlert("成功提示", "图片已经修改成功！", "确认");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("错误提示", content, "确认");
                        }
                        #endregion

                    }
                }
            }
            this.btnSave.Text = "使用";

        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;

            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            canvas.DrawBitmap(croppedBitmap, info.Rect, BitmapStretch.Uniform);
        }



    }
}