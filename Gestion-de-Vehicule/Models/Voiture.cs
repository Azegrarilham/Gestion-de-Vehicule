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

        public Voiture()
        {
            Type = "Voiture";
        }
    }
}
