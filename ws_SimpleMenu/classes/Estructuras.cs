using Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_SimpleMenu.classes
{
    public class PrettyUser
    {
        public int id_user { get; set; }
        public bool locked { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public bool verificated { get; set; }
        public List<PrettyRoles> roles { get; set; }

        public static PrettyUser parseUserLogin(User user)
        {
            PrettyUser ul = new PrettyUser();
            ul.email = user.email;
            ul.id_user = user.reference_id;
            ul.username = user.username;
            ul.verificated = user.verificated;
            ul.roles = getRoles(user.Roles);
            return ul;
        }

        private static List<PrettyRoles> getRoles(List<Rol_User> list)
        {
            List<PrettyRoles> roles = new List<PrettyRoles>();
            foreach (var rol in list) { 
                PrettyRoles r = new PrettyRoles();
                r.name = rol.Rol.description;
                r.id_rol = rol.Rol.reference_id;
                roles.Add(r);
            }
            return roles;
        }
    }

    public class PrettyRoles
    {
        public int id_rol { get; set; }
        public string name { get; set; }


        public static List<PrettyRoles> parse(List<Rol> roles)
        {
            List<PrettyRoles> rls = new List<PrettyRoles>();
            foreach (var r in roles)
            {
                PrettyRoles rol = new PrettyRoles();
                rol.id_rol = r.reference_id;
                rol.name = r.description;
                rls.Add(rol);
            }
            return rls;
        }
    }

}