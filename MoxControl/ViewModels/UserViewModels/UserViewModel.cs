namespace MoxControl.ViewModels.UserViewModels
{
#pragma warning disable CS8618
    public class UserViewModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Role { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
    }
#pragma warning restore CS8618
}
