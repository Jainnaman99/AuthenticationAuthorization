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
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registration.password);

            // Open a connection to the database
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConn").ToString()))
            {
                con.Open();

                // Prepare the SQL query
                string query = "INSERT INTO creds VALUES (@Username, @Password, @ClientID)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Set the query parameters
                    cmd.Parameters.AddWithValue("@Username", registration.username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);
                    cmd.Parameters.AddWithValue("@ClientID", registration.clientid);

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
