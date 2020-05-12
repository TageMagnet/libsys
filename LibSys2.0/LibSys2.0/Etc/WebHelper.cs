using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;

namespace LibrarySystem.Etc
{
    /// <summary>
    /// Web requests etc. Gör saker över internet
    /// </summary>
    public static class WebHelper
    {
        public static async void UploadCoverImage()
        {
            string url = "http://api.jkb.zone/upload";
            string method = "POST";


            WebRequest request = WebRequest.Create(url);
            request.Method = method;

        }
    }
}
