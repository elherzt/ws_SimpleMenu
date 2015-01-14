using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelos;
using ws_SimpleMenu.Models;

namespace ws_SimpleMenu.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            UserContext db = new UserContext();
            return View(db.Users.ToList());
        }

        public string codificar(string cadena)
        {
            return Encriptar.Encrypt(cadena);
        }
    }
}
