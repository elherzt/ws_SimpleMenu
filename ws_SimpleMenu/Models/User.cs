using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Modelos;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Net.Mail;

namespace ws_SimpleMenu.Models
{
    public class UserOptions 
    {
        static UserContext db = new UserContext();
        public static Response AddUser(User user, int? secure)
        {
            Response response = new Response();
            try
            {
                var message = isValid(user, secure);
                if (message == "valid")
                {
                    user.locked = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    response.succes = true;
                    response.message = "User added";
                    response.datos = user;
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

        public static Response EditEmail(int id_reference, string new_email)
        {
            Response response = new Response();
            try
            {
                if (isValidIdUser(id_reference))
                {
                    if (isNewEmail(new_email))
                    {
                        if (isValidEmail(new_email))
                        {
                            var user = find_by_reference_id(id_reference);
                            user.email = new_email;
                            db.SaveChanges();
                            response.succes = true;
                            response.message = "NO ERROR";
                            response.datos = null;
                        }
                        else {
                            response.succes = false;
                            response.message = "Email is not valid";
                            response.datos = null;
                        }
                    }
                    else
                    {
                        response.succes = false;
                        response.message = "Email current in use";
                        response.datos = null;
                    }
                }
                else {
                    response.succes = false;
                    response.message = "User doesn't exists";
                    response.datos = null;
                }
                return response;
            }
            catch(Exception e)
            {
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
        }

        public static Response AddRol(int id_rol, int id_user)
        {
            Response response = new Response();
            try
            {
                if (isValidIdUser(id_user))
                {
                    if (isValiIdRol(id_rol))
                    {
                        var UserId = getIdUser(id_user);
                        var RolId = getIdRol(id_rol);
                        if (isNewUserRol(UserId, RolId))
                        {
                            Rol_User ru = new Rol_User();
                            ru.IdRol = RolId;
                            ru.IdUser = UserId;
                            db.Roles_Users.Add(ru);
                            db.SaveChanges();
                            response.succes = true;
                            response.message = "Rol added to user";
                            response.datos = null;
                            return response;
                        }
                        else
                        {
                            response.succes = false;
                            response.message = "The user already has the role";
                            response.datos = null;
                        }
                    }
                    else
                    {
                        response.succes = false;
                        response.message = "Rol not added to user, id rol is not valid";
                        response.datos = null;
                    }
                }
                else
                {
                    response.succes = false;
                    response.message = "Rol not added to user, id user is not valid";
                    response.datos = null;
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

        public static Response getAllInfoUser(int id)
        {
            Response response = new Response();
            try
            {
                response.succes = true;
                response.message = "NO ERROR";
                response.datos = UserOptions.find_by_reference_id(id);
                return response;
            }
            catch(Exception e)
            {
                response.succes = false;
                response.message = e.Message;
                response.datos = null;
                return response;
            }
        }

        private static bool isNewUserRol(int id_user, int id_rol)
        {
            return (db.Roles_Users.Where(x => x.IdRol == id_rol && x.IdUser == id_user).ToList().Count() == 0) ? true : false;
        }

        public static Response getRoles(int id_user)
        {
            Response response = new Response();
            try {
                if (isValidIdUser(id_user))
                {
                    var UserId = getIdUser(id_user);
                    var roles_ids = db.Roles_Users.Where(x => x.IdUser == UserId).Select(x => x.IdRol).ToList();
                    var roles = db.Roles.Where(p => roles_ids.Any(p2 => p2 == p.IdRol)).ToList();
                    response.succes = true;
                    response.message = "NO ERROR";
                    if (roles.Count() > 0)
                    {
                        response.datos = roles;
                    }
                    else {
                        response.datos = "User doesn't have roles";
                    }
                }
                else {
                    response.succes = false;
                    response.message = "Id reference in not exists or is invalid";
                    response.datos = null;
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

        private static int getIdUser(int id_user)
        {
            return db.Users.Where(x => x.reference_id == id_user).SingleOrDefault().IdUser;
        }

        private static int getIdRol(int id_rol)
        {
            return db.Roles.Where(x => x.reference_id == id_rol).SingleOrDefault().IdRol;
        }

        private static bool isValiIdRol(int id_rol)
        {
            if (id_rol < 0)
            {
                return false;
            }
            else {
                if (db.Roles.Where(x => x.reference_id == id_rol).ToList().Count() == 1)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
        }

        private static bool isValidIdUser(int id_user)
        {
            if (id_user < 0)
            {
                return false;
            }
            else
            {
                if (db.Users.Where(x => x.reference_id == id_user).ToList().Count() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static string isValid(User user, int? secure)
        {
            //string securePass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$";
            string message = "";
            if (isNewEmail(user.email))
            {
                if (isValidEmail(user.email))
                {
                    message += "0 Email: invalid,";
                }
                else
                {
                    message += "Email: valid,";
                }
            }
            else { 
                    message += "Email: Email current existing,";
            }
           try{
                string password = Encriptar.descifrar(user.password); 
            
            if (secure == 0)
            {
                if (password.Length < 8)
                {
                    message += "0 Password: invalid,";
                }
                else
                {
                    message += "Password: valid,";
                }
            }
            else if (secure == 1)
            {
                if (Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)(?!.*(.)\1\1)[a-zA-Z0-9@]{8,15}$")) // password valido: al menos 1 mayuscula, al menos 1 numero, no caracteres especiales, de 8 a 15
                {
                    message += "Password: valid,";
                }
                else
                {
                    message += "0 Password: invalid,";
                }
            }
            else{
                 message += "0 Password: secure value is invalid,";
            }
            }catch{
                message += "0 Password: wrong encrypted,";
            }
            if (IsNew(user.username))
            {
                message += "Username: valid,";
            }
            else {
                message += "0 Username existing,";
            }
            if (referenceIsValid(user.reference_id))
            {
                message += "id_reference: valid";
            }
            else {
                message += "0 id reference invalid o existing";
            }
            if (message.Contains("0"))
            {
                message = message.Replace("0", "");
            }
            else {
                message = message = "valid";
            }
            return message;
        }

        private static bool isValidEmail(string email)
        {
            Regex ValidEmail = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            Match match = (ValidEmail.Match(email));
            if (match.Success)
            {
                return true;
            }
            else {
                return false;
            }
            //try
            //{
            //    MailAddress m = new MailAddress(email);

            //    return true;
            //}
            //catch (FormatException)
            //{
            //    return false;
            //}
        }

        private static bool isNewEmail(string p)
        {
            return (db.Users.Where(x => x.email == p).ToList().Count()) == 0 ? true : false;
        }

        private static bool referenceIsValid(int reference_id)
        {
            if (reference_id < 1) {
                return false;
            }
            return (db.Users.Where(x => x.reference_id == reference_id).ToList().Count == 0) ? true : false;
        }

        private static bool IsNew(string p)
        {
           return (db.Users.Where(x => x.username == p.Trim()).ToList().Count == 0) ? true : false;
        }




        internal static User find_by_id(int id_user)
        {
            return db.Users.Where(x => x.IdUser == id_user).SingleOrDefault();
        }

        internal static User find_by_reference_id(int id_reference)
        {
            return db.Users.Where(x => x.reference_id == id_reference).SingleOrDefault();
        }
    }
}