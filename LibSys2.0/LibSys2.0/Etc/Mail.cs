using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LibrarySystem.Etc
{
    /// <summary>
    /// Use this for sending email
    /// </summary>
    public class Mail
    {
        private string username = "libsys@jkb.zone";
        private string password = "hunter12";
        private MailMessage Message = new MailMessage();
        private SmtpClient Client = new SmtpClient("ns12.inleed.net", 587);

        private string emailBody = "no body set";

        /// <summary>
        /// Constructor just to initialize some private variables
        /// </summary>
        public Mail(){}

        /// <summary>
        /// Send an email to specified address
        /// </summary>
        /// <param name="email"></param>
        public void SendTestEmail(string email)
        {
            Message.From = new MailAddress(username);
            Message.To.Add(new MailAddress(email));
            Message.Subject = "Välkommen till libsys";
            Message.IsBodyHtml = true;
            Message.Body = $"<table><tbody><tr><td>{" test email "}</td></tr></tbody></table>";
            Send();
        }

        /// <summary>
        /// Send activation email to adress http://tjackobacco.com:23080/libsys/activate?key={activation_code} to verify new users' email
        /// </summary>
        /// <param name="email">users' email adress</param>
        /// <param name="urlLink">link activation code</param>
        public void SendActivationEmail(string email, string urlLink)
        {
            // Hämta .html-filen från inuti projektet
            string imagePath = "pack://application:,,,/Assets/email-template-register.html";
            var fileLines = new System.Collections.Generic.List<string>();
            string htmlString = "";
            string line;

            // Some stream stuff since we are not 100% of a common file path
            System.Windows.Resources.StreamResourceInfo info = System.Windows.Application.GetResourceStream(new Uri(imagePath));
            System.IO.StreamReader sr = new System.IO.StreamReader(info.Stream);
            
            // Parse into string
            while ((line = sr.ReadLine()) != null)
                fileLines.Add(line);
            sr.Close();
            fileLines.ForEach(a => htmlString += a);

            // Replacea adressen med korrekt aktiveringskod
            htmlString = htmlString.Replace("[[link]]", string.Format("http://tjackobacco.com:23080/libsys/activate?key={0}", urlLink));

            Message.From = new MailAddress(username);
            Message.To.Add(new MailAddress(email));
            Message.Subject = "Välkommen till libsys";
            Message.IsBodyHtml = true;
            // Shove in the HTML for a pretty looking email
            Message.Body = htmlString;
            Send();
        }

        /// <summary>
        /// Private, with information from class properties
        /// </summary>
        private void Send()
        {
            Client.EnableSsl = true;
            Client.UseDefaultCredentials = false;
            Client.Credentials = new NetworkCredential(username, password);
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.Send(Message);
        }
    }
}
