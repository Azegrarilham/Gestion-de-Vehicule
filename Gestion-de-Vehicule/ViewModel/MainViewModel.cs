using Gestion_de_Vehicule.Command;
using Gestion_de_Vehicule.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace Gestion_de_Vehicule.ViewModel
{
    internal class MainViewModel: INotifyPropertyChanged
    {
        string marque;
        string modele;
        string nomberPlaces;
        string cylindree;
        string CapaciteCharge;
        Vehicule selectedVehicule;

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
        public string NomberPlaces
        {
            get => nomberPlaces;
            set
            {
                nomberPlaces = value;
                OnPropertyChanged();
            }
        }
        public string Cylindree
        {
            get => cylindree;
            set
            {
                cylindree = value;
                OnPropertyChanged();
            }
        }
        public string CapaciteCharge1
        {
            get => CapaciteCharge;
            set
            {
                CapaciteCharge = value;
                OnPropertyChanged();
            }
        }

        string filePath = "Data/Vehicules.json";

        public ICommand AfficherVehicules { get; }

        public Vehicule SelectedVehicule
        {
            get => selectedVehicule;
            set
            {
                selectedVehicule = value;
                OnPropertyChanged();
               
            }
        }
        public ObservableCollection<Vehicule> Vehicules { get; set; } = new ObservableCollection<Vehicule>();
        public MainViewModel()
        {
            // Constructor logic here
            AfficherVehicules = new RelayCommand(AfficherVehiculesInfo);
            
        }
        private void AfficherVehiculesInfo(Object obj)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);

                    var vehicules = JsonSerializer.Deserialize<List<Vehicule>>(json);
                    Vehicules.Clear();
                    foreach (var V in vehicules)
                    {
                        Vehicules.Add(V);
                    }
                }
                else
                {
                    MessageBox.Show("fichier non trouvé : " + Path.GetFullPath(filePath));
                }
            }
            catch (Exception ex)
            {
                // Handle error - you might want to show a message box
                System.Windows.MessageBox.Show($"Erreur de chargement: {ex.Message}");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
