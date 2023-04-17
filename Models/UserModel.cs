using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtApp.Models
{
    public class UserModel
    {
        public string clientid { get; set; }
        public string rolename { get; set; }
        public string privilege { get; set; }
        public string packagename { get; set; }
        public string api { get; set; }
        public string verb { get; set; }
    }
}
