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
    public sealed partial class VeicoliPage : Page
    {
        private List<VeicoloSwapi> tuttiIVeicoli = new List<VeicoloSwapi>();
        private ObservableCollection<VeicoloSwapi> veicoliFiltrati = new ObservableCollection<VeicoloSwapi>();

        public VeicoliPage()
        {
            this.InitializeComponent();
            VeicoliListView.ItemsSource = veicoliFiltrati;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaVeicoliAsync();
            if (e.Parameter is CategoriaFiltroArgs categoriaFiltro)
            {
                MessageTextBlock.Text = categoriaFiltro.MessaggioDescrizione;
                MessageTextBlock.Visibility = Visibility.Visible;
                Veicoli_Scelti(categoriaFiltro.Urls);
            }
        }

        private async Task CaricaVeicoliAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Endpoint per la lista completa dei veicoli
                    string json = await client.GetStringAsync("https://swapi.info/api/vehicles");
                    var risultato = JsonSerializer.Deserialize<List<VeicoloSwapi>>(json);

                    if (risultato != null)
                    {
                        tuttiIVeicoli = risultato;
                        AggiornaLista(tuttiIVeicoli);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API Veicoli: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tuttiIVeicoli);
            }
            else
            {
                var risultati = tuttiIVeicoli
                    .Where(v => v.Name != null && v.Name.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }
        private void Veicoli_Scelti(List<string> urls)
        {
            var veicoliSelezionati = tuttiIVeicoli
                .Where(v => urls.Contains(v.Url))
                .ToList();
            AggiornaLista(veicoliSelezionati);
        }
        private void AggiornaLista(IEnumerable<VeicoloSwapi> lista)
        {
            veicoliFiltrati.Clear();
            foreach (var v in lista)
            {
                veicoliFiltrati.Add(v);
            }
        }

        private void VeicoliListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is VeicoloSwapi veicoloSelezionato)
            {
                Frame.Navigate(typeof(DetailViews.VeicoloDetailPage), veicoloSelezionato);
            }
        }
    }
}