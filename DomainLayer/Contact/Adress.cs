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
                City = adress.City;
                Index = adress.Index;
                AdressLines = adress.AdressLines;
                Phones = adress.Phones;
            }
        }

        public string Country { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string Index { get; set; }

        private IDictionary<string, string> _adressLines;
        public IDictionary<string, string> AdressLines
        {
            get
            {
                if (_adressLines == null)
                {
                    _adressLines = new Dictionary<string, string>();
                }
                return _adressLines;
            }
            set
            {
                _adressLines = value != null ? new Dictionary<string, string>(value) : null;
            }
        }

        private IDictionary<string, string> _phones;
        public IDictionary<string, string> Phones
        {
            get
            {
                if (_phones == null)
                {
                    _phones = new Dictionary<string, string>();
                }
                return _phones;
            }
            set
            {
                _phones = value != null ? new Dictionary<string, string>(value) : null;
            }
        }
    }
}
