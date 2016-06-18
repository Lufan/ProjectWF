using System.Web.Mvc;

using web.Infrastructure;
using DomainLayer.Contact;
using web.Models;


namespace web.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        public ContactsController(ContactsQueryManager contactQM, OrganizationsQueryManager organizationQM)
        {
            this.contactQM = contactQM;
            this.organizationQM = organizationQM;
        }

        // GET: Contact
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {

            }
            ViewBag.CurrentPage = "contacts";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact(string id)
        {
            ViewBag.CurrentPage = "contacts";
            return View();
        }

        private ContactsQueryManager contactQM;
        private OrganizationsQueryManager organizationQM;

        //magic default value
        //TO DO: 
        private const int defaultCountsPerPage = 20;
    }
}