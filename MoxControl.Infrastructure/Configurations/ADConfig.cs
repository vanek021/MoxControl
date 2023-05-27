namespace MoxControl.Infrastructure.Configurations
{
#pragma warning disable CS8618
    public class ADConfig
    {
        public string Server { get; set; }
        public int Port { get; set; } = 389;
        public string Zone { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string UsersOU { get; set; } = string.Empty;
        public string GroupsOU { get; set; } = string.Empty;

        public string SearchBase { get; set; } = string.Empty;
        public string SearchFilter { get; set; } = string.Empty;
        public string GroupSearchFilter { get; set; } = string.Empty;
    }
#pragma warning restore CS8618
}
