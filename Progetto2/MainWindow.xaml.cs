using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Progetto2.DbModels;
using Progetto2.Views;
using System.Linq;

namespace Progetto2
{
    public sealed partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            this.InitializeComponent();
            Instance = this; // Riferimento statico per aggiornare la finestra da LoginPage
            AggiornaStatoInterfaccia();
        }

        public void AggiornaStatoInterfaccia()
        {
            using (var db = new SwapiDbContext())
            {
                db.Database.EnsureCreated();

                var sessione = db.UserSessions.FirstOrDefault(s => s.IsLoggedIn);

                if (sessione != null)
                {
                    // Utente Loggato: Mostra Header e Categorie
                    UserHeaderGrid.Visibility = Visibility.Visible;
                    CategoriesPanel.Visibility = Visibility.Visible;
                    TxtUsernameHeader.Text = sessione.Username;

                    // Apri la prima pagina (es. FilmsPage)
                    RootFrame.Navigate(typeof(FilmsPage));
                }
                else
                {
                    // Non Loggato: Nascondi Header e Categorie, mostra solo il Login a tutto schermo
                    UserHeaderGrid.Visibility = Visibility.Collapsed;
                    CategoriesPanel.Visibility = Visibility.Collapsed;

                    RootFrame.Navigate(typeof(LoginPage));
                }
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new SwapiDbContext())
            {
                var sessioni = db.UserSessions.ToList();
                db.UserSessions.RemoveRange(sessioni);
                db.SaveChanges();
            }

            AggiornaStatoInterfaccia(); // Ritorna al Login a tutto schermo
        }

        private void BtnOfflineSection_Click(object sender, RoutedEventArgs e)
        {
            RootFrame.Navigate(typeof(OfflinePage));
        }

        private void BtnCategoria_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button bottoneCliccato)
            {
                // 2. Recuperiamo il valore della proprietà Tag
                string categoria = bottoneCliccato.Tag?.ToString();

                // 3. Gestiamo la navigazione con una sola funzione
                switch (categoria)
                {
                    case "Films":
                        RootFrame.Navigate(typeof(Views.FilmsPage));
                        break;
                    case "Personaggi":
                        RootFrame.Navigate(typeof(Views.PersonaggiPage));
                        break;
                    case "Pianeti":
                        RootFrame.Navigate(typeof(Views.PianetiPage));
                        break;
                    case "Specie":
                        RootFrame.Navigate(typeof(Views.SpeciePage));
                        break;
                    case "Veicoli":
                        RootFrame.Navigate(typeof(Views.VeicoliPage));
                        break;
                    case "Navicelle":
                        RootFrame.Navigate(typeof(Views.NavicellePage));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}