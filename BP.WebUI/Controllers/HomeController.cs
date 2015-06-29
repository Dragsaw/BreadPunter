using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using BP.WebUI.Infrastructure.Filters;
using BP.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.WebUI.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Language()
        {
            return PartialView("_LanguagesPartial", new LanguagesViewModel());
        }

        public ActionResult ChangeLanguage(string lang, string returnUrl)
        {
            CultureInfo ci = CultureInfo.GetCultureInfo(lang);
            if (ci != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies["lang"];
                if (cookie != null)
                    HttpContext.Response.Cookies["lang"].Value = ci.Name;
                else
                {
                    cookie = new HttpCookie("lang", ci.Name);
                    cookie.Expires.AddYears(1);
                    HttpContext.Response.Cookies.Add(cookie);
                }
            }
            return Redirect(returnUrl);
        }

        public ActionResult Error(int statusCode = 0)
        {
            if (statusCode == 404)
                return View("Error404");
            return View();
        }
    }
}