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
using BCrypt.Net;

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
    // Hash the password
    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registration.Password);

    // Open a connection to the database
    using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConn").ToString()))
    {
        con.Open();

        // Prepare the SQL query
        string query = "INSERT INTO Users (Username, Password, EmailAddress, GivenName, Surname, Role) VALUES (@Username, @Password, @EmailAddress, @GivenName, @Surname, @Role)";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            // Set the query parameters
            cmd.Parameters.AddWithValue("@Username", registration.Username);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@EmailAddress", registration.EmailAddress);
            cmd.Parameters.AddWithValue("@GivenName", registration.GivenName);
            cmd.Parameters.AddWithValue("@Surname", registration.Surname);
            cmd.Parameters.AddWithValue("@Role", registration.Role);

            // Execute the query
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
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

            
        }
    }
