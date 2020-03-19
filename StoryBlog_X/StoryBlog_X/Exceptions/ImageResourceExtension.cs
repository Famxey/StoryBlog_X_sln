using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoryBlog_X.Exceptions
{
    [Preserve(AllMembers=true)]
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source);

            Debug.WriteLine(imageSource);

            return imageSource;

           

        }
    }
}
