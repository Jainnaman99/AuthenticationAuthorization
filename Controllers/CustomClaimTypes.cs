namespace JwtApp.Controllers
{
    public static class CustomClaimTypes
    {
        public const string clientid = "http://schemas.myapp.com/claims/clientid";

        public const string rolename = "http://schemas.myapp.com/claims/rolename";
        public const string privilege = "http://schemas.myapp.com/claims/privilege";
        public const string packagename = "http://schemas.myapp.com/claims/packagename";
        public const string api = "http://schemas.myapp.com/claims/api";
        public const string verb = "http://schemas.myapp.com/claims/verb";

    }
}