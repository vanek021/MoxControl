using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Infrastructure.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this System.Enum value)
        {
            var attribute = value.GetType().GetField(value.ToString())?.GetCustomAttribute<DisplayAttribute>();
            return attribute?.GetName() ?? value.ToString();
        }
    }
}
