using MoxControl.Infrastructure.Extensions;

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
    }
}
