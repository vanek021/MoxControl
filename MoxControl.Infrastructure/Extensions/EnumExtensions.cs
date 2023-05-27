using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MoxControl.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<T> GetAllItems<T>(this Enum value)
        {
            foreach (object item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }

        public static IEnumerable<T> GetAllItems<T>() where T : struct
        {
            foreach (object item in Enum.GetValues(typeof(T)))
            {
                yield return (T)item;
            }
        }

        public static string GetDisplayName(this System.Enum value)
        {
            var attribute = value.GetType().GetField(value.ToString())?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.GetName() ?? value.ToString();
        }
    }
}
