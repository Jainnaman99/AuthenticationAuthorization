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


namespace JwtApp.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {

        [HttpGet("[controller]")]
        [Authorize(Roles="1,2,3,4")]
        public IActionResult EndPoint1()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.username}, you are an {currentUser.rolename} and you can access APIs : {currentUser.api}");
        }


        [HttpPost("[controller]")]
        [Authorize(Policy = "KIOSK")]
        [Authorize(Roles="2,3")]
        //[Authorize(clientid = 1)]
        public IActionResult EndPoint2()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.username}, you are an {currentUser.rolename} and you can access APIs : {currentUser.api}");
        }

        [HttpPatch("[controller]")]
        [Authorize(Policy = "KIOSK")]
        [Authorize(Roles="2,3")]
        //[Authorize(clientid = 1)]
        public IActionResult EndPoint5()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.username}, you are an {currentUser.rolename} and you can access APIs : {currentUser.api}");
        }

        [HttpGet("[controller]/{player-id}/offers")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult EndPoint3()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.username}, you are an {currentUser.rolename} and you can access APIs : {currentUser.api}");
        }

        [HttpPost("[controller]/{player-id}/offers")]
        [Authorize(Roles = "1,2,3,4")]
        public IActionResult EndPoint4()
        {
            var currentUser = GetCurrentUser();

            return Ok($"Hi {currentUser.username}, you are an {currentUser.rolename} and you can access APIs : {currentUser.api}");
        }


        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new UserModel
                {
                    username = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.username)?.Value,
                    clientid = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                    rolename = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.rolename)?.Value,
                    privilege = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.privilege)?.Value,
                    packagename = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.packagename)?.Value,
                    api = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.api)?.Value,
                    verb = userClaims.FirstOrDefault(o => o.Type == CustomClaimTypes.verb)?.Value
                };
            }
            return null;
        }
    }
}
