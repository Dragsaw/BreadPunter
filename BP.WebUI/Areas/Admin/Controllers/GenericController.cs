using BP.BLL.Interface;
using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.WebUI.Areas.Admin.Controllers
{
    public abstract class GenericController<T> : Controller
        where T : class, IBllEntity
    {
        protected readonly IService<T> service;

        protected GenericController(IService<T> service)
        {
            this.service = service;
        }

        public ActionResult Index()
        {
            var objects = service.GetAll();
            return View(objects);
        }

        public virtual ActionResult Edit(int id = 0)
        {
            T obj = service.Find(id);
            if (obj != null)
                return View(obj);

            return View();
        }

        [HttpPost]
        public virtual ActionResult Edit(T obj)
        {
            if (service.Find(obj.Id) != null)
                service.Update(obj);
            else service.Create(obj);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            if (service.Remove(id))
                return RedirectToAction("Index");
            else ModelState.AddModelError("", Resources.Resource.NotFound);
            return View("Index");
        }
    }
}