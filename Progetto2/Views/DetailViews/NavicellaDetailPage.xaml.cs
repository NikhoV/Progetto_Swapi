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
    public sealed partial class NavicellaDetailPage : Page
    {
        private NavicellaSwapi navicellaCorrente;

        public NavicellaDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is NavicellaSwapi navicella)
            {
                navicellaCorrente = navicella;
                PopolaDettagliNavicella();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (navicellaCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se la navicella è già stata salvata in passato (per ID o per Titolo)
                var navicellaEsistente = db.Navicelle.FirstOrDefault(n => n.Name == navicellaCorrente.Name);

                if (navicellaEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovaNavicellaOffline = new NavicellaCache
                    {
                        Name = navicellaCorrente.Name,
                        Model = navicellaCorrente.Model,
                        Manufacturer = navicellaCorrente.Manufacturer,
                        CostInCredits = navicellaCorrente.CostInCredits,
                        Length = navicellaCorrente.Length,
                        MaxAtmospheringSpeed = navicellaCorrente.MaxAtmospheringSpeed,
                        Crew = navicellaCorrente.Crew,
                        Passengers = navicellaCorrente.Passengers,
                        StarshipClass = navicellaCorrente.StarshipClass,
                        SavedAt = DateTime.Now
                    };

                    db.Navicelle.Add(nuovaNavicellaOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    navicellaEsistente.Name = navicellaCorrente.Name;
                    navicellaEsistente.Model = navicellaCorrente.Model;
                    navicellaEsistente.Manufacturer = navicellaCorrente.Manufacturer;
                    navicellaEsistente.CostInCredits = navicellaCorrente.CostInCredits;
                    navicellaEsistente.Length = navicellaCorrente.Length;
                    navicellaEsistente.MaxAtmospheringSpeed = navicellaCorrente.MaxAtmospheringSpeed;
                    navicellaEsistente.Crew = navicellaCorrente.Crew;
                    navicellaEsistente.Passengers = navicellaCorrente.Passengers;
                    navicellaEsistente.StarshipClass = navicellaCorrente.StarshipClass;
                    navicellaEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }
        private void PopolaDettagliNavicella()
        {
            NameTextBlock.Text = navicellaCorrente.Name;
            ModelTextBlock.Text = $"Modello: {navicellaCorrente.Model}";
            ManufacturerClassTextBlock.Text = $"Produttore: {navicellaCorrente.Manufacturer} | Classe: {navicellaCorrente.StarshipClass}";
            SpecsTextBlock.Text = $"Costo: {navicellaCorrente.CostInCredits} crediti | Lunghezza: {navicellaCorrente.Length}m | Vel. Max: {navicellaCorrente.MaxAtmospheringSpeed}";
            CrewPassengersTextBlock.Text = $"Equipaggio: {navicellaCorrente.Crew} | Passeggeri: {navicellaCorrente.Passengers}";

            // Conteggi per i bottoni delle categorie correlate
            PilotiCountText.Text = $"{navicellaCorrente.Pilots?.Count ?? 0} presenti";
            FilmCountText.Text = $"{navicellaCorrente.Films?.Count ?? 0} presenti";

            // Specifiche e Dettagli completi (Destra)
            SpeedTextBlock.Text = $"• Vel. Max Atmosferica: {navicellaCorrente.MaxAtmospheringSpeed}";
            HyperdriveMgltTextBlock.Text = $"• Hyperdrive Rating: {navicellaCorrente.HyperdriveRating} | MGLT: {navicellaCorrente.Mglt}";
            CrewPassengersTextBlock.Text = $"• Equipaggio: {navicellaCorrente.Crew} | Passeggeri: {navicellaCorrente.Passengers}";
            CargoConsumablesTextBlock.Text = $"• Capacità Carico: {navicellaCorrente.CargoCapacity} | Consumabili: {navicellaCorrente.Consumables}";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        // Navigazione verso i Piloti (Personaggi) della navicella
        private void PilotiCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = navicellaCorrente.Pilots?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = navicellaCorrente.Name,
                MessaggioDescrizione = $"Piloti della navicella: {navicellaCorrente.Name}, Quantità: {quantita}",
                Urls = navicellaCorrente.Pilots ?? new List<string>()
            };
            Frame.Navigate(typeof(PersonaggiPage), parametri);
        }

        private void FilmCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = navicellaCorrente.Films?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = navicellaCorrente.Name,
                MessaggioDescrizione = $"Film in cui appare la navicella: {navicellaCorrente.Name}, Quantità: {quantita}",
                Urls = navicellaCorrente.Films ?? new List<string>()
            };
            Frame.Navigate(typeof(FilmsPage), parametri);
        }
    }
}