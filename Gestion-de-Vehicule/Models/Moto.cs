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
        public override string Type => "Moto";
        public override string InfoSpesifique => $"{Cylindree}";


        public Moto(string marque, string modele, int cylindree) : base(marque, modele)
        {
            Cylindree = cylindree;
        }
    }
}
