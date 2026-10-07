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
    public sealed partial class PianetaDetailPage : Page
    {
        private PianetaSwapi pianetaCorrente;

        public PianetaDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PianetaSwapi pianeta)
            {
                pianetaCorrente = pianeta;
                PopolaDettagliPianeta();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (pianetaCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se il pianeta è già stato salvato in passato (per ID o per Nome)
                var pianetaEsistente = db.Pianeti.FirstOrDefault(p => p.Name == pianetaCorrente.Name);

                if (pianetaEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovoPianetaOffline = new PianetaCache
                    {
                        Name = pianetaCorrente.Name,
                        Climate = pianetaCorrente.Climate,
                        Terrain = pianetaCorrente.Terrain,
                        Diameter = pianetaCorrente.Diameter,
                        Gravity = pianetaCorrente.Gravity,
                        Population = pianetaCorrente.Population,
                        SavedAt = DateTime.Now
                    };

                    db.Pianeti.Add(nuovoPianetaOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    pianetaEsistente.Name = pianetaCorrente.Name;
                    pianetaEsistente.Climate = pianetaCorrente.Climate;
                    pianetaEsistente.Terrain = pianetaCorrente.Terrain;
                    pianetaEsistente.Diameter = pianetaCorrente.Diameter;
                    pianetaEsistente.Gravity = pianetaCorrente.Gravity;
                    pianetaEsistente.Population = pianetaCorrente.Population;
                    pianetaEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }
        private void PopolaDettagliPianeta()
        {
            // Intestazione (Sinistra)
            NameTextBlock.Text = pianetaCorrente.Name;
            ClimateTerrainTextBlock.Text = $"Clima: {pianetaCorrente.Climate} | Terreno: {pianetaCorrente.Terrain}";

            // Dettagli Ambientali e Astronomici (Destra)
            DiameterTextBlock.Text = $"• Diametro: {pianetaCorrente.Diameter} km";
            GravityTextBlock.Text = $"• Gravità: {pianetaCorrente.Gravity}";
            PopulationTextBlock.Text = $"• Popolazione: {pianetaCorrente.Population}";
            PeriodsTextBlock.Text = $"• Rotazione: {pianetaCorrente.RotationPeriod}h | Orbita: {pianetaCorrente.OrbitalPeriod} giorni";
            SurfaceWaterTextBlock.Text = $"• Acqua in superficie: {pianetaCorrente.SurfaceWater}%";

            // Conteggi per i bottoni delle categorie
            ResidentiCountText.Text = $"{pianetaCorrente.Residents?.Count ?? 0} presenti";
            FilmCountText.Text = $"{pianetaCorrente.Films?.Count ?? 0} presenti";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void ResidentiCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = pianetaCorrente.Residents?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = pianetaCorrente.Name,
                MessaggioDescrizione = $"Residenti originari del pianeta {pianetaCorrente.Name}, Quantità: {quantita}",
                Urls = pianetaCorrente.Residents ?? new List<string>()
            };
            Frame.Navigate(typeof(PersonaggiPage), parametri);
        }

        private void FilmCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = pianetaCorrente.Films?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = pianetaCorrente.Name,
                MessaggioDescrizione = $"Film in cui appare il pianeta {pianetaCorrente.Name}, Quantità: {quantita}",
                Urls = pianetaCorrente.Films ?? new List<string>()
            };
            Frame.Navigate(typeof(FilmsPage), parametri);
        }
    }
}