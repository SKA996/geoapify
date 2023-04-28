namespace GEO_SU {
    internal class Config {

        public string ApiKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public Config(string apiKey, string username, string password) {
            ApiKey = apiKey;
            Username = username;
            Password = password;
        }
    }
}