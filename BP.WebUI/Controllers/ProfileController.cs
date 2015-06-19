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
        private int usersPerPage = 2;
        private IService<BllSkill> skillService;
        private readonly IUserService userService;
        private readonly string defaultImagePath = @"~/Content/Images/User.png";
        private readonly string defaultImageType = ".png";

        public ProfileController(IUserService userService, IService<BllSkill> skillService)
        {
            this.userService = userService;
            this.skillService = skillService;
        }

        public ActionResult Index(int? id = null)
        {
            BllUser user;
            if (id != null)
                user = userService.Find((int)id);
            else user = userService.Find(User.Identity.Name);

            if (user is BllProgrammer)
                return View("ProgrammerProfile", (BllProgrammer)user);
            else if (user is BllManager)
                return View("ManagerProfile", (BllManager)user);
            else return RedirectToAction("Index", "Home");
        }

        #region Programmer specific

        public FileResult GetPhoto(int id)
        {
            BllProgrammer user = (BllProgrammer)userService.Find(id);
            if (user.Photo != null && user.ImageType != null)
                return File(user.Photo, user.ImageType);
            else return File(Server.MapPath(defaultImagePath), defaultImageType);
        }

        [Authorize(Roles = "Programmer")]
        public ActionResult EditInfo()
        {
            BllProgrammer user = (BllProgrammer)userService.Find(User.Identity.Name);
            UserInfoViewModel ui = new UserInfoViewModel();
            ui.GetInfo(user);
            return View(ui);
        }

        [HttpPost]
        [Authorize(Roles = "Programmer")]
        public ActionResult EditInfo(UserInfoViewModel userInfo)
        {
            BllProgrammer user = (BllProgrammer)userService.Find(User.Identity.Name);
            userInfo.SetUserInfo(user);
            userService.Update(user);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Programmer")]
        public ActionResult EditSkills()
        {
            BllProgrammer user = (BllProgrammer)userService.Find(User.Identity.Name);

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
            BllProgrammer user = (BllProgrammer)userService.Find(model.Id);
            user.Skills = model.Skills
                .Where(x => x.Level != 0)
                .ToDictionary(k => k.Skill, v => v.Level);
            userService.Update(user);
            return RedirectToAction("Index");
        }

        #endregion

        #region Manager specific

        [Authorize(Roles = "Manager")]
        public ActionResult Filter(int filterId = 0)
        {
            BllFilter filter = null;
            FilterViewModel filterModel;

            if (filterId != 0)
            {
                BllManager user = (BllManager)userService.Find(User.Identity.Name);
                filter = user.Filters.FirstOrDefault(x => x.Id == filterId);
            }

            filterModel = ExtractSkills(filter);

            return View("Filter", filterModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Filter(FilterViewModel model)
        {
            SaveFilter(model);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult DeleteFilter(int filterId)
        {
            BllManager user = (BllManager)userService.Find(User.Identity.Name);
            BllFilter filter = user.Filters.FirstOrDefault(x => x.Id == filterId);
            if (filter == null)
                return HttpNotFound();

            user.Filters.Remove(filter);
            userService.Update(user);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Browse(int Id)
        {
            BllFilter filter;
            if (Id != 0)
            {
                BllManager manager = (BllManager)userService.Find(User.Identity.Name);
                filter = manager.Filters.FirstOrDefault(x => x.Id == Id);
            }
            else filter = new BllFilter();
            FilterViewModel filterViewModel = ExtractSkills(filter);
            filterViewModel.LastViewed = DateTime.Now;
            if (Id != 0)
                SaveFilter(filterViewModel);
            return Browse(filterViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult Browse(FilterViewModel model)
        {
            if (Request.Form["save"] != null)
            {
                model.LastViewed = DateTime.Now;
                SaveFilter(model);
            }
            TempData["filter"] = model;
            return View(model);
        }

        public ActionResult GetUsers(int page = 0)
        {
            FilterViewModel model = (FilterViewModel)TempData["filter"];
            TempData["filter"] = model;

            var neededSkills = model.Skills.Where(x => x.Include).Select(x => new BllUserSkill { Skill = x.Skill.Skill, Level = x.Skill.Level });
            var users = userService.Get(neededSkills);
            var usersForPage = users.Skip(page * usersPerPage).Take(usersPerPage).Cast<BllProgrammer>();
            BrowseViewModel browseModel = new BrowseViewModel
            {
                Filter = model,
                Users = usersForPage.ToList(),
                Page = page,
                PageCount = users.Count() / usersPerPage
            };

            return PartialView("_UsersPartial", browseModel);
        }

        [Authorize(Roles = "Manager")]
        private FilterViewModel ExtractSkills(BllFilter filter)
        {
            int id = filter != null ? filter.Id : 0;
            var allSkills = skillService.GetAll().Select(x => x.ToMvc()).ToList();

            if (filter != null)
            {
                foreach (var skill in filter.Skills)
                {
                    FilterSkillViewModel fsvm = allSkills.First(x => x.Skill.Skill.Id == skill.Key.Id);
                    fsvm.Skill.Level = skill.Value;
                    fsvm.Include = true;
                }
            }

            FilterViewModel filterViewModel = new FilterViewModel
            {
                Id = id,
                Skills = allSkills
            };

            return filterViewModel;
        }

        [Authorize(Roles = "Manager")]
        private void SaveFilter(FilterViewModel model)
        {
            BllManager user = (BllManager)userService.Find(User.Identity.Name);
            BllFilter filter = model.ToBal();
            BllFilter userfilter = user.Filters.FirstOrDefault(x => x.Id == filter.Id);

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

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            skillService.Dispose();
            base.Dispose(disposing);
        }
    }
}