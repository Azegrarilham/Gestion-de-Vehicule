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

        public override string Type => "Voiture";
        public override string InfoSpesifique => $"{NombrePlaces}";


        public Voiture(string marque, string modele, int nombrePlaces) : base(marque, modele)
        {
            NombrePlaces = nombrePlaces;
        }
    }
}
