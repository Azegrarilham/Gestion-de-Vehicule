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

        private string _type = "Camion";
        public override string Type
        {
            get => _type;
            set => _type = value;
        }

        public Camion(string marque, string modele, double capaciteCharge)
            : base(marque, modele)
        {
            CapaciteCharge = capaciteCharge;
        }
    }
}
