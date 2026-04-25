using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;

namespace SENSEI.WEB.Helpers
{
    public static class GlobalHelpers
    {
        public static string GenerateOtp()
        {
            int otp = RandomNumberGenerator.GetInt32(100000, 1000000);
            return otp.ToString();
        }

        // Get Description Attribute
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute =
                field.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? value.ToString();
        }

        // Get Display(Name="...")
        public static string GetEnumDisplayName(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DisplayAttribute attribute =
                field.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }

        public static DateTime GetSriLankaTime()
        {
            DateTime utcTime = DateTime.UtcNow;
            try
            {
                TimeZoneInfo slZone = TimeZoneInfo.FindSystemTimeZoneById("Sri Lanka Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(utcTime, slZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallback for non-Windows environments or if TZ not found
                return utcTime.AddHours(5.5);
            }
        }
    }
}
