using System.Web.Mvc;

namespace Framework.Helper.Extensions
{
    public static class UrlHelperExtension
    {
        public static string Home(this UrlHelper helper)
        {
            return helper.Content("~/");
        }
    }
}
