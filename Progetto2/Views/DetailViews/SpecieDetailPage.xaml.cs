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
    public sealed partial class SpecieDetailPage : Page
    {
        private SpecieSwapi specieCorrente;

        public SpecieDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is SpecieSwapi specie)
            {
                specieCorrente = specie;
                PopolaDettagliSpecie();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (specieCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se la specie è già stata salvata in passato (per ID o per Nome)
                var specieEsistente = db.Specie.FirstOrDefault(s => s.Name == specieCorrente.Name);

                if (specieEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovaSpecieOffline = new SpecieCache
                    {
                        Name = specieCorrente.Name,
                        Classification = specieCorrente.Classification,
                        Designation = specieCorrente.Designation,
                        Language = specieCorrente.Language,
                        AverageHeight = specieCorrente.AverageHeight,
                        SavedAt = DateTime.Now
                    };

                    db.Specie.Add(nuovaSpecieOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    specieEsistente.Name = specieCorrente.Name;
                    specieEsistente.Classification = specieCorrente.Classification;
                    specieEsistente.Designation = specieCorrente.Designation;
                    specieEsistente.Language = specieCorrente.Language;
                    specieEsistente.AverageHeight = specieCorrente.AverageHeight;
                    specieEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }
        private void PopolaDettagliSpecie()
        {
            // Intestazione (Sinistra)
            NameTextBlock.Text = specieCorrente.Name;
            ClassificationDesignationTextBlock.Text = $"Classificazione: {specieCorrente.Classification} | Designazione: {specieCorrente.Designation}";

            // Dettagli Biologici e Linguistici (Destra)
            LanguageTextBlock.Text = $"• Lingua parlata: {specieCorrente.Language}";
            AverageHeightTextBlock.Text = $"• Altezza media: {specieCorrente.AverageHeight} cm";
            AverageLifespanTextBlock.Text = $"• Vita media: {specieCorrente.AverageLifespan} anni";
            SkinColorsTextBlock.Text = $"• Colori pelle: {specieCorrente.SkinColors}";
            HairColorsTextBlock.Text = $"• Colori capelli: {specieCorrente.HairColors}";
            EyeColorsTextBlock.Text = $"• Colori occhi: {specieCorrente.EyeColors}";

            // Conteggi per i bottoni delle categorie
            PersonaggiCountText.Text = $"{specieCorrente.People?.Count ?? 0} presenti";
            FilmCountText.Text = $"{specieCorrente.Films?.Count ?? 0} presenti";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void PersonaggiCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = specieCorrente.People?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = specieCorrente.Name,
                MessaggioDescrizione = $"Membri della specie {specieCorrente.Name}, Quantità: {quantita}",
                Urls = specieCorrente.People ?? new List<string>()
            };
            Frame.Navigate(typeof(PersonaggiPage), parametri);
        }

        private void FilmCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = specieCorrente.Films?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = specieCorrente.Name,
                MessaggioDescrizione = $"Film in cui appare la specie {specieCorrente.Name}, Quantità: {quantita}",
                Urls = specieCorrente.Films ?? new List<string>()
            };
            Frame.Navigate(typeof(FilmsPage), parametri);
        }
    }
}