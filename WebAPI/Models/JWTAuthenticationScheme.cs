namespace WebAPI.Models
{
    public class JWTAuthenticationScheme
    {
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }

    }
}
