using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Services;
using BP.WebUI.Infrastructure.Filters;
using BP.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BP.WebUI.Controllers
{
    [AllowAnonymous]
    [Culture]
    public class AccountController : Controller
    {
        private readonly IUserService userService;
        private readonly IRoleService roleService;

        public AccountController(IUserService userService, IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password, bool remember, string returnUrl)
        {
            if (ModelState.IsValid && userService.Exists(email, password))
            {
                FormsAuthentication.SetAuthCookie(email, remember);
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                else return RedirectToAction("Index", "Home");
            }
            else ModelState.AddModelError("", Resources.Resource.WrongEmailOrPassword);

            return View();
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register(string role)
        {
            if (role != null)
            {
                BalRole roleEntity = roleService.Find(role.ToLower());
                if (roleEntity != null && roleEntity.Name != "admin")
                {
                    ViewBag.Role = role;
                    return View();
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Register(RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                if (userService.Find(model.Email) == null)
                {
                    userService.Create(model.Email, model.Password, roleService.Find(model.Role));
                    return View("RegistrationSuccess");
                }
                else ModelState.AddModelError("", string.Format(Resources.Resource.UserAlreadyExists, model.Email));
            }
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            roleService.Dispose();
            base.Dispose(disposing);
        }
    }
}