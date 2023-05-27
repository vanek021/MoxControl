using System.ComponentModel.DataAnnotations;

namespace MoxControl.Areas.Identity.ViewModels
{
#pragma warning disable CS8618
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
#pragma warning restore CS8618
}
