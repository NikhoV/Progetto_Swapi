using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Progetto2.DbModels;
using Progetto2.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

namespace Progetto2.Views.DetailViews
{
    public sealed partial class FilmDetailPage : Page
    {
        private FilmSwapi filmCorrente;

        public FilmDetailPage()
        {
            // È sufficiente questo: le GridView e le ObservableCollection 
            // non servono più in questa pagina.
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is FilmSwapi film)
            {
                filmCorrente = film;
                PopolaDettagliFilm();
            }
        }

        private async void BtnSalvaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (filmCorrente == null) return;

            using (var db = new SwapiDbContext())
            {
                // Verifica se il film è già stato salvato in passato (per ID o per Titolo)
                var filmEsistente = db.Films.FirstOrDefault(f => f.Title == filmCorrente.Title);

                if (filmEsistente == null)
                {
                    // Aggiunge un nuovo record nel DB
                    var nuovoFilmOffline = new FilmCache
                    {
                        Title = filmCorrente.Title,
                        EpisodeId = filmCorrente.EpisodeId,
                        Director = filmCorrente.Director,
                        Producer = filmCorrente.Producer,
                        ReleaseDate = filmCorrente.ReleaseDate,
                        OpeningCrawl = filmCorrente.OpeningCrawl,
                        SavedAt = DateTime.Now
                    };

                    db.Films.Add(nuovoFilmOffline);
                }
                else
                {
                    // Aggiorna i dati se già esistente
                    filmEsistente.Title = filmCorrente.Title;
                    filmEsistente.Director = filmCorrente.Director;
                    filmEsistente.Producer = filmCorrente.Producer;
                    filmEsistente.OpeningCrawl = filmCorrente.OpeningCrawl;
                    filmEsistente.SavedAt = DateTime.Now;
                }

                // Salva le modifiche in SQLite
                await db.SaveChangesAsync();
            }

            // Mostra la notifica visiva all'utente
            InfoBarNotifica.IsOpen = true;
        }

        private void PopolaDettagliFilm()
        {
            TitleTextBlock.Text = filmCorrente.Title;
            EpisodeTextBlock.Text = $"Episodio {filmCorrente.EpisodeId}";
            DirectorProducerTextBlock.Text = $"Regia: {filmCorrente.Director} | Produzione: {filmCorrente.Producer}";
            ReleaseDateTextBlock.Text = $"Data di uscita: {filmCorrente.ReleaseDate}";
            CrawlTextBlock.Text = filmCorrente.OpeningCrawlNoIndent;

            // Imposta i conteggi sui tre bottoni della pagina
            NavicelleCountText.Text = $"{filmCorrente.Starships?.Count ?? 0} presenti";
            VeicoliCountText.Text = $"{filmCorrente.Vehicles?.Count ?? 0} presenti";
            PersonaggiCountText.Text = $"{filmCorrente.Characters?.Count ?? 0} presenti";
            PianetiCountText.Text = $"{filmCorrente.Planets?.Count ?? 0} presenti";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        // Eventi Click dei bottoni
        private void NavicelleCategory_Click(object sender, RoutedEventArgs e)
        {
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = filmCorrente.Title,
                MessaggioDescrizione = $"Navicelle apparse nel film: {filmCorrente.Title}, Quantità: {filmCorrente.Starships.Count}",
                Urls = filmCorrente.Starships
            };
            Frame.Navigate(typeof(NavicellePage), parametri);
        }

        private void VeicoliCategory_Click(object sender, RoutedEventArgs e)
        {
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = filmCorrente.Title,
                MessaggioDescrizione = $"Veicoli apparsi nel film: {filmCorrente.Title}, Quantità: {filmCorrente.Vehicles.Count}",
                Urls = filmCorrente.Vehicles
            };
            Frame.Navigate(typeof(VeicoliPage), parametri);
        }

        private void PersonaggiCategory_Click(object sender, RoutedEventArgs e)
        {
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = filmCorrente.Title,
                MessaggioDescrizione = $"Personaggi apparsi nel film: {filmCorrente.Title}, Quantità: {filmCorrente.Characters.Count}",
                Urls = filmCorrente.Characters
            };
            Frame.Navigate(typeof(PersonaggiPage), parametri);
        }

        private void PianetiCategory_Click(object sender, RoutedEventArgs e)
        {
            var parametri = new CategoriaFiltroArgs
            {
                OrigineCorrelato = filmCorrente.Title,
                MessaggioDescrizione = $"Pianeti apparsi nel film: {filmCorrente.Title}, Quantità: {filmCorrente.Planets.Count}",
                Urls = filmCorrente.Planets
            };
            Frame.Navigate(typeof(PianetiPage), parametri);
        }
    }
}