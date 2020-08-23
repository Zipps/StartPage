namespace StartPage
{
    public class AppSettings
    {
        public bool AllowSignup { get; set; }
        public JwtSettings Jwt { get; set; }

        public class JwtSettings
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
        }

    }
}