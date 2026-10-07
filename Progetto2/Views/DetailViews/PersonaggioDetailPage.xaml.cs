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
    public sealed partial class PersonaggioDetailPage : Page
    {
        private PersonaggioSwapi personaggioCorrente;

        public PersonaggioDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is PersonaggioSwapi personaggio)
            {
                personaggioCorrente = personaggio;
                PopolaDettagliPersonaggio();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (personaggioCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se il personaggio è già stato salvato in passato (per ID o per Nome)
                var personaggioEsistente = db.Personaggi.FirstOrDefault(p =>  p.Name == personaggioCorrente.Name);

                if (personaggioEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovoPersonaggioOffline = new PersonaggioCache
                    {
                        Name = personaggioCorrente.Name,
                        Gender = personaggioCorrente.Gender,
                        BirthYear = personaggioCorrente.BirthYear,
                        Height = personaggioCorrente.Height,
                        Mass = personaggioCorrente.Mass,
                        SavedAt = DateTime.Now
                    };

                    db.Personaggi.Add(nuovoPersonaggioOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    personaggioEsistente.Name = personaggioCorrente.Name;
                    personaggioEsistente.Gender = personaggioCorrente.Gender;
                    personaggioEsistente.BirthYear = personaggioCorrente.BirthYear;
                    personaggioEsistente.Height = personaggioCorrente.Height;
                    personaggioEsistente.Mass = personaggioCorrente.Mass;
                    personaggioEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }

        private void PopolaDettagliPersonaggio()
        {
            // Intestazione (Sinistra)
            NameTextBlock.Text = personaggioCorrente.Name;
            GenderBirthTextBlock.Text = $"Genere: {personaggioCorrente.Gender} | Anno di nascita: {personaggioCorrente.BirthYear}";

            // Dati Fisici e Biografici (Destra)
            HeightTextBlock.Text = $"• Altezza: {personaggioCorrente.Height} cm";
            MassTextBlock.Text = $"• Peso: {personaggioCorrente.Mass} kg";
            HairColorTextBlock.Text = $"• Colore Capelli: {personaggioCorrente.HairColor}";
            SkinColorTextBlock.Text = $"• Colore Pelle: {personaggioCorrente.SkinColor}";
            EyeColorTextBlock.Text = $"• Colore Occhi: {personaggioCorrente.EyeColor}";

            // Conteggi per i bottoni delle categorie
            FilmCountText.Text = $"{personaggioCorrente.Films?.Count ?? 0} presenti";
            NavicelleCountText.Text = $"{personaggioCorrente.Starships?.Count ?? 0} presenti";
            VeicoliCountText.Text = $"{personaggioCorrente.Vehicles?.Count ?? 0} presenti";
            SpecieCountText.Text = $"{personaggioCorrente.Species?.Count ?? 0} presenti";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void FilmCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = personaggioCorrente.Films?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = personaggioCorrente.Name,
                MessaggioDescrizione = $"Film in cui appare {personaggioCorrente.Name}, Quantità: {quantita}",
                Urls = personaggioCorrente.Films ?? new List<string>()
            };
            Frame.Navigate(typeof(FilmsPage), parametri);
        }

        private void NavicelleCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = personaggioCorrente.Starships?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = personaggioCorrente.Name,
                MessaggioDescrizione = $"Navicelle pilotate da {personaggioCorrente.Name}, Quantità: {quantita}",
                Urls = personaggioCorrente.Starships ?? new List<string>()
            };
            Frame.Navigate(typeof(NavicellePage), parametri);
        }

        private void VeicoliCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = personaggioCorrente.Vehicles?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = personaggioCorrente.Name,
                MessaggioDescrizione = $"Veicoli guidati da {personaggioCorrente.Name}, Quantità: {quantita}",
                Urls = personaggioCorrente.Vehicles ?? new List<string>()
            };
            Frame.Navigate(typeof(VeicoliPage), parametri);
        }

        private void SpecieCategory_Click(object sender, RoutedEventArgs e)
        {
            int quantita = personaggioCorrente.Species?.Count ?? 0;
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = personaggioCorrente.Name,
                MessaggioDescrizione = $"Specie di appartenenza per {personaggioCorrente.Name}, Quantità: {quantita}",
                Urls = personaggioCorrente.Species ?? new List<string>()
            };
            Frame.Navigate(typeof(SpeciePage), parametri);
        }
    }
}