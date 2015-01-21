using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_SimpleMenu.classes
{
    public class Configuraciones
    {
        public static int locked_time = 15;
        public static int fails_to_lock = 5;
        public static int time_between_fails = 5;
        public static int recent_logins_by_user = 10;
        public static int time_to_verificate_email = 3;
    }
}