namespace Server
{
    public static class Constants
    {
        // This identifies to whom or to which application, we are issuing JWT tokens
        public const string Audience = "https://localhost:44321/";
        public const string Issuer = Audience;
        public const string SecretKey = "not_too_small_secret_otherwise_it_might_error";
    }
}
