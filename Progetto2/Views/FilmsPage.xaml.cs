using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Progetto2.Models;

namespace Progetto2.Views
{
    public sealed partial class FilmsPage : Page
    {
        private List<FilmSwapi> tuttiIFilm = new List<FilmSwapi>();
        private ObservableCollection<FilmSwapi> filmFiltrati = new ObservableCollection<FilmSwapi>();

        public FilmsPage()
        {
            this.InitializeComponent();
            FilmsListView.ItemsSource = filmFiltrati;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaFilmAsync();
            if (e.Parameter is CategoriaFiltroArgs parametri)
            {
                MessageTextBlock.Text = parametri.MessaggioDescrizione;
                MessageTextBlock.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                Films_Scelte(parametri.Urls);
            }
        }

        private async Task CaricaFilmAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // L'API swapi.info/api/films restituisce l'array con tutti i film
                    string json = await client.GetStringAsync("https://swapi.info/api/films");
                    var risultato = JsonSerializer.Deserialize<List<FilmSwapi>>(json);

                    if (risultato != null)
                    {
                        // Ordina i film per numero di episodio
                        tuttiIFilm = risultato.OrderBy(f => f.EpisodeId).ToList();
                        AggiornaLista(tuttiIFilm);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tuttiIFilm);
            }
            else
            {
                var risultati = tuttiIFilm
                    .Where(f => f.Title.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }

        private void Films_Scelte(List<string> urls)
        {
            var filmSelezionati = tuttiIFilm
                .Where(f => urls.Contains(f.Url))
                .ToList();
            AggiornaLista(filmSelezionati);
        }

        private void AggiornaLista(IEnumerable<FilmSwapi> lista)
        {
            filmFiltrati.Clear();
            foreach (var film in lista)
            {
                filmFiltrati.Add(film);
            }
        }

        private void FilmListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FilmSwapi filmSelezionato)
            {
                // Naviga verso la pagina di dettaglio passando il film selezionato
                Frame.Navigate(typeof(DetailViews.FilmDetailPage), filmSelezionato);
            }
        }
    }
}