using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Progetto2.DbModels;
using System;
using System.Linq;

namespace Progetto2.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = string.Empty;
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Inserisci username e password.";
                return;
            }

            using (var db = new SwapiDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    // Resetta sessioni precedenti
                    var oldSessions = db.UserSessions.ToList();
                    db.UserSessions.RemoveRange(oldSessions);

                    // Registra nuova sessione attiva
                    db.UserSessions.Add(new UserSession
                    {
                        Username = user.Username,
                        IsLoggedIn = true,
                        LastLogin = DateTime.Now
                    });
                    db.SaveChanges();

                    // Aggiorna l'interfaccia principale per mostrare l'Header e caricare la pagina principale
                    MainWindow.Instance?.AggiornaStatoInterfaccia();
                }
                else
                {
                    TxtError.Text = "Credenziali non valide.";
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            TxtError.Text = string.Empty;
            string username = TxtUsername.Text.Trim();
            string password = TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                TxtError.Text = "Inserisci un username e una password per registrarti.";
                return;
            }

            using (var db = new SwapiDbContext())
            {
                if (db.Users.Any(u => u.Username == username))
                {
                    TxtError.Text = "Username già esistente.";
                    return;
                }

                // Salva nuovo utente nel database
                db.Users.Add(new User { Username = username, Password = password });
                db.SaveChanges();

                TxtError.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Green);
                TxtError.Text = "Registrazione completata! Clicca su 'Accedi'.";
            }
        }
    }
}