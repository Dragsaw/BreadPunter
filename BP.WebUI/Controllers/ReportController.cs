using BP.BLL.Interface.Entities.Users;
using BP.BLL.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using SelectPdf;

namespace BP.WebUI.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ReportController : Controller
    {
        private readonly IService<BllUser> service;

        public ReportController(IService<BllUser> service)
        {
            this.service = service;
        }

        public void UpdateReport(string email)
        {
            if (Session["report"] == null)
                Session["report"] = new List<string>();

            var reportList = (List<string>)Session["report"];
            if (reportList.Contains(email))
                reportList.Remove(email);
            else reportList.Add(email);
        }

        public ActionResult Preview()
        {
            return View(GetUsers());
        }

        [HttpPost]
        public ActionResult Preview(string email)
        {
            var usersList = (List<string>)Session["report"];
            usersList.Remove(email);
            return RedirectToAction("Preview");
        }

        public void Download()
        {
            using (StringWriter sw = new StringWriter())
            {
                var preview = ViewEngineCollection.FindView(this.ControllerContext, "Report", null);
                ViewData.Add("users", GetUsers());
                var vc = new ViewContext(ControllerContext, preview.View, ViewData, TempData, sw);
                preview.View.Render(vc, sw);

                HtmlToPdf htp = new HtmlToPdf();
                PdfDocument doc = htp.ConvertHtmlString(sw.ToString());
                using (MemoryStream ms = new MemoryStream())
                {
                    doc.Save(System.Web.HttpContext.Current.Response, false, "BPReport.pdf");
                    doc.Close();
                }
            }
        }

        private IEnumerable<BllProgrammer> GetUsers()
        {
            List<BllProgrammer> users;
            if (Session["report"] == null)
                users = new List<BllProgrammer>();
            else
            {
                users = ((List<string>)Session["report"]).Select(x => (BllProgrammer)service.Find(x)).ToList();
            }

            return users;
        }

        protected override void Dispose(bool disposing)
        {
            service.Dispose();
            base.Dispose(disposing);
        }
    }
}