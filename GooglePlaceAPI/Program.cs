using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace GooglePlaceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //string url = @"https://maps.googleapis.com/maps/api/place/textsearch/json?query=cabinet+medicale+lyon&key=AIzaSyA5FnFVac0QH1fxjQrute3Zn2xY9IGkm6I";
                string txt = File.ReadAllText(@"C:\Users\Desbois\Desktop\recherche didou\jsonv1.txt");
                GetPlaceDescription(GetPlaceId(txt));
            }
            catch (Exception)
            {
                throw;
            }           
        }

        public static List<string> GetPlaceId(string json)
        {
            try
            {
                List<string> lId = new List<string>();
                Dictionary<string, object> test = JsonConvert.DeserializeObject < Dictionary < string, object>>(json);
                Newtonsoft.Json.Linq.JArray tes = (Newtonsoft.Json.Linq.JArray)test["results"];
                for(int i = 0; i < tes.Count; i++)
                {
                    Newtonsoft.Json.Linq.JToken tok = (Newtonsoft.Json.Linq.JToken)tes[i]["place_id"];
                    lId.Add(tok.ToString());
                }
                return lId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void GetPlaceDescription(List<string> lPlaceId)
        {
            try
            {
                List<Places> lPlace = new List<Places>();
                foreach(string id in lPlaceId)
                {
                    string html = "";
                    string url = @"https://maps.googleapis.com/maps/api/place/details/json?placeid=" + id+"&key=AIzaSyA5FnFVac0QH1fxjQrute3Zn2xY9IGkm6I";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        html = reader.ReadToEnd();
                    }
                    Places p = new Places();
                    Dictionary<string, object> test = JsonConvert.DeserializeObject<Dictionary<string, object>>(html);
                    Newtonsoft.Json.Linq.JObject tes = (Newtonsoft.Json.Linq.JObject)test["result"];
                    string fullAdresse = ((Newtonsoft.Json.Linq.JToken)tes["formatted_address"]).ToString();
                    p.Route = fullAdresse.Split(',')[0];
                    p.Cp = Convert.ToInt32(fullAdresse.Split(',')[1].Trim().Substring(0,5));
                    p.Ville = fullAdresse.Split(',')[1].Trim().Substring(5);

                    p.Tel= Convert.ToInt32(((Newtonsoft.Json.Linq.JToken)tes["formatted_phone_number"]).ToString().Replace(" ",""));
                    p.Nom = ((Newtonsoft.Json.Linq.JToken)tes["name"]).ToString();
                    List<Newtonsoft.Json.Linq.JToken> lOpen = (List<Newtonsoft.Json.Linq.JToken>)tes["opening_hours"].Children().ToList();
                    Newtonsoft.Json.Linq.JToken lWeek = lOpen[2];
                    string[] val = lWeek.ToString().Split(',');
                    Dictionary<string, string> dicoHoraire = new Dictionary<string, string>();
                    foreach(string s in val)
                    {

                        string[] jourHoraire = s.Split(':');
                        if(jourHoraire[1].Trim() != "Closed")
                        {
                            string[] heurs = jourHoraire[1].Trim().Split('-');
                            string[] h1 = heurs[0].Trim().Split(' ');
                            string[] h2 = heurs[1].Trim().Split(' ');

                            int heure1 = Convert.ToInt32(h1[0]);
                            if (h1[1].Contains("PM"))
                                heure1 += 12;

                            int heure2 = Convert.ToInt32(h2[0]);
                            if (h2[1].Contains("PM"))
                                heure2 += 12;
                            dicoHoraire.Add(jourHoraire[0], heure1 + " - " + heure2);
                        }
                        else
                            dicoHoraire.Add(jourHoraire[0],"Fermé");
                    }
                    p.Horaires = dicoHoraire;
                    p.Specialite = "";
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
