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
    public class ContactsController : Controller
    {
        public ContactsController(IQueryManager<Contact> contactQM, IQueryManager<Organization> organizationQM)
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

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> Contact(string id)
        {
            ViewBag.CurrentPage = "contacts";
            if (id != null)
            {
                ViewBag.Title = "Редактор контакта";
                try
                {
                    var contact = await contactQM.FindByIdAsync(id);
                    if (contact != null)
                    {
                        var organizationName = "";
                        if (contact.OrganizationId != null)
                        {
                            organizationName = (await organizationQM.FindByIdAsync(contact.OrganizationId)).OrganizationName;
                        }
                        var result = new ContactsViewModel
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
                        };
                        return View(result);
                    }
                }
                catch (System.Exception ex)
                {
                    return HttpNotFound(ex.ToString());
                }
            }
            ViewBag.Title = "Новый контакт";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactsViewModel model)
        {
            //TO DO - validate and save data
            return Redirect("/");
        }

        private IQueryManager<Contact> contactQM;
        private IQueryManager<Organization> organizationQM;
    }
}