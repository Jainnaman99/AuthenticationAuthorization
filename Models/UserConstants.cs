using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtApp.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel() { Username = "harry_admin", EmailAddress = "harry.admin@email.com", Password = "MyPass_w0rd", GivenName = "Harry", Surname = "Ahuja", Role = "Administrator" },
            new UserModel() { Username = "keshav_client", EmailAddress = "keshav.client@email.com", Password = "MyPass_w0rd", GivenName = "Keshav", Surname = "Kang", Role = "Client" },
        };
    }
}
