using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Extensions
{
    public static class AttributeHelper
    {
        public static string GetDescription<T>(this T source) where T : struct
        {
            var type = source.GetType();
            Debug.Assert(type.GetTypeInfo().IsEnum, "Only enums allowed");
            FieldInfo fi = type.GetField(source.ToString());

            DisplayAttribute[] attributesName = (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            DescriptionAttribute[] attributesDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributesName != null && attributesName.Length > 0)
                return attributesName[0].Name;
            else if (attributesDesc != null && attributesDesc.Length > 0)
                return attributesDesc[0].Description;
            else
                return source.ToString();
        }
    }
}
