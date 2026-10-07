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
    public sealed partial class PianetiPage : Page
    {
        private List<PianetaSwapi> tuttiIPianeti = new List<PianetaSwapi>();
        private ObservableCollection<PianetaSwapi> pianetiFiltrati = new ObservableCollection<PianetaSwapi>();

        public PianetiPage()
        {
            this.InitializeComponent();
            PianetiListView.ItemsSource = pianetiFiltrati;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaPianetiAsync();
            if (e.Parameter is CategoriaFiltroArgs parametri)
            {
                MessageTextBlock.Text = parametri.MessaggioDescrizione;
                MessageTextBlock.Visibility = Visibility.Visible;
                Pianeti_Scelte(parametri.Urls);
            }
        }

        private async Task CaricaPianetiAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Endpoint per la lista completa dei pianeti
                    string json = await client.GetStringAsync("https://swapi.info/api/planets");
                    var risultato = JsonSerializer.Deserialize<List<PianetaSwapi>>(json);

                    if (risultato != null)
                    {
                        tuttiIPianeti = risultato;
                        AggiornaLista(tuttiIPianeti);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API Pianeti: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tuttiIPianeti);
            }
            else
            {
                var risultati = tuttiIPianeti
                    .Where(p => p.Name != null && p.Name.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }

        private void Pianeti_Scelte(List<string> urls)
        {
            var pianetiSelezionati = tuttiIPianeti
                .Where(p => urls.Contains(p.Url))
                .ToList();
            AggiornaLista(pianetiSelezionati);
        }

        private void AggiornaLista(IEnumerable<PianetaSwapi> lista)
        {
            pianetiFiltrati.Clear();
            foreach (var p in lista)
            {
                pianetiFiltrati.Add(p);
            }
        }

        private void PianetiListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is PianetaSwapi pianetaSelezionato)
            {
                Frame.Navigate(typeof(DetailViews.PianetaDetailPage), pianetaSelezionato);
            }
        }
    }
}