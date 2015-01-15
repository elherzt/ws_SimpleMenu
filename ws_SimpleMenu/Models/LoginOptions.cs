using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelos;
using System.Net;
using ws_SimpleMenu.classes;

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
            else
            {
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
                else if ((bool)user.First().locked == true)
                {
                    if (check_lock(user.First().IdUser))
                    {
                        response.succes = false;
                        response.message = "user locked";
                        response.datos = null;
                    }
                    else
                    {
                        response = LoginOptions.login(us, pass, ban);
                    }
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
            catch (Exception e)
            {
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
        }

        private static bool check_lock(int id_user)
        {
            var query = db.Locks.Where(x => x.IdUser == id_user).ToList().Last();
            var minutes = DateTime.Now.Subtract(query.date).Minutes;
            if (DateTime.Now.Subtract(query.date).Minutes >= Configuraciones.locked_time)
            {
                var user = db.Users.Where(x => x.IdUser == id_user).SingleOrDefault();
                user.locked = false;
                db.SaveChanges();
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void createLogin(int id_user, int estado)
        {
            try
            {
                if (estado == 2) { 
                    maybe_lock_user(id_user);
                }
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
            catch
            {
            }

        }

        private static void maybe_lock_user(int id_user)
        {
            var logins = db.Logins.Where(x => x.IdUser == id_user).ToList();
            if (logins.Count() >= (Configuraciones.fails_to_lock - 1))
            {
                var recent_logins = logins.Skip(Math.Max(0, logins.Count() - (Configuraciones.fails_to_lock - 1))).Take(Configuraciones.time_between_fails-1);
                var recent_fails = recent_logins.Where(x => x.IdStatus == 2).ToList();
                if (recent_fails.Count() == Configuraciones.fails_to_lock - 1) 
                {
                    if (DateTime.Now.Subtract(recent_fails.First().date).Minutes <= Configuraciones.time_between_fails)
                    {
                        var user = db.Users.Where(x => x.IdUser == id_user).SingleOrDefault();
                        user.locked = true;
                        Lock _lock = new Lock();
                        _lock.IdUser = id_user;
                        _lock.date = DateTime.Now;
                        db.Locks.Add(_lock);
                        db.SaveChanges();
                    }
                }
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