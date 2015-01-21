using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Modelos;
//using System.Web.Mail;

namespace ws_SimpleMenu.classes
{
    public class Mailer
    {
        static UserContext db = new UserContext();

        public static bool send_welcome_mail(string mail)
        {
            string subject = "Bienvenido a la Fake Application";
            string body = "Hola, bienvenido a bla bla bla bla bla bla bla bla";
            return send_mail(mail, subject, body);
        }

        public static bool send_link_verification(string mail, string token)
        {
            try
            {
                string subject = "Link de verificacion para tu cuenta";
                string link = "http://localhost:10380/Verification/link_verification?secure_token=" + token;
                string body = db.Messages.Where(x => x.clave.Contains("register")).SingleOrDefault().body + link;
                return send_mail(mail, subject, body);
            }
            catch {
                return false;
            }
        }

        private static bool send_mail(string mail, string subject, string body)
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
                client.Send(from_email, mail, subject, body);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}