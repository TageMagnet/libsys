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

        public Mail(){}

        /// <summary>
        /// Send an email to specified address
        /// </summary>
        /// <param name="email"></param>
        public void SendTo(string email)
        {
            Message.From = new MailAddress(username);
            Message.To.Add(new MailAddress(email));

            Message.Subject = "Välkommen till libsys";
            Message.IsBodyHtml = true;
            Message.Body = $"<table><tbody><tr><td>{"Klicka på länken här [] för att registera dig "}</td></tr></tbody></table>";

            Client.EnableSsl = true;
            Client.UseDefaultCredentials = false;
            Client.Credentials = new NetworkCredential(username, password);
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.Send(Message);
        }
    }
}
