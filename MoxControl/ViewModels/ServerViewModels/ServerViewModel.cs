using MoxControl.Connect.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoxControl.ViewModels.ServerViewModels
{
    public class ServerViewModel
    {
        public long Id { get; set; }

        [Display(Name = "Система виртуализации")]
        [Required]
        public VirtualizationSystem VirtualizationSystem { get; set; }

        [Display(Name = "Адрес")]
        [Required]
        public string Host { get; set; }

        [Display(Name = "Порт")]
        [Required]
        public int Port { get; set; }

        [Display(Name = "Тип авторизации")]
        [Required]
        public AuthorizationType AuthorizationType { get; set; } = AuthorizationType.UserCredentials;

        [Display(Name = "Имя root пользователя")]
        public string? RootLogin { get; set; }

        [Display(Name = "Пароль root пользователя")]
        public string? RootPassword { get; set; }

        [Display(Name = "Имя")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required]
        public string Description { get; set; }

        public ServerStatus Status { get; set; }
    }
}
