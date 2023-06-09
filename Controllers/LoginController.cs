﻿using JwtApp.Models;
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
using System.Security.Cryptography;
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
            using RSA rsa = RSA.Create();
            RSAParameters rsaPublicKey = rsa.ExportParameters(true);
            var tokenHandler = new JwtSecurityTokenHandler();
            
            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var rsaKey = new RsaSecurityKey(rsaPublicKey);
            var credentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Email, user.EmailAddress),
                    new Claim(ClaimTypes.GivenName, user.GivenName),
                    new Claim(ClaimTypes.Surname, user.Surname),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            //var tokenString = new JwtSecurityTokenHandler().CreateToken(token);
            /*var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = rsaKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            */
            //var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out var validatedToken);
            //return new JwtSecurityTokenHandler().WriteToken(tokenString);
            return tokenString;
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DbConn").ToString());
            con.Open();
            String query="(Select * from users where Username='"+userLogin.Username+"' AND Password='"+userLogin.Password+"')";
            SqlDataAdapter da = new SqlDataAdapter(query,con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            //Console.WriteLine(dt);
            
            //var currentUser = UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

            if (dt.Rows.Count != 0)
            {
                DataRow row = dt.Rows[0];
                UserModel userModel = new UserModel();
                userModel.Username = row["Username"].ToString();
                userModel.Password = row["Password"].ToString();
                userModel.EmailAddress = row["EmailAddress"].ToString();
                userModel.Role = row["Role"].ToString();
                userModel.Surname = row["Surname"].ToString();
                userModel.GivenName = row["GivenName"].ToString();
                return userModel;
                
            }
            else{
                return null;
            }
            
        }
    }
}
