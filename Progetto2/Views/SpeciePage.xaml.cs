using Microsoft.UI.Xaml;
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
    public sealed partial class SpeciePage : Page
    {
        private List<SpecieSwapi> tutteLeSpecie = new List<SpecieSwapi>();
        private ObservableCollection<SpecieSwapi> specieFiltrate = new ObservableCollection<SpecieSwapi>();

        public SpeciePage()
        {
            this.InitializeComponent();
            SpecieListView.ItemsSource = specieFiltrate;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaSpecieAsync();
            if (e.Parameter is CategoriaFiltroArgs parametri)
            {
                MessageTextBlock.Text = parametri.MessaggioDescrizione;
                MessageTextBlock.Visibility = Visibility.Visible;
                Specie_Scelte(parametri.Urls);
            }
        }

        private async Task CaricaSpecieAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Endpoint per la lista completa delle specie
                    string json = await client.GetStringAsync("https://swapi.info/api/species");
                    var risultato = JsonSerializer.Deserialize<List<SpecieSwapi>>(json);

                    if (risultato != null)
                    {
                        tutteLeSpecie = risultato;
                        AggiornaLista(tutteLeSpecie);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API Specie: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tutteLeSpecie);
            }
            else
            {
                var risultati = tutteLeSpecie
                    .Where(s => s.Name != null && s.Name.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }

        private void Specie_Scelte(List<string> specie)
        {
            var specieSelezionate = tutteLeSpecie
                .Where(s => specie.Contains(s.Url))
                .ToList();
            AggiornaLista(specieSelezionate);
        }

        private void AggiornaLista(IEnumerable<SpecieSwapi> lista)
        {
            specieFiltrate.Clear();
            foreach (var s in lista)
            {
                specieFiltrate.Add(s);
            }
        }

        private void SpecieListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is SpecieSwapi specieSelezionata)
            {
                Frame.Navigate(typeof(DetailViews.SpecieDetailPage), specieSelezionata);
            }
        }
    }
}