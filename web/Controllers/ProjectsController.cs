using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        // GET: Projects
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}