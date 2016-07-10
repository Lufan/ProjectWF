using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using web.Infrastructure;
using DomainLayer.Contact;
using web.Models;


namespace web.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        public OrganizationsController(IQueryManager<Organization> organizationQM)
        {
            this.organizationQM = organizationQM;
        }

        // GET: Organization
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.isAuthenticated = Request.IsAuthenticated;
            ViewBag.CurrentPage = "organizations";

            return View();
        }

        private IQueryManager<Organization> organizationQM;
    }
}