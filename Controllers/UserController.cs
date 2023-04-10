using JwtApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace JwtApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet("Admins")]
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }


        [HttpGet("Clients")]
        [Authorize(Roles = "Client")]
        public IActionResult ClientsEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are a {currentUser.Role}");
        }

        [HttpGet("AdminsAndClients")]
        [Authorize(Roles = "Administrator,Client")]
        public IActionResult AdminsAndSellersEndpoint()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.GivenName}, you are an {currentUser.Role}");
        }

        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi, you're on public property");
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            
            var jwtToken = identity?.FindFirst("jwt")?.Value;
            //var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            //var jwtToken = authorizationHeader?.Replace("Bearer ", "");
           
            if (jwtToken !=null)
            {
                Console.WriteLine(jwtToken);
                // Create an instance of the JwtSecurityTokenHandler class
                var tokenHandler = new JwtSecurityTokenHandler();

                // Create an instance of the RsaSecurityKey class using the RSA public key
                //var rsaPublicKey = RSA.Create();
                //var rsa = RSA.Create();
                //var rsaPublicKeyBytes = Convert.FromBase64String("your_rsa_public_key_here");
                //rsaPublicKey.ImportSubjectPublicKeyInfo(rsaPublicKeyBytes, out _);
                //var rsaKey = new RsaSecurityKey(rsaPublicKey);
                //var rsa = RSA.Create();
                using RSA rsa = RSA.Create();
                RSAParameters rsaPrivateKey = rsa.ExportParameters(false);
                
                //var publicKeyParams = rsa.ExportRSAPublicKey();
                //var rsaPublicKey = RSA.Create();
                //rsaPublicKey.ImportRSAPublicKey(publicKeyParams, out _);
                var rsaKey = new RsaSecurityKey(rsaPrivateKey);
                

                // Create an instance of the TokenValidationParameters class
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = rsaKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                 var principal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
                 var userName = principal.FindFirst(ClaimTypes.Name)?.Value;
                 var userClaims = identity.Claims;

                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            
            return null;
        }
    }
}
