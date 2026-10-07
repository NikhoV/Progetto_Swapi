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
    public sealed partial class PersonaggiPage : Page
    {
        private List<PersonaggioSwapi> tuttiIPersonaggi = new List<PersonaggioSwapi>();
        private ObservableCollection<PersonaggioSwapi> personaggiFiltrati = new ObservableCollection<PersonaggioSwapi>();

        public PersonaggiPage()
        {
            this.InitializeComponent();
            PersonaggiListView.ItemsSource = personaggiFiltrati;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await CaricaPersonaggiAsync();
            if (e.Parameter is CategoriaFiltroArgs parametri)
            {
                MessageTextBlock.Text = parametri.MessaggioDescrizione;
                MessageTextBlock.Visibility = Visibility.Visible;
                Personaggi_Scelte(parametri.Urls);
            }
        }

        private async Task CaricaPersonaggiAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Endpoint per la lista completa dei personaggi
                    string json = await client.GetStringAsync("https://swapi.info/api/people");
                    var risultato = JsonSerializer.Deserialize<List<PersonaggioSwapi>>(json);

                    if (risultato != null)
                    {
                        tuttiIPersonaggi = risultato;
                        AggiornaLista(tuttiIPersonaggi);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Errore API Personaggi: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string ricerca = SearchTextBox.Text;

            if (string.IsNullOrWhiteSpace(ricerca))
            {
                AggiornaLista(tuttiIPersonaggi);
            }
            else
            {
                var risultati = tuttiIPersonaggi
                    .Where(p => p.Name != null && p.Name.Contains(ricerca, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                AggiornaLista(risultati);
            }
        }

        public void Personaggi_Scelte(List<string> Personaggi)
        {
            var risultati = tuttiIPersonaggi
                .Where(p => p.Url != null && Personaggi.Contains(p.Url))
                .ToList();

            AggiornaLista(risultati);
        }

        private void AggiornaLista(IEnumerable<PersonaggioSwapi> lista)
        {
            personaggiFiltrati.Clear();
            foreach (var p in lista)
            {
                personaggiFiltrati.Add(p);
            }
        }

        private void PersonaggiListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is PersonaggioSwapi personaggio)
            {
                Frame.Navigate(typeof(DetailViews.PersonaggioDetailPage), personaggio);
            }
        }
    }
}