using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Mail;

namespace ws_SimpleMenu.classes
{
    public class Mailer
    {
        public static bool send_mail(string mail)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/email.txt");
                string[] lines = System.IO.File.ReadAllLines(path);
                string from_email = lines.First();
                string password = lines.Last();
                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(from_email, password),
                    EnableSsl = true
                };
                client.Send(from_email, mail, "test", "testbody");
                return true;
            }
            catch {
                return false;
            }
        }
    }
}