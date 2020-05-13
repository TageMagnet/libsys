using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LibrarySystem.Etc
{
    /// <summary>
    /// Web requests etc. Gör saker över internet
    /// </summary>
    public static class WebHelper
    {
        public static async Task<JObject> UploadCoverImage(string filename)
        {
            // target URL
            string url = "http://api.jkb.zone/file";
            JObject joResponse;

            // Client initiated
            HttpClient httpClient = new HttpClient();
            // Form containing data
            MultipartFormDataContent form = new MultipartFormDataContent();

            // Some magic with the file
            byte[] file = System.IO.File.ReadAllBytes(filename);
            form.Add(new ByteArrayContent(file, 0, file.Length), "file", filename);
            form.Add(new StringContent("false"), "cover");
            // Sorry attempt at security
            form.Add(new StringContent("ijfgiouahfbnuivboaefh"), "upload_preset");

            // POST method
            HttpResponseMessage response = await httpClient.PostAsync(url, form);

            // Parse the response into string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Dynamic JSON object, alla respones from this server url are JSON
            joResponse = JObject.Parse(responseBody);

            // Return the hashed named of the uploaded file
            string location = joResponse.Value<string>("location");

            // Murder the variable
            httpClient.Dispose();

            return joResponse;
        }
    }
}
