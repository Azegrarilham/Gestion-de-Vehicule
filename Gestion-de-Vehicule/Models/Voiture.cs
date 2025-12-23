using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_de_Vehicule.Models
{
    public class Voiture : Vehicule
    {
        public int NombrePlaces { get; set; }

        private string _type = "Voiture";
        public override string Type
        {
            get => _type;
            set => _type = value;
        }

        public Voiture(string marque, string modele, int nombrePlaces)
            : base(marque, modele)
        {
            NombrePlaces = nombrePlaces;
        }
    }
}
