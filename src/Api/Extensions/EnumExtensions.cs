using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Api.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            MemberInfo member = type.GetMembers().Where(w => w.Name == Enum.GetName(type, value)).FirstOrDefault();
            var attribute = member?.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description != null ? attribute.Description : value.ToString();
        }
    }
}
