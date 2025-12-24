using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_de_Vehicule.Models
{
    public class Camion : Vehicule
    {
        public double CapaciteCharge { get; set; }
        public override string Type => "Camion";
        public override string InfoSpesifique => $"{CapaciteCharge}";


        public Camion(string marque, string modele, double capaciteCharge) : base(marque, modele)
        {
            CapaciteCharge = capaciteCharge;
        }
    }
}
