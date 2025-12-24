using Gestion_de_Vehicule.Command;
using Gestion_de_Vehicule.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;

namespace Gestion_de_Vehicule.ViewModel
{
    internal class MainViewModel: INotifyPropertyChanged
    {
        string marque;
        string modele;
        int nomberPlaces;
        int cylindree;
        double capaciteCharge;
        string typeVehicule = "Voiture";
        string filtreType = "Tous";
        Vehicule selectedVehicule;
        public ObservableCollection<Vehicule> Vehicules { get; set; }= new ObservableCollection<Vehicule>();
        public ObservableCollection<Vehicule> VehiculesFiltres { get; set; } = new ObservableCollection<Vehicule>();
        public string Marque
        {
            get => marque;
            set
            {
                marque = value;
                OnPropertyChanged();
            }
        }
        public string Modele
        {
            get => modele;
            set
            {
                modele = value;
                OnPropertyChanged();
            }
        }
        public int NomberPlaces
        {
            get => nomberPlaces;
            set
            {
                nomberPlaces = value;
                OnPropertyChanged();
            }
        }
        public int Cylindree
        {
            get => cylindree;
            set
            {
                cylindree = value;
                OnPropertyChanged();
            }
        }
        public double CapaciteCharge
        {
            get => capaciteCharge;
            set
            {
                capaciteCharge = value;
                OnPropertyChanged();
            }
        }
        public string TypeVehicule
        {
            get => typeVehicule;
            set
            {
                typeVehicule = value;
                OnPropertyChanged();
            }
        }
        public string FiltreType
        {
            get => filtreType;
            set
            {
                filtreType = value;
                OnPropertyChanged();
                FilterVehiculesInfo(null);
            }
        }

        string filePath = "Data/Vehicules.json";
        public ICommand ChargerVehicules { get; }
        public ICommand AfficherVehicules { get; }
        public ICommand AjouterVehicules { get; }

        public Vehicule SelectedVehicule
        {
            get => selectedVehicule;
            set
            {
                selectedVehicule = value;
                OnPropertyChanged();
            }
        }
        
        public MainViewModel()
        {
            // Constructor logic here

            AfficherVehicules = new RelayCommand(AfficherVehiculesInfo);
            AjouterVehicules = new RelayCommand(AjouterVehiculeInfo);
            AfficherVehiculesInfo(null);
        }
        
        private void AfficherVehiculesInfo(Object obj)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    var loaded = JsonConvert.DeserializeObject<ObservableCollection<Vehicule>>(json, settings);
                    //var vehicules = JsonSerializer.Deserialize<List<Vehicule>>(json);
                    Vehicules.Clear();
                    foreach (var V in loaded)
                    {
                        Vehicules.Add(V);
                    }
                    FilterVehiculesInfo(null);
                }
                else
                {
                    MessageBox.Show("fichier non trouvé : " + Path.GetFullPath(filePath));
                }              
            }
            catch (Exception ex)
            {
                // Handle error 
               MessageBox.Show($"Erreur de chargement: {ex.Message}");
            }
        }
        void AjouterVehiculeInfo(Object obj)
        {
            if (string.IsNullOrWhiteSpace(Marque) || string.IsNullOrWhiteSpace(Modele))
                return;

            Vehicule vehicule = null;
            switch (TypeVehicule)
            {
                case "Voiture":
                    vehicule = new Voiture(Marque, Modele, NomberPlaces);
                    break;
                case "Moto":
                    vehicule = new Moto(Marque, Modele, Cylindree);
                    break;
                case "Camion":
                    vehicule = new Camion(Marque, Modele, CapaciteCharge);
                    break;
            }
            if (vehicule != null)
            {
                Vehicules.Add(vehicule);
                Vehicule.SauvegarderVehicules(Vehicules, filePath);
                FilterVehiculesInfo(null);
                ResetInputFields();
            }
        }
        void FilterVehiculesInfo(Object obj)
        {
            VehiculesFiltres.Clear();
            var filtres = FiltreType == "Tous"
                ? Vehicules
                : Vehicules.Where(v => v.Type == FiltreType);

            foreach (var v in filtres)
                VehiculesFiltres.Add(v);
        }
        void ResetInputFields()
        {
            Marque = string.Empty;
            Modele = string.Empty;
            NomberPlaces = 0;
            Cylindree = 0;
            CapaciteCharge = 0.0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
