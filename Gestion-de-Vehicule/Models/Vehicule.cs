using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_de_Vehicule.Models
{
    public abstract class Vehicule
    {

        public int Id { get; set; }
        public string Marque { get; set; }
        public string Modele { get; set; }
        public abstract string Type { get; set; }

        protected Vehicule(string marque, string modele)
        {
            Marque = marque;
            Modele = modele;
        }
    }
}
