using System.Collections.Generic;

namespace DomainLayer.Contact
{
    public sealed class Adress : IAdress
    {
        public Adress()
        { }

        public Adress(IAdress adress)
        {
            if (adress != null)
            {
                Country = adress.Country;
                District = adress.District;
                Region = adress.Region;
                City = adress.City;
                Index = adress.Index;
                AdressLines = adress.AdressLines;
                Phones = adress.Phones;
            }
        }

        public string Country { get; set; }

        public string District { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public string Index { get; set; }

        private IList<string> _adressLines;
        public IList<string> AdressLines
        {
            get
            {
                if (_adressLines == null)
                {
                    _adressLines = new List<string>();
                }
                return _adressLines;
            }
            set
            {
                _adressLines = value != null ? new List<string>(value) : null;
            }
        }

        private IEnumerable<Email> _emails;
        //<email adress, note>
        public IEnumerable<Email> Emails
        {
            get
            {
                if (_emails == null)
                {
                    _emails = new List<Email>();
                }
                return _emails;
            }
            set
            {
                _emails = value != null ? new List<Email>(value) : null;
            }
        }

        private IEnumerable<Phone> _phones;
        //<phone number, note>
        public IEnumerable<Phone> Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new List<Phone>();
                }
                return _phones;
            }
            set
            {
                _phones = value != null ? new List<Phone>(value) : null;
            }
        }
    }
}
