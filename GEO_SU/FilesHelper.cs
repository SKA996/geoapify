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



    }
}
