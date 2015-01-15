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

    public class AddRolToUserController : ApiController
    {
        public Response Post(int id_rol, int id_user)
        {
            return UserOptions.AddRol(id_rol, id_user);
        }
    }

    public class LoginController : ApiController
    {
        public Response Post(string user, string password)
        {
            return LoginOptions.Autenticar(user, password);
        }
    }

    public class getUserRolesController : ApiController
    {
        public Response Post(int id_user)
        {
            return UserOptions.getRoles(id_user);
        }
    }
}
