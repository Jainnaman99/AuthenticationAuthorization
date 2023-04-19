using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;


namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);

            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, user.clientid),
                new Claim(CustomClaimTypes.rolename, user.rolename),
                new Claim(CustomClaimTypes.privilege, user.privilege),
                new Claim(CustomClaimTypes.packagename, user.packagename),
                new Claim(CustomClaimTypes.api, user.api),
                new Claim(CustomClaimTypes.verb, user.verb),
                new Claim(CustomClaimTypes.username, user.username)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
//         private UserModel Authenticate(UserLogin userLogin)
// {
//     SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConn").ToString());
//     con.Open();
//     //String query = "(Select * from users where Username='" + userLogin.Username + "')";
//     String query = "select a.clientid, b.rolename, b.privilege, c.packagename, c.api, c.verb from client_package_role a join role_privilege b on a.roleid = b.roleid join tpackage c on a.packagename = c.packagename where a.clientid='"+userLogin.clientid+"' order by a.clientid ;";
//     SqlDataAdapter da = new SqlDataAdapter(query, con);
//     DataTable dt = new DataTable();
//     da.Fill(dt);

//     if (dt.Rows.Count != 0)
//     {

//         String apis = "";
//         foreach( DataRow ro in dt.Rows){
//             apis += "\n[" + ro["verb"].ToString() + "]: " + ro["api"].ToString() ;
//         }

//         DataRow row = dt.Rows[0];
//         //string hashedPassword = row["Password"].ToString();
//         //bool passwordVerified = BCrypt.Net.BCrypt.Verify(userLogin.Password, hashedPassword);

//         //if (passwordVerified)
//         {
//             UserModel userModel = new UserModel();
//             userModel.clientid = row["clientid"].ToString();
//             userModel.rolename = row["rolename"].ToString();
//             userModel.privilege = row["privilege"].ToString();
//             userModel.packagename = row["packagename"].ToString();
//             // userModel.api = row["api"].ToString();
//             userModel.api = apis;
//             userModel.verb = row["verb"].ToString();
//             return userModel;
//         }
//     }

//     return null;
// }


private UserModel Authenticate(UserLogin userLogin)
{
    SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConn").ToString());
    con.Open();
    String query = "(Select * from creds where username='" + userLogin.username + "')";
    SqlDataAdapter da = new SqlDataAdapter(query, con);
    DataTable dt = new DataTable();
    da.Fill(dt);

    if (dt.Rows.Count != 0)
    {        

        DataRow row = dt.Rows[0];
        string hashedPassword = row["password"].ToString();
        bool passwordVerified = BCrypt.Net.BCrypt.Verify(userLogin.password, hashedPassword);

        if (passwordVerified)
        {
            String usermodel_clientid = row["clientid"].ToString();
            String query1 = "select a.clientid, b.rolename, b.privilege, c.packagename, c.api, c.verb from client_package_role a join role_privilege b on a.roleid = b.roleid join tpackage c on a.packagename = c.packagename where a.clientid='"+usermodel_clientid+"' order by a.clientid ;";
            SqlDataAdapter da1 = new SqlDataAdapter(query1, con);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            if(dt1.Rows.Count != 0){
                String apis = "";
                foreach( DataRow ro in dt1.Rows){
                    apis += "\n[" + ro["verb"].ToString() + "]: " + ro["api"].ToString() ;
                }
                DataRow um_row = dt1.Rows[0];

                UserModel userModel = new UserModel();
                userModel.username = userLogin.username.ToString();
                userModel.clientid = um_row["clientid"].ToString();
                userModel.rolename = um_row["rolename"].ToString();
                userModel.privilege = um_row["privilege"].ToString();
                userModel.packagename = um_row["packagename"].ToString();
                // userModel.api = row["api"].ToString();
                userModel.api = apis;
                userModel.verb = um_row["verb"].ToString();
                return userModel;
            }   
        }
    }

    return null;
}


    }
}
