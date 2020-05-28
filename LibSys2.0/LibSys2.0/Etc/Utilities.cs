using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LibrarySystem.Etc
{
    public static class Utilities
    {
        /// <summary>
        /// Check if provied string is a valid email adress
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();
                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Generate valid [A-Za-z0-9] link URL
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateLinkUrl(int Length)
        {
            string str = "";
            // 48-75    0-9
            // 65-90    A-Z
            // 97-122   a-z
            int[] codes = new int[] { 48,49,50,51,52,53,54,55,56,57,
                65,66,67,68,69,70,71,72,73,74,75,76,76,78,79,80,81,82,83,84,85,86,87,88,89,90,
                97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121,122};

            Random r = new Random();

            for (int j = 0; j < Length; j++)
            {
                // Get one of the specified cahracter codes
                int code = codes[r.Next(0, codes.Length)];
                str += (char)code;
            }

            return str;
        }

        /// <summary>
        /// Validate URL is kind of correkt
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool UrlChecker(string url)
        {
            Uri uriResult;
            bool tryCreateResult = Uri.TryCreate(url, UriKind.Absolute, out uriResult);
            if (tryCreateResult == true && uriResult != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Random date
        /// </summary>
        /// <returns></returns>
        public static DateTime RandomDate()
        {
            Random gen = new Random();
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }
}
