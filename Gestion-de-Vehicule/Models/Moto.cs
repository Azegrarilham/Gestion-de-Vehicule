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

        private string _type = "Moto";
        public override string Type
        {
            get => _type;
            set => _type = value;
        }

        public Moto(string marque, string modele, int cylindree)
            : base(marque, modele)
        {
            Cylindree = cylindree;
        }
    }
}
