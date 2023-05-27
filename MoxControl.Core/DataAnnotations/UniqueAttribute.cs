using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MoxControl.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UniqueAttribute : Attribute
    {
        internal static void CheckAttribute(ModelBuilder builder, Type entityType, PropertyInfo[] columns)
        {
            foreach (var it in columns)
            {
                if (it.GetCustomAttribute<UniqueAttribute>() == null)
                    continue;

                builder.Entity(entityType)
                     .HasIndex(it.Name)
                     .IsUnique();
            }
        }
    }
}
