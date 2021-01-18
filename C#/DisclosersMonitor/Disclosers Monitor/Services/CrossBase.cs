using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace Disclosers_Monitor.Services
{
    class CrossBase
    {
        private string db_name;

        public CrossBase(string dbName) => db_name = dbName;


        private static string baseLink = "http://crossbase.herokuapp.com/";
        public string getData(string keyString = null)
        {
            string requestLink;
            if (keyString == null)
            {
                Console.WriteLine("None of keys got, moving on");
                requestLink = baseLink + "get/" + db_name;

            }
            else
            {
                requestLink = baseLink + "get-by-keyword/" + db_name + "/" + keyString;
                Console.WriteLine(keyString);
            }
            var request = (HttpWebRequest)WebRequest.Create(requestLink);
            var response = request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            response.Close();
            return responseString;
        }

        public void setData(string key, string value)
        {
            string requestLink = baseLink + "set/" + db_name + "/" + key + "/" + value;
            var request = (HttpWebRequest)WebRequest.Create(requestLink);
            var response = request.GetResponse();
            response.Close();
        }
    }
}
