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
        private readonly string defaultCulture = "en";

        static CultureAttribute()
        {
            CultureInfo.CreateSpecificCulture("en");
            CultureInfo.CreateSpecificCulture("ru");
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string culture;
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cookie != null)
                culture = cookie.Value;
            else culture = defaultCulture;

            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            }
            catch (CultureNotFoundException)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(defaultCulture);
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}