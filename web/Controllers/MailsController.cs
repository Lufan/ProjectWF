using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    [Authorize]
    public class MailsController : Controller
    {
        // GET: Mails
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
    }
}