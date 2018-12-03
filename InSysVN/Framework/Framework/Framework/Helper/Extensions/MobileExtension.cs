using System.Text.RegularExpressions;

namespace Framework.Helper.Extensions
{
    /// <summary>
    /// Extension valid phone number
    /// </summary>
    public static class MobileExtension
    {
        public static bool IsPhoneNumber(string phone)
        {
            var regex = new Regex(@"^[\d\s\-\+\(\)]*$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(phone).Length > 0;
        }
    }
}
