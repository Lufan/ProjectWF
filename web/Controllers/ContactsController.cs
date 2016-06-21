using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;


using web.Infrastructure;
using DomainLayer.Contact;
using web.Models;


namespace web.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        public ContactsController(IQueryManager<IContact> contactQM, IQueryManager<IOrganization> organizationQM)
        {
            this.contactQM = contactQM;
            this.organizationQM = organizationQM;
        }

        // GET: Contact
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.isAuthenticated = Request.IsAuthenticated;
            ViewBag.CurrentPage = "contacts";

            var contacts = GetViewModel(10, 0);

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact(string id)
        {
            if (id != null)
            {

            }
            ViewBag.CurrentPage = "contacts";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactsViewModel model)
        {
            //TO DO - validate and save data
            ViewBag.CurrentPage = "contacts";
            return Redirect("Index");
        }

        private IEnumerable<ContactsViewModel> GetViewModel(int count = 10, int start_pos = 0)
        {
            var contacts = contactQM.TakeNAsync(count, start_pos);
            IList<ContactsViewModel> result = new List<ContactsViewModel>(count);
            foreach (var contact in contacts.Result)
            {
                var organizationName = "";
                // TO DO - select all names using only one query to database 
                if (contact.OrganizationId != null)
                {
                    organizationName = organizationQM.FindByIdAsync(contact.OrganizationId).Result.OrganizationName;
                }
                result.Add(
                    new ContactsViewModel
                    {
                        Id = contact.Id,
                        Name = contact.Name,
                        Shurname = contact.Shurname,
                        Patronymic = contact.Patronymic,
                        Emails = contact.Emails,
                        Phones = contact.Phones,
                        OrganizationId = contact.OrganizationId,
                        OrganizationName = organizationName,
                        Position = contact.Position,
                        Remarks = contact.Remarks
                    });
            }
            return result.AsEnumerable();
        }

        private IQueryManager<IContact> contactQM;
        private IQueryManager<IOrganization> organizationQM;
    }
}