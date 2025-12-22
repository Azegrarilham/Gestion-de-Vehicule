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

        public Camion()
        {
            Type = "Camion";
        }
    }
}
