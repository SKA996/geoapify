using GEO_SU.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEO_SU {
    public class FilesHelper {
        private string input = $"{Directory.GetCurrentDirectory()}/in/";
        private string output = $"{Directory.GetCurrentDirectory()}/out/";

        public FilesHelper() {

            if (!Directory.Exists(input)) {
                Directory.CreateDirectory(input);
            }

            if (!Directory.Exists(output)) {
                Directory.CreateDirectory(output);
            }
        }


        public List<Address> GetAddressesFromJson(string json, string id) {


            List<Address> addresses = new List<Address>();

            JObject res = JObject.Parse(json);

            List<JToken>? results = res["results"]?.ToList();

            foreach (JToken item in results) {

                if ((item["street"] != null && item["lat"] != null && item["lon"] != null) && item["housenumber"] != null) {

                    string city = item["city"] != null ? item["city"].ToString() : "";

                    string country = item["country"] != null ? item["country"].ToString() : "";

                    string district = item["district"] != null ? item["district"].ToString() : "";

                    string housenumber = item["housenumber"] !=null? item["housenumber"].ToString() : "";

                    string street = item["street"] !=null ? item["street"].ToString() :"";

                    string postcode = item["postcode"] != null ? item["postcode"].ToString() : "";

                    string region = item["region"] != null ? item["region"].ToString() : "";

                    string suburb = item["suburb"] != null ? item["suburb"].ToString() : "";


                    double lat = item["lat"] !=null ? double.Parse(item["lat"].ToString()) : double.NaN;
                    double lon = item["lon"] !=null ? double.Parse(item["lon"].ToString()): double.NaN;

                    Address address = new Address() { Id=id, City = city, Country = country, District = district, Housenumber = housenumber, Street = street, Postcode = postcode, Region = region, Suburb = suburb, Lat = lat, Lon = lon };
                    addresses.Add(address);
                }
            }
            return addresses;
        }

       
           
       

        public List<string> GetRequestFiles() {
            return Directory.GetFiles(input).ToList();
        }

        public List<string> GetRequestStringFromFiles(string file) {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(file) && File.Exists(file)) {
                result = File.ReadAllLines(file).ToList();
            }
            return result;
        }


        public List<string[]> GetRequestsString(List<string> strings) {


            if (strings.Count() > 0) {
                List<string[]> strs = new List<string[]>();

                foreach (string item in strings) {

                    string[] arr = item.Split(';');
                    strs.Add(arr);
                }

                return strs;
            } else { 
                return new List<string[]>();
            }            
        }

        public void WriteFileResult(string file, List<List<Address>> listArdsResults) {
            string fileResult = $"{output}{Path.GetFileNameWithoutExtension(file)}_out.csv";

            foreach (List<Address> addresses in listArdsResults) {

                foreach (Address address in addresses) {
                   // string lat = address.Lat.ToString();
                   // string lon = address.Lon.ToString();
                    if (address != null && !string.IsNullOrEmpty(address.Housenumber) && !string.IsNullOrEmpty(address.Street) && (!double.IsNaN(address.Lat) && !double.IsNaN(address.Lon))) {

                        string res = $"{address.Id};{address.City};{address.Street};{address.Housenumber};{address.Lat};{address.Lon};{Environment.NewLine}";

                        using (FileStream fs = new FileStream(fileResult, FileMode.Append, FileAccess.Write)) {
                            byte[] buffer = Encoding.Default.GetBytes(res);
                            fs.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }
        }


    }
}

