using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ws_SimpleMenu.Models;
using Modelos;

namespace ws_SimpleMenu.Controllers
{
    public class AddUserController : ApiController
    {
        public Response Post(User user, int? secure)
        {
            return UserOptions.AddUser(user, secure);
        }
    }

    public class AddRolController : ApiController
    {
        public Response Post(Rol rol)
        {
            return RolOptions.Add(rol);
        }
    }
}
