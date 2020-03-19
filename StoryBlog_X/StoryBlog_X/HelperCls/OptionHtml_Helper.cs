using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StoryBlog_X.HelperCls
{
    public class OptionHtml_Helper
    {
        public static readonly string fileName
                       = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Index.txt");

        public static bool WriteHtml()
        {

            try
            {
                bool doesExist = File.Exists(fileName);

                if (!doesExist)
                {
                    string html = @"<html>
                                    <head>
                                    <script src=""JavaScript.js""></script>
                                    </head>
                                    <body onload=""loading()"">                               
                                    </body>
                                    </html>";
                    File.WriteAllText(fileName, html.Trim());
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static string ReadHtml()
        {
            return File.ReadAllText(fileName);
        }


    }
}
