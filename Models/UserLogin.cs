using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtApp.Models
{
    public class UserLogin
    {
        public string rolename { get; set; }
        public string clientid { get; set; }
        public string privilege { get; set; }
    }
}
