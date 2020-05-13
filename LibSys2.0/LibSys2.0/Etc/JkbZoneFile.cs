using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Etc
{
    public class JkbZoneFile
    {
        public int UnixTimestamp { get; set; }
        public string Filename { get; set; }
        public string RandomString { get; set; }
        public string FileExtension { get; set; }
        public DateTime CreatedAt { get => DateTimeOffset.FromUnixTimeSeconds(UnixTimestamp).DateTime;}
        public string Location { get => String.Format("{0}{1}", "https://api.jkb.zone/file/", Filename); }

        public JkbZoneFile(string response)
        {
            SplitAndSet(response);
        }

        /// <summary>
        /// "UNIXTIMESTAMP.FILENAME.RANDOMSTRING.FILEEXTENSION"
        /// </summary>
        /// <param name="arg"></param>
        private void SplitAndSet(string arg)
        {
            // The incoming string is divided by dots.
            string[] parts = arg.Split('.');

            if (parts.Length < 4)
                throw new Exception("Too few parts");

            UnixTimestamp = Int32.Parse(parts[0]);
            Filename = parts[1];
            RandomString = parts[2];
            FileExtension = parts[3];
        }
    }
}
