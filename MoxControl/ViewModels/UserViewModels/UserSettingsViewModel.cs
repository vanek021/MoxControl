using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.UserViewModels
{
    public class UserSettingsViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Имя в Telegram")]
        public string? TelegramName { get; set; }
    }
}
