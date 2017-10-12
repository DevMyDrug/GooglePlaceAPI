using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GooglePlaceAPI
{
    class Places
    {
        string idplace = "";
        string nom= "";
        int numero = 0;
        string route = "";
        string ville = "";
        string specialite = "";
        int cp = 0;
        int tel = 0;
        Dictionary<string, string> horaires = new Dictionary<string, string>();

        public string Nom { get => nom; set => nom = value; }
        public int Numero { get => numero; set => numero = value; }
        public string Route { get => route; set => route = value; }
        public string Ville { get => ville; set => ville = value; }
        public string Specialite { get => specialite; set => specialite = value; }
        public int Cp { get => cp; set => cp = value; }
        public int Tel { get => tel; set => tel = value; }
        public Dictionary<string, string> Horaires { get => horaires; set => horaires = value; }

        public Places()
        {
        }
    }
}
