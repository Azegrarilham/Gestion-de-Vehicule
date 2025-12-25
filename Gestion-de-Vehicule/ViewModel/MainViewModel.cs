using Gestion_de_Vehicule.Command;
using Gestion_de_Vehicule.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;

namespace Gestion_de_Vehicule.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        string marque;
        string modele;
        int nomberPlaces;
        int cylindree;
        double capaciteCharge;
        string typeVehicule = "Voiture";
        string filtreType = "Tous";
        Vehicule selectedVehicule;
        bool isEditing = false;

        public ObservableCollection<Vehicule> Vehicules { get; set; } = new ObservableCollection<Vehicule>();
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
                // Clear specific fields when type changes
                if (value != "Voiture") NomberPlaces = 0;
                if (value != "Moto") Cylindree = 0;
                if (value != "Camion") CapaciteCharge = 0.0;
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

        public Vehicule SelectedVehicule
        {
            get => selectedVehicule;
            set
            {
                selectedVehicule = value;
                OnPropertyChanged();
                LoadSelectedVehicule();
            }
        }

        string filePath = "Data/Vehicules.json";

        public ICommand AfficherVehicules { get; }
        public ICommand AjouterVehicules { get; }
        public ICommand ModifierVehicule { get; }
        public ICommand SupprimerVehicule { get; }
        public ICommand ResetCommand { get; }

        public MainViewModel()
        {
            AfficherVehicules = new RelayCommand(AfficherVehiculesInfo);
            AjouterVehicules = new RelayCommand(AjouterVehiculeInfo, CanAddOrUpdateVehicule);
            ModifierVehicule = new RelayCommand(ModifierVehiculeInfo, CanAddOrUpdateVehicule);
            SupprimerVehicule = new RelayCommand(SupprimerVehiculeInfo, CanDeleteVehicule);
            ResetCommand = new RelayCommand(ResetInputFieldsCommand);

            AfficherVehiculesInfo(null);
        }

        private bool CanAddOrUpdateVehicule(object obj)
        {
            return !string.IsNullOrWhiteSpace(Marque) && !string.IsNullOrWhiteSpace(Modele);
        }

        private bool CanDeleteVehicule(object obj)
        {
            return SelectedVehicule != null;
        }

        private void AfficherVehiculesInfo(object obj)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    var loaded = JsonConvert.DeserializeObject<ObservableCollection<Vehicule>>(json, settings);

                    Vehicules.Clear();
                    foreach (var V in loaded)
                    {
                        Vehicules.Add(V);
                    }
                    FilterVehiculesInfo(null);
                }
                else
                {
                    // Create directory if it doesn't exist
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    // Create empty file
                    File.WriteAllText(filePath, "[]");
                    MessageBox.Show("Fichier créé : " + Path.GetFullPath(filePath), "Information",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement: {ex.Message}", "Erreur",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void AjouterVehiculeInfo(object obj)
        {
            if (!ValidateInputs())
                return;

            Vehicule vehicule = CreateVehicule();
            Console.WriteLine(vehicule);
            if (vehicule != null)
            {
                Vehicules.Add(vehicule);
                selectedVehicule.SauvegarderVehicules(Vehicules, filePath);
                FilterVehiculesInfo(null);
                ResetInputFields();
                MessageBox.Show("Véhicule ajouté avec succès!", "Succès",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        void ModifierVehiculeInfo(object obj)
        {
            if (SelectedVehicule == null)
            {
                MessageBox.Show("Veuillez sélectionner un véhicule à modifier.", "Avertissement",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateInputs())
                return;

            var result = MessageBox.Show($"Voulez-vous vraiment modifier ce véhicule?\n" +
                                       $"{SelectedVehicule.Type} - {SelectedVehicule.Marque} {SelectedVehicule.Modele}",
                                       "Confirmation",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Remove old vehicle
                Vehicules.Remove(SelectedVehicule);

                // Create updated vehicle
                Vehicule updatedVehicule = CreateVehicule();
                if (updatedVehicule != null)
                {
                    Vehicules.Add(updatedVehicule);
                    selectedVehicule.SauvegarderVehicules(Vehicules, filePath);
                    FilterVehiculesInfo(null);
                    ResetInputFields();
                    MessageBox.Show("Véhicule modifié avec succès!", "Succès",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        void SupprimerVehiculeInfo(object obj)
        {
            if (SelectedVehicule == null)
            {
                MessageBox.Show("Veuillez sélectionner un véhicule à supprimer.", "Avertissement",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Voulez-vous vraiment supprimer ce véhicule?\n" +
                                       $"{SelectedVehicule.Type} - {SelectedVehicule.Marque} {SelectedVehicule.Modele}",
                                       "Confirmation",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                Vehicules.Remove(SelectedVehicule);
                selectedVehicule.SauvegarderVehicules(Vehicules, filePath);
                FilterVehiculesInfo(null);
                ResetInputFields();
                MessageBox.Show("Véhicule supprimé avec succès!", "Succès",
                              MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private Vehicule CreateVehicule()
        {
            Vehicule vehicule = null;
            switch (TypeVehicule)
            {
                case "Voiture":
                    if (NomberPlaces <= 0)
                    {
                        MessageBox.Show("Le nombre de places doit être supérieur à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                    vehicule = new Voiture(Marque, Modele, NomberPlaces);
                    break;

                case "Moto":
                    if (Cylindree <= 0)
                    {
                        MessageBox.Show("La cylindrée doit être supérieure à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                    vehicule = new Moto(Marque, Modele, Cylindree);
                    break;

                case "Camion":
                    if (CapaciteCharge <= 0)
                    {
                        MessageBox.Show("La capacité de charge doit être supérieure à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                    vehicule = new Camion(Marque, Modele, CapaciteCharge);
                    break;
            }
            return vehicule;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(Marque) || string.IsNullOrWhiteSpace(Modele))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Validate specific fields based on vehicle type
            switch (TypeVehicule)
            {
                case "Voiture":
                    if (NomberPlaces <= 0)
                    {
                        MessageBox.Show("Le nombre de places doit être supérieur à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    break;

                case "Moto":
                    if (Cylindree <= 0)
                    {
                        MessageBox.Show("La cylindrée doit être supérieure à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    break;

                case "Camion":
                    if (CapaciteCharge <= 0)
                    {
                        MessageBox.Show("La capacité de charge doit être supérieure à 0.", "Erreur",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    break;
            }

            return true;
        }

        private void LoadSelectedVehicule()
        {
            if (SelectedVehicule != null)
            {
                isEditing = true;
                Marque = SelectedVehicule.Marque;
                Modele = SelectedVehicule.Modele;
                TypeVehicule = SelectedVehicule.Type;

                if (SelectedVehicule is Voiture voiture)
                {
                    NomberPlaces = voiture.NombrePlaces;
                }
                else if (SelectedVehicule is Moto moto)
                {
                    Cylindree = moto.Cylindree;
                }
                else if (SelectedVehicule is Camion camion)
                {
                    CapaciteCharge = camion.CapaciteCharge;
                }
            }
        }

        void FilterVehiculesInfo(object obj)
        {
            VehiculesFiltres.Clear();
            var filtres = FiltreType == "Tous"
                ? Vehicules
                : Vehicules.Where(v => v.Type == FiltreType);

            foreach (var v in filtres)
                VehiculesFiltres.Add(v);
        }

        void ResetInputFieldsCommand(object obj)
        {
            ResetInputFields();
            SelectedVehicule = null;
            isEditing = false;
        }

        void ResetInputFields()
        {
            Marque = string.Empty;
            Modele = string.Empty;
            NomberPlaces = 0;
            Cylindree = 0;
            CapaciteCharge = 0.0;
            TypeVehicule = "Voiture";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}