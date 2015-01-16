using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelos;

namespace ws_SimpleMenu.Models
{
    public class RolOptions
    {
        static UserContext db = new UserContext();
        public static Response Add(Rol rol) 
        {
                Response response = new Response();
            try
            {
                var message = IsRolValid(rol);
                if (message == "valid")
                {
                    db.Roles.Add(rol);
                    db.SaveChanges();
                    response.succes = true;
                    response.message = "rol added";
                    response.datos = rol;
                }
                else
                {
                    response.succes = false;
                    response.message = message;
                    response.datos = null;
                }
                return response;
            }
            catch (Exception e) {
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
            
        }



        public static Response getRoles() 
        {
            Response response = new Response();
            try
            {
                response.succes = true;
                response.message = "NO ERROR";
                response.datos = db.Roles.ToList();
                return response;
            }
            catch(Exception e){
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
        }

        private static string IsRolValid(Rol rol)
        {
            var message = "";
            if (IsNewReference(rol.reference_id))
            {
                if (IsValidReference(rol.reference_id))
                {
                    message += "reference id: valid,";
                }
                else
                {
                    message += "reference id: 0 Id de referencia invalido,";
                }
            }
            else
            {
                message += "reference id: 0 Id de referencia existente,";
            }
            if (rol.description.Length > 50)
            {
                message += "descripcion: 0 Descripcion demasiado larga";
            }
            else if (!IsNewRolName(rol.description))
            {
                message += "descripcion: 0 Ese nombre de rol ya existe";
            }
            else { 
                message += "valid";
            }

            if (message.Contains("0"))
            {
                message = message.Replace("0", "");
            }
            else
            {
                message = "valid";
            }
            return message;
        }

        private static bool IsNewRolName(string p)
        {
            return (db.Roles.Where(x => x.description == p.Trim()).ToList().Count() == 0) ? true : false;
        }

        private static bool IsNewReference(int id)
        {
            return (db.Roles.Where(x => x.reference_id == id).ToList().Count() == 0) ? true : false;
        }

        private static bool IsValidReference(int id)
        {
            return id > 0 ? true : false;
        }
    }
}