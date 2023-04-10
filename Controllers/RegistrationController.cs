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
using System.Data.SqlClient;

namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RegistrationController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public string registration([FromBody] Registration registration)
        {
            //String constring ="Data Source=PSL-2RXL6Q3\\SQLEXPRESS;Initial Catalog=newDB;TrustServerCertificate=true;Integrated Security=SSPI";
            SqlConnection con= new SqlConnection(_config.GetConnectionString("DbConn").ToString());
            con.Open();
            
            String query="INSERT INTO Users (Username,Password,EmailAddress,GivenName,Surname,Role) VALUES ('"+registration.Username+"','"+registration.Password+"','"+registration.EmailAddress+"','"+registration.GivenName+"','"+registration.Surname+"','"+registration.Role+"')";
            //String query="INSERT INTO USERS (Username,Password,EmailAddress,GivenName,Surname,Role) VALUES ('Jerry','MyPass_w0rd','jerry.admin@email.com','Jerry','Leo','Admin')";
            SqlCommand cmd= new SqlCommand(query,con);
            //SqlCommand cmd= new SqlCommand("Select * from Users",con);
            
            int i=cmd.ExecuteNonQuery();
            con.Close();
            if(i>0)
            {
                return "Data Inserted";
            }

            else
            {
                return "Error";
            }
            
        }

            
        }
    }
