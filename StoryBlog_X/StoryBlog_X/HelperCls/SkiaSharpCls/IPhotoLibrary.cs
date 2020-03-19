using System;
using System.IO;
using System.Threading.Tasks;

namespace StoryBlog_X.HelperCls.SkiaSharpCls
{
    public interface IPhotoLibrary
    {
        Task<Stream> PickPhotoAsync();

        Task<bool> SavePhotoAsync(byte[] data, string folder, string filename);

        Task<string> SavePhotoGetPathAsync(byte[] data, string folder, string filename);

    }
}
