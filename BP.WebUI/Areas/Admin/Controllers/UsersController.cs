using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using BP.WebUI.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BP.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : GenericController<BllUser>
    {
        private readonly IService<BllRole> roleService;

        public UsersController(IService<BllUser> userService, IService<BllRole> roleService)
            : base(userService)
        {
            this.roleService = roleService;
        }

        public override ActionResult Edit(int id = 0)
        {
            BllUser obj = null;
            if (id != 0)
                obj = service.Find(id);

            if (obj == null)
            {
                obj = new BllUser();
                obj.Role = new BllRole();
            }

            return View(new EditUserViewModel { Id = obj.Id, Password = obj.Password, Email = obj.Email, Role = obj.Role.Name });
        }

        [HttpPost]
        public ActionResult EditUser(EditUserViewModel obj)
        {
            if (!ModelState.IsValid)
                return View("Edit", obj);

            BllRole role = roleService.Find(obj.Role);
            BllUser user = new BllUser
            {
                Id = obj.Id,
                Email = obj.Email,
                Password = obj.Password,
                Role = role
            };

            if (obj.Id == 0)
            {
                if (user.Password == null)
                {
                    ModelState.AddModelError("", Resources.Resource.RequiredPassword);
                    return View("Edit", user);
                }
                service.Create(user);
                return RedirectToAction("Index");
            }

            BllUser exsistUser = service.Find(obj.Id);
            if (exsistUser == null)
            {
                ModelState.AddModelError("", Resources.Resource.NotFound);
                return View("Edit", user);
            }

            if (string.IsNullOrEmpty(user.Password))
                user.Password = exsistUser.Password;

            service.Update(user);

            return RedirectToAction("Index");
        }
    }
}