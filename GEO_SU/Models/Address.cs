using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEO_SU.Models {
    public class Address {

        private string _id;
        private string _country;
        private string _region;
        private string _city;
        private string _postcode;
        private string _district;
        private string _suburb;
        private string _street;
        private string _housenumber;
        private double _lon;
        private double _lat;

        public string Id { get => _id; set => _id = value; }
        public string Country { get => _country; set => _country = value; }
        public string Region { get => _region; set => _region = value; }
        public string City { get => _city; set => _city = value; }
        public string Postcode { get => _postcode; set => _postcode = value; }
        public string District { get => _district; set => _district = value; }
        public string Suburb { get => _suburb; set => _suburb = value; }
        public string Street { get => _street; set => _street = value; }
        public string Housenumber { get => _housenumber; set => _housenumber = value; }
        public double Lon { get => _lon; set => _lon = value; }
        public double Lat { get => _lat; set => _lat = value; }
     

        public Address() { }
    }
}
