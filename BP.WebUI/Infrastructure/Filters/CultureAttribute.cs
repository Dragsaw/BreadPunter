using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace BP.WebUI.Infrastructure.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        private static List<string> customCultures = new List<string> { "en", "ru" };
        public static List<string> Cultures { get { return customCultures; } }
        private readonly string defaultCulture = "en";

        static CultureAttribute()
        {
            foreach (var lang in customCultures)
                CultureInfo.CreateSpecificCulture(lang);
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string culture;
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cookie != null)
                culture = cookie.Value;
            else culture = defaultCulture;

            if (customCultures.Contains(culture))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(defaultCulture);
            }
        }
    }
}