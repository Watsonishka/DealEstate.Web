using System.ComponentModel;
using System.Reflection;

namespace OnlineShopWebApp.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);

            if (name == null)
            {
                return value.ToString();
            }

            var field = type.GetField(name);

            if (field == null)
            {
                return value.ToString();
            }

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}