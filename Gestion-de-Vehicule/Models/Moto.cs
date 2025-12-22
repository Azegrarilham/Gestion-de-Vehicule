using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestion_de_Vehicule.Models
{
    public class Moto : Vehicule
    {
        public int Cylindree { get; set; }

        public Moto()
        {
            Type = "Moto";
        }
    }
}
