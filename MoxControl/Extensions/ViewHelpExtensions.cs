using MoxControl.Connect.Models.Enums;
using MoxControl.Infrastructure.Extensions;
using MoxControl.Models.Enums;

namespace MoxControl.Extensions
{
    public static class ViewHelpExtensions
    {
        public static string GetWelcomeMessage(this string username)
        {
            var moscowTime = DateTime.UtcNow.ToMoscowTime();

            if (moscowTime.Hour <= 11 && moscowTime.Hour >= 5)
            {
                return $"Доброе утро, {username}!";
            }

            if (moscowTime.Hour >= 12 && moscowTime.Hour <= 16)
                return $"Добрый день, {username}!";

            if (moscowTime.Hour >= 17 && moscowTime.Hour <= 23)
                return $"Добрый вечер, {username}!";

            return $"Доброй ночи, {username}!";
        }

        public static string GetServerStatusColor(this ServerStatus status)
        {
            return status switch
            {
                ServerStatus.Running => "badge badge-light-success",
                ServerStatus.Stopped => "badge badge-light-danger",
                ServerStatus.Unstable => "badge badge-light-warning",
                _ => "badge badge-light",
            };
        }

        public static string GetMachineStatusColor(this MachineStatus status)
        {
            return status switch
            {
                MachineStatus.Running => "badge badge-light-success",
                MachineStatus.Stopped => "badge badge-light-danger",
                _ => "badge badge-light",
            };
        }

        public static string GetMachineStageColor(this MachineStage stage)
        {
            return stage switch
            {
                MachineStage.ReadyForCreate => "badge badge-light-info",
                _ => "badge badge-light",
            };
        }

        public static string GetGeneralNotificationColor(this GeneralNotificationType type)
        {
			return type switch
			{
				GeneralNotificationType.Error => "badge-light-danger",
				GeneralNotificationType.Warning => "badge-light-warning",
				GeneralNotificationType.Info => "badge-light-info",
				GeneralNotificationType.Success => "badge-light-success",
				GeneralNotificationType.InternalServerError => "badge-light-danger",
				_ => "badge badge-light",
			};
		}

		public static string GetNotificationColor(this NotificationType type)
		{
			return type switch
			{
				NotificationType.Common => "badge-light-success",
				_ => "badge badge-light",
			};
		}
	}
}
