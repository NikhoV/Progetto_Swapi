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
    public sealed partial class NavicellePage : Page
    {
        private List<NavicellaSwapi> tutteLeNavicelle = new List<NavicellaSwapi>();
        private ObservableCollection<NavicellaSwapi> navicelleFiltrate = new ObservableCollection<NavicellaSwapi>();

        public NavicellePage()
        {
            this.InitializeComponent();
            NavicelleListView.ItemsSource = navicelleFiltrate;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaNavicelleAsync();
            if (e.Parameter is CategoriaFiltroArgs categoriaFiltro)
            {
                MessageTextBlock.Text = categoriaFiltro.MessaggioDescrizione;
                MessageTextBlock.Visibility = Visibility.Visible;
                Navicelle_Scelte(categoriaFiltro.Urls);
            }
        }

        private async Task CaricaNavicelleAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Endpoint per la lista completa delle navicelle
                    string json = await client.GetStringAsync("https://swapi.info/api/starships");
                    var risultato = JsonSerializer.Deserialize<List<NavicellaSwapi>>(json);

                    if (risultato != null)
                    {
                        tutteLeNavicelle = risultato;
                        AggiornaLista(tutteLeNavicelle);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API Navicelle: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tutteLeNavicelle);
            }
            else
            {
                var risultati = tutteLeNavicelle
                    .Where(n => n.Name != null && n.Name.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }

        public void Navicelle_Scelte(List<string> Starships)
        {
            var risultati = tutteLeNavicelle;
            risultati = tutteLeNavicelle
                .Where(n => n.Url != null && Starships.Contains(n.Url))
                .ToList();

            AggiornaLista(risultati);
        }

        private void AggiornaLista(IEnumerable<NavicellaSwapi> lista)
        {
            navicelleFiltrate.Clear();
            foreach (var n in lista)
            {
                navicelleFiltrate.Add(n);
            }
        }

        private void NavicelleListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is NavicellaSwapi navicella)
            {
                Frame.Navigate(typeof(Views.DetailViews.NavicellaDetailPage), navicella);
            }
        }
    }
}