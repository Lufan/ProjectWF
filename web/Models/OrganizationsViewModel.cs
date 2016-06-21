using DomainLayer.Contact;

namespace web.Models
{
    public class OrganizationsViewModel
    {
        public string Id { get; set; }

        public string OrganizationName { get; set; }

        public IAdress Adress { get; set; }

        public string Remarks { get; set; }
    }
}