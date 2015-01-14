using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ws_SimpleMenu.Models
{
    public class Response
    {
        public bool succes { get; set; }
        public string message { get; set; }
        public dynamic datos { get; set; }
    }
}