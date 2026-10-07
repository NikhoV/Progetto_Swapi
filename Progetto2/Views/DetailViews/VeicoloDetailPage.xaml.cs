using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Progetto2.DbModels;
using Progetto2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Progetto2.Views.DetailViews
{
    public sealed partial class VeicoloDetailPage : Page
    {
        private VeicoloSwapi veicoloCorrente;

        public VeicoloDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is VeicoloSwapi veicolo)
            {
                veicoloCorrente = veicolo;
                PopolaDettagliVeicolo();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (veicoloCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se il veicolo è già stato salvato in passato (per ID o per Nome)
                var veicoloEsistente = db.Veicoli.FirstOrDefault(v => v.Name == veicoloCorrente.Name);

                if (veicoloEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovoVeicoloOffline = new VeicoloCache  
                    {
                        Name = veicoloCorrente.Name,
                        Model = veicoloCorrente.Model,
                        Manufacturer = veicoloCorrente.Manufacturer,
                        CostInCredits = veicoloCorrente.CostInCredits,
                        Length = veicoloCorrente.Length,
                        MaxAtmospheringSpeed = veicoloCorrente.MaxAtmospheringSpeed,
                        Crew = veicoloCorrente.Crew,
                        Passengers = veicoloCorrente.Passengers,
                        VehicleClass = veicoloCorrente.VehicleClass,
                        SavedAt = DateTime.Now
                    };

                    db.Veicoli.Add(nuovoVeicoloOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    veicoloEsistente.Name = veicoloCorrente.Name;
                    veicoloEsistente.Model = veicoloCorrente.Model;
                    veicoloEsistente.Manufacturer = veicoloCorrente.Manufacturer;
                    veicoloEsistente.CostInCredits = veicoloCorrente.CostInCredits;
                    veicoloEsistente.Length = veicoloCorrente.Length;
                    veicoloEsistente.MaxAtmospheringSpeed = veicoloCorrente.MaxAtmospheringSpeed;
                    veicoloEsistente.Crew = veicoloCorrente.Crew;
                    veicoloEsistente.Passengers = veicoloCorrente.Passengers;
                    veicoloEsistente.VehicleClass = veicoloCorrente.VehicleClass;
                    veicoloEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }
        private void PopolaDettagliVeicolo()
        {
            // Intestazione (Sinistra)
            NameTextBlock.Text = veicoloCorrente.Name;
            ModelTextBlock.Text = $"Modello: {veicoloCorrente.Model}";
            ManufacturerClassTextBlock.Text = $"Produttore: {veicoloCorrente.Manufacturer}\nClasse: {veicoloCorrente.VehicleClass}";

            // Specifiche e Dettagli completi (Destra)
            CostTextBlock.Text = $"• Costo in crediti: {veicoloCorrente.CostInCredits}";
            LengthTextBlock.Text = $"• Lunghezza: {veicoloCorrente.Length} m";
            SpeedTextBlock.Text = $"• Vel. Max Atmosferica: {veicoloCorrente.MaxAtmospheringSpeed}";
            CrewPassengersTextBlock.Text = $"• Equipaggio: {veicoloCorrente.Crew} | Passeggeri: {veicoloCorrente.Passengers}";
            CargoConsumablesTextBlock.Text = $"• Capacità Carico: {veicoloCorrente.CargoCapacity} | Consumabili: {veicoloCorrente.Consumables}";

            // Conteggi per i bottoni delle categorie
            PilotiCountText.Text = $"{veicoloCorrente.Pilots?.Count ?? 0} presenti";
            FilmCountText.Text = $"{veicoloCorrente.Films?.Count ?? 0} presenti";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void PilotiCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = veicoloCorrente.Pilots?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = veicoloCorrente.Name,
                MessaggioDescrizione = $"Piloti del veicolo {veicoloCorrente.Name}, Quantità: {quantita}",
                Urls = veicoloCorrente.Pilots ?? new List<string>()
            };
            Frame.Navigate(typeof(PersonaggiPage), parametri);
        }

        private void FilmCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = veicoloCorrente.Films?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = veicoloCorrente.Name,
                MessaggioDescrizione = $"Film in cui appare il veicolo {veicoloCorrente.Name}, Quantità: {quantita}",
                Urls = veicoloCorrente.Films ?? new List<string>()
            };
            Frame.Navigate(typeof(FilmsPage), parametri);
        }
    }
}