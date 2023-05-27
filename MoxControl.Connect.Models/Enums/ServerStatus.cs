using System.ComponentModel.DataAnnotations;

namespace MoxControl.Connect.Models.Enums
{
    public enum ServerStatus
    {
        [Display(Name = "Неизвестно")]
        Unknown = 0,
        [Display(Name = "Запущен")]
        Running,
        [Display(Name = "Остановлен")]
        Stopped,
        [Display(Name = "Не стабилен")]
        Unstable
    }
}
