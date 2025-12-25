using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Gestion_de_Vehicule.Models
{
    public abstract class Vehicule
    {
        public string Marque { get; set; }
        public string Modele { get; set; }
        public abstract string Type { get; }
        public abstract string InfoSpesifique { get; }
        protected Vehicule(string marque, string modele)
        {
            Marque = marque;
            Modele = modele;
        }

        public void SauvegarderVehicules(ObservableCollection<Vehicule> vehicules, string filePath)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented
                };
                string json = JsonConvert.SerializeObject(vehicules, Newtonsoft.Json.Formatting.Indented, settings);

                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de sauvegarde: {ex.Message}", "Erreur",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
