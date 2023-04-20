
using GEO_SU.Models;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace GEO_SU {
    public class Program {

        private static string country = "Russia";

        private static string BaseURL = "https://api.geoapify.com/v1/geocode/search?";// housenumber=7&street=Ярослава%20Гашека&city=Санкт-Петербург&country={country}&format=json&apiKey=9bfcc95b9a7440d7adb8b3b5e1b67f39";
        private static string urlParams = "lang=ru&format=json&apiKey=9bfcc95b9a7440d7adb8b3b5e1b67f39";
        private static string username = "mbr0969@gmail.com";
        private static string password = "7710118mM*";

        private const string userAgent =  "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/73.0.3683.103 Safari/537.36";

        public static void Main(string[] args) {

           

            FilesHelper filesHelper = new FilesHelper();

            List<string> filesRequest = filesHelper.GetRequestFiles();

            if (filesRequest.Count() > 0) {

                using (WebClient client = new WebClient()) {

                    client.Headers.Add("user-agent", userAgent);
                    client.Credentials = new NetworkCredential(username, password);

                    List<List<Address>> listAdrsResult = new();
                    foreach (string file in filesRequest) {

                        List<Address> adrsResult = new();
                        List<string> jsons = new();
                        List<string> requestStrings = filesHelper.GetRequestStringFromFiles(file);

                        if (requestStrings.Count() > 0) {
                            List<string[]> arrStr = filesHelper.GetRequestsString(requestStrings);

                            foreach (string[] str in arrStr) {
                                if (str.Length >= 4) {

                                    var id = str[0];
                                    var housenumber = str[1];
                                    var street = str[2];
                                    var city = str[3];

                                    string url = $"{BaseURL}housenumber={housenumber}&street={street}&city={city}&country={country}&{urlParams}";

                                    string request = HttpReqiestToApi(url, client);

                                    if (!string.IsNullOrEmpty(request)) {

                                        adrsResult =  filesHelper.GetAddressesFromJson(request, id);

                                        listAdrsResult.Add(adrsResult);

                                        Console.WriteLine($"Ответ на запрос с  ID = {id} добавлен для записи.");
                                    }

                                    Thread.Sleep(500);
                                }
                            }
                        }


                       //List<Address> adrsResult =  filesHelper.GetAddressesFromJson(jsons);

                        filesHelper.WriteFileResult(file, listAdrsResult);

                        Console.WriteLine($"Файл запросов с именем {Path.GetFileNameWithoutExtension(file)} обработан.");
                    }
                }
           }





            #region

            //JObject res = JObject.Parse(json);

            //List<JToken>? results = res["results"]?.ToList();

            //List<Address> addresses = new();            
            //foreach (JToken item in results) {

            //    if ((item["lat"] != null && item["lon"] !=null) && item["housenumber"] != null) {

            //        string city = item["city"] !=null ? item["city"].ToString() : "";
                    
            //        string country = item["country"] != null ? item["country"].ToString() : "";
                    
            //        string district = item["district"] != null ? item["district"].ToString() : "";

            //        string housenumber = item["housenumber"].ToString();

            //        string street = item["street"].ToString();

            //        string postcode = item["postcode"] != null ? item["postcode"].ToString() : ""; 
                    
            //        string region = item["region"] != null ? item["region"].ToString() : "" ;

            //        string suburb = item["region"] != null ? item["suburb"].ToString() : "" ;

            //        double lat = double.Parse(item["lat"].ToString());
            //        double lon = double.Parse(item["lon"].ToString());

            //        Address address = new Address() { City = city, Country = country, District = district, Housenumber = housenumber,  Street = street, Postcode = postcode,   Region = region,  Suburb = suburb,  Lat = lat,  Lon = lon };
            //        addresses.Add(address);
            //    }
            //}

            #endregion

            //var adr = addresses;
           

        }


      

        public static string HttpReqiestToApi(string url, WebClient client) {

            using (Stream stream = client.OpenRead(url)) {
                using (StreamReader reader = new StreamReader(stream)) {              
                    return reader.ReadToEnd();
                }
            }           
        }
         


        private static async Task<Stream> GetDataStream() {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(BaseURL, HttpCompletionOption.ResponseHeadersRead).Result;
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines() {

            using var data_sream = GetDataStream().Result;
            using var data_reader = new StreamReader(data_sream);

            while (!data_reader.EndOfStream) {
                var line = data_reader.ReadLine();
                //if (string.IsNullOrWhiteSpace(line)) continue;
                //if (line.Contains('"'))
                //line = line.Insert(line.IndexOf(',', line.IndexOf('"')) + 1, " -").Remove(line.IndexOf(',', line.IndexOf('"')), 1);
                yield return line;
            }
        }     




    }
}
