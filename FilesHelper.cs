using GEO_SU.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
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


        /// <summary> Получаем спсок адресов </summary><param name="json"></param><param name="id"></param>
        public List<Address> GetAddressesFromJson(string json, string id) {

            List<Address> addresses = new List<Address>();
            JObject res = JObject.Parse(json);

            List<JToken>? results = res["results"]?.ToList();
            foreach (JToken item in results) {

                if ((item["street"] != null && item["lat"] != null && item["lon"] != null) && item["housenumber"] != null) {

                    string state = item["state"] != null ? item["state"].ToString() : "";
                    string city = item["city"] != null ? item["city"].ToString() : "";
                    string country = item["country"] != null ? item["country"].ToString() : "";
                    string county = item["county"] != null ? item["county"].ToString() : "";
                    string district = item["district"] != null ? item["district"].ToString() : "";
                    string housenumber = item["housenumber"] != null ? item["housenumber"].ToString() : "";
                    string street = item["street"] != null ? item["street"].ToString() : "";
                    string postcode = item["postcode"] != null ? item["postcode"].ToString() : "";
                    string region = item["region"] != null ? item["region"].ToString() : "";
                    string suburb = item["suburb"] != null ? item["suburb"].ToString() : "";
                    double lat = item["lat"] != null ? double.Parse(item["lat"].ToString()) : double.NaN;
                    double lon = item["lon"] != null ? double.Parse(item["lon"].ToString()) : double.NaN;

                    Address address = new Address() { Id = id, County = county, City = city, State = state, Country = country, District = district, Housenumber = housenumber, Street = street, Postcode = postcode, Region = region, Suburb = suburb, Lat = lat, Lon = lon };
                    addresses.Add(address);
                }
            }
            return addresses;
        }


        /// <summary>Поулчем список фалов запроса</summary>
        public List<string> GetRequestFiles() {
            return Directory.GetFiles(input).ToList();
        }

        /// <summary> Получаем список строк запроса из файла запроса</summary> <param name="file"></param>
        public List<string> GetRequestStringFromFiles(string file) {
            List<string> result = new List<string>();
            if (!string.IsNullOrEmpty(file) && File.Exists(file)) {
                result = File.ReadAllLines(file).ToList();
            }
            return result;
        }


        /// <summary>Поулчаем список массивов строк запроса </summary><param name="strings"></param>
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

        /// <summary>ЗАписываем файл ответов на запрос</summary><param name="file"></param><param name="listArdsResults"></param>
        public void WriteFileResult(string file, List<List<Address>> listArdsResults, bool isSpb) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding ANSI = Encoding.GetEncoding("windows-1251");
            Encoding UTF8 = Encoding.UTF8;
            string fileResult = $"{output}{Path.GetFileNameWithoutExtension(file)}_out.csv";
            foreach (List<Address> addresses in listArdsResults) {
                foreach (Address address in addresses) {
                    // string lat = address.Lat.ToString();
                    // string lon = address.Lon.ToString();
                    if (address != null && !string.IsNullOrEmpty(address.Housenumber) && !string.IsNullOrEmpty(address.Street) && (!double.IsNaN(address.Lat) && !double.IsNaN(address.Lon))) {
                        if (isSpb) {
                            string res = $"{address.Id};{address.City};{address.Street};{address.Housenumber};{address.Lat};{address.Lon};{Environment.NewLine}";

                            using (FileStream fs = new FileStream(fileResult, FileMode.Append, FileAccess.Write)) {
                                byte[] buffer = Encoding.Convert(UTF8,ANSI, Encoding.Default.GetBytes(res));
                                fs.Write(buffer, 0, buffer.Length);
                            }
                        } else {
                            string res = $"{address.Id};{address.State} {address.County} {address.City};{address.Street};{address.Housenumber};{address.Lat};{address.Lon};{Environment.NewLine}";

                            using (FileStream fs = new FileStream(fileResult, FileMode.Append, FileAccess.Write)) {
                                byte[] buffer = Encoding.Convert(UTF8, ANSI, Encoding.Default.GetBytes(res));
                                fs.Write(buffer, 0, buffer.Length);
                            }
                        }
                       
                    }
                }
            }
        }

        /// <summary>ЗАпрос по СПб? </summary> <param name="fileName"></param> <returns></returns>
        public bool IsSPbOrLO(string fileName) {
          int i =   fileName.IndexOf("sp");
            if ( i != -1) {
                return true;
            } else {
                return false;
            }
        }

    }
}

