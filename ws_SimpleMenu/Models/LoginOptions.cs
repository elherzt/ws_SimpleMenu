using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelos;
using System.Net;

namespace ws_SimpleMenu.Models
{
    public class LoginOptions
    {
        static UserContext db = new UserContext();
        public static Response Autenticar(string user, string password)
        {
            if (user.Contains("@"))
            {
                return login(user, password, 0);
            }
            else { 
                return login(user, password, 1);
            }
        }

        private static Response login(string us, string pass, int ban)
        {
            Response response = new Response();
            try
            {
                List<User> user = new List<User>();
                if (ban == 0)
                {
                    user = db.Users.Where(x => x.email == us).ToList();
                }
                else
                {
                    user = db.Users.Where(x => x.username == us).ToList();
                }
                if (user.Count() != 1)
                {
                    response.succes = false;
                    response.message = "This username or email doesn't exists";
                    response.datos = null;
                }
                else if (Encriptar.descifrar(pass) == Encriptar.descifrar(user.First().password))
                {
                    response.succes = true;
                    response.message = "success login";
                    response.datos = user.First();
                    createLogin(user.First().IdUser, 1);
                }
                else
                {
                    response.succes = false;
                    response.message = "login failed";
                    response.datos = null;
                    createLogin(user.First().IdUser, 2);
                }
                return response;
            }
            catch(Exception e) {
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
        }

        private static void createLogin(int id_user, int estado)
        {
            try
            {
               
                System.Web.HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
                Login login = new Login();
                login.IdUser = id_user;
                login.date = DateTime.Now;
                login.browser = browser.Browser;
                login.ip_address = GetIP();
                login.IdStatus = estado;
                db.Logins.Add(login);
                db.SaveChanges();
            }
            catch { 
            }

        }

        private static string GetIP()
        {
            //String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();

            //String IPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            //return IPAddress;

            //string VisitorsIPAddr = string.Empty;
            //if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //{
            //    VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //}
            //else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            //{
            //    VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            //}
            //return "IP: " + VisitorsIPAddr;

           // return Request.ServerVariables["REMOTE_ADDR"];
            return HttpContext.Current.Server.HtmlEncode(HttpContext.Current.Request.UserHostAddress);

        }
    }
}