using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using BP.WebUI.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BP.WebUI.Infrastructure.Mappers;
using BP.WebUI.Models;

namespace BP.WebUI.Controllers
{
    [Authorize]
    [Culture]
    public class ProfileController : Controller
    {
        private int usersPerPage = 15;
        private readonly IUserService userService;
        private readonly string defaultImagePath = @"~/Content/Images/User.png";
        private readonly string defaultImageType = ".png";
        private IService<BalSkill> skillService;

        public ProfileController(IUserService userService, IService<BalSkill> skillService)
        {
            this.userService = userService;
            this.skillService = skillService;
        }

        public ActionResult Index(int? id = null)
        {
            BalUser user;
            if (id != null)
                user = userService.Find((int)id);
            else user = userService.Find(User.Identity.Name);

            if (user is BalProgrammer)
                return View("ProgrammerProfile", (BalProgrammer)user);
            else if (user is BalManager)
                return View("ManagerProfile", (BalManager)user);
            else return RedirectToAction("Index", "Home");
        }

        public FileResult GetPhoto(int id)
        {
            BalProgrammer user = (BalProgrammer)userService.Find(id);
            if (user.Photo != null && user.ImageType != null)
                return File(user.Photo, user.ImageType);
            else return File(Server.MapPath(defaultImagePath), defaultImageType);
        }

        [Authorize(Roles="Programmer")]
        public ActionResult EditInfo()
        {
            BalProgrammer user = (BalProgrammer)userService.Find(User.Identity.Name);
            UserInfoViewModel ui = new UserInfoViewModel();
            ui.GetInfo(user);
            return View(ui);
        }

        [HttpPost]
        [Authorize(Roles = "Programmer")]
        public ActionResult EditInfo(UserInfoViewModel userInfo)
        {
            BalProgrammer user = (BalProgrammer)userService.Find(User.Identity.Name);
            userInfo.SetUserInfo(user);
            if (userInfo.Image != null)
            {
                user.ImageType = userInfo.Image.ContentType;
                user.Photo = new byte[userInfo.Image.ContentLength];
                userInfo.Image.InputStream.Read(user.Photo, 0, userInfo.Image.ContentLength);
            }

            userService.Update(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Programmer")]
        public ActionResult EditSkills()
        {
            BalProgrammer user = (BalProgrammer)userService.Find(User.Identity.Name);

            var skills = skillService.GetAll().Select(x => new UserSkillViewModel { Skill = x }).ToList();
            foreach (var skill in user.Skills)
            {
                skills.First(x => x.Skill.Id == skill.Key.Id).Level = skill.Value;
            }

            return View(new EditUserSkillsViewModel { Id = user.Id, Skills = skills });
        }

        [HttpPost]
        [Authorize(Roles = "Programmer")]
        public ActionResult EditSkills(EditUserSkillsViewModel model)
        {
            BalProgrammer user = (BalProgrammer)userService.Find(model.Id);
            user.Skills = model.Skills
                .Where(x => x.Level != 0)
                .ToDictionary(k => k.Skill, v => v.Level);
            userService.Update(user);
            return RedirectToAction("Index");
        }

        #region Manager specific

        public ActionResult Filter()
        {
            return View("Filter", new FilterViewModel
            {
                Skills = skillService.GetAll().Select(x => x.ToMvc()).ToList()
            });
        }

        [HttpPost]
        public ActionResult Filter(FilterViewModel model)
        {
            SaveFilter(model);

            return RedirectToAction("Index");
        }

        public ActionResult EditFilter(int filterID)
        {
            BalManager user = (BalManager)userService.Find(User.Identity.Name);
            BalFilter filter = user.Filters.FirstOrDefault(x => x.Id == filterID);

            if (filter != null)
            {
                FilterViewModel filterViewModel = ExtractSkills(filter);
                return View("Filter", filterViewModel);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult DeleteFilter(int filterId)
        {
            BalManager user = (BalManager)userService.Find(User.Identity.Name);
            BalFilter filter = user.Filters.FirstOrDefault(x => x.Id == filterId);
            if (filter == null)
                return HttpNotFound();

            user.Filters.Remove(filter);
            userService.Update(user);
            return RedirectToAction("Index");
        }

        public ActionResult Browse(int filterId = 0)
        {
            BalFilter filter;
            if (filterId != 0)
            {
                BalManager manager = (BalManager)userService.Find(User.Identity.Name);
                filter = manager.Filters.FirstOrDefault(x => x.Id == filterId);
            }
            else filter = new BalFilter();
            FilterViewModel filterViewModel = ExtractSkills(filter);
            filterViewModel.LastViewed = DateTime.Now;
            if (filterId != 0)
                SaveFilter(filterViewModel);
            return Browse(filterViewModel);
        }

        [HttpPost]
        public ActionResult Browse(FilterViewModel model, int page = 0)
        {
            if (Request.Form["save"] != null)
                SaveFilter(model);
            var neededSkills = model.Skills.Where(x => x.Include).Select(x => new BalUserSkill { Skill = x.Skill.Skill, Level = x.Skill.Level });
            var users = userService.Get(neededSkills).Skip(page*usersPerPage).Take(usersPerPage).Cast<BalProgrammer>();
            BrowseViewModel browseModel = new BrowseViewModel
            {
                Filter = model,
                Users = users.ToList()
            };
            return View(browseModel);
        }

        private FilterViewModel ExtractSkills(BalFilter filter)
        {
            var allSkills = skillService.GetAll().Select(x => x.ToMvc()).ToList();
            foreach (var skill in filter.Skills)
            {
                FilterSkillViewModel fsvm = allSkills.First(x => x.Skill.Skill.Id == skill.Key.Id);
                fsvm.Skill.Level = skill.Value;
                fsvm.Include = true;
            }

            FilterViewModel filterViewModel = new FilterViewModel
            {
                Id = filter.Id,
                Skills = allSkills
            };

            return filterViewModel;
        }

        private void SaveFilter(FilterViewModel model)
        {
            BalManager user = (BalManager)userService.Find(User.Identity.Name);
            BalFilter filter = model.ToBal();
            BalFilter userfilter = user.Filters.FirstOrDefault(x => x.Id == filter.Id);

            if (userfilter == null)
                user.Filters.Add(filter);
            else
            {
                userfilter.Skills = filter.Skills;
                userfilter.LastViewed = filter.LastViewed;
            }
            userService.Update(user);
        }

        #endregion
    }
}