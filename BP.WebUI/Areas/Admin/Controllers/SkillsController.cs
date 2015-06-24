using BP.BLL.Interface.Entities;
using BP.BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BP.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SkillsController : GenericController<BllSkill>
    {
        public SkillsController(IService<BllSkill> service): base(service) { }
    }
}