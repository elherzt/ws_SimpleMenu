using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ws_SimpleMenu.Models;

namespace ws_SimpleMenu.Controllers
{
    public class VerificationController : Controller
    {
        //
        // GET: /Verification/

        public ActionResult link_verification(string secure_token)
        {
            var status = LoginOptions.verificate_account(secure_token);
            if (status == 0) {
                return RedirectToAction("Invalid");
            }
            else if (status == 1)
            {
                return RedirectToAction("Verificated");
            }
            else { 
                return RedirectToAction("newToken");
            }
        }

        public ActionResult Verificated()
        {
            return View();
        }

        public ActionResult Invalid()
        {
            return View();
        }

        public ActionResult newToken()
        {
            return View();
        }
    }
}
