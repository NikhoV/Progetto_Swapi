using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Progetto2.DbModels;
using Progetto2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Progetto2.Views
{
    public sealed partial class OfflinePage : Page
    {
        private string _categoriaSelezionata = "Films";

        public OfflinePage()
        {
            this.InitializeComponent();
            CaricaDatiCategoria(_categoriaSelezionata);
        }

        private void BtnCategoriaOffline_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string categoria)
            {
                _categoriaSelezionata = categoria;
                TxtCategoriaCorrente.Text = $"Categoria: {categoria}";
                CaricaDatiCategoria(categoria);
            }
        }

        private void CaricaDatiCategoria(string categoria)
        {
            using (var db = new SwapiDbContext())
            {
                switch (categoria)
                {
                    case "Films":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplateFilms"];
                        LstDatiOffline.ItemsSource = db.Films.ToList();
                        break;
                    case "Personaggi":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplatePersonaggi"];
                        LstDatiOffline.ItemsSource = db.Personaggi.ToList();
                        break;
                    case "Pianeti":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplatePianeti"];
                        LstDatiOffline.ItemsSource = db.Pianeti.ToList();
                        break;
                    case "Navicelle":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplateNavicelle"];
                        LstDatiOffline.ItemsSource = db.Navicelle.ToList();
                        break;
                    case "Veicoli":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplateVeicoli"];
                        LstDatiOffline.ItemsSource = db.Veicoli.ToList();
                        break;
                    case "Specie":
                        LstDatiOffline.ItemTemplate = (DataTemplate)Resources["TemplateSpecie"];
                        LstDatiOffline.ItemsSource = db.Specie.ToList();
                        break;
                    default:
                        LstDatiOffline.ItemsSource = null;
                        break;
                }
            }
        }

        #region Esportazione Singola Categoria

        private async void EsportaCategoria_Json_Click(object sender, RoutedEventArgs e)
        {
            var dati = GetDatiCategoriaAttuale();
            string json = JsonSerializer.Serialize(dati, new JsonSerializerOptions { WriteIndented = true });
            await SalvaFileAsync($"SWAPI_{_categoriaSelezionata}.json", json, ".json", "File JSON (*.json)");
        }

        private async void EsportaCategoria_Xml_Click(object sender, RoutedEventArgs e)
        {
            var dati = GetDatiCategoriaAttuale();
            string xml = ConvertiInXml(dati);
            await SalvaFileAsync($"SWAPI_{_categoriaSelezionata}.xml", xml, ".xml", "File XML (*.xml)");
        }

        private async void EsportaCategoria_Txt_Click(object sender, RoutedEventArgs e)
        {
            var dati = GetDatiCategoriaAttuale();
            string txt = ConvertiInTxt(dati);
            await SalvaFileAsync($"SWAPI_{_categoriaSelezionata}.txt", txt, ".txt", "File di testo (*.txt)");
        }

        #endregion

        #region Esportazione Globale (Tutto il DB)

        private async void EsportaTutto_Json_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new SwapiDbContext())
            {
                var tutto = new { Films = db.Films.ToList(), Personaggi = db.Personaggi.ToList(), Pianeti = db.Pianeti.ToList(), Navicelle = db.Navicelle.ToList(), Veicoli = db.Veicoli.ToList(), Specie = db.Specie.ToList() };
                string json = JsonSerializer.Serialize(tutto, new JsonSerializerOptions { WriteIndented = true });
                await SalvaFileAsync("SWAPI_DatabaseCompleto.json", json, ".json", "File JSON (*.json)");
            }
        }

        private async void EsportaTutto_Xml_Click(object sender, RoutedEventArgs e)
        {
            SwapiExportData tutto;

            using (var db = new SwapiDbContext())
            {
                // Lettura asincrona per non bloccare la UI + AsNoTracking per prestazioni
                tutto = new SwapiExportData
                {
                    Films = await db.Films.AsNoTracking().ToListAsync(),
                    Personaggi = await db.Personaggi.AsNoTracking().ToListAsync(),
                    Pianeti = await db.Pianeti.AsNoTracking().ToListAsync(),
                    Navicelle = await db.Navicelle.AsNoTracking().ToListAsync(),
                    Veicoli = await db.Veicoli.AsNoTracking().ToListAsync(),
                    Specie = await db.Specie.AsNoTracking().ToListAsync()
                };
            }

            string xml = ConvertiInXml(tutto);
            await SalvaFileAsync("SWAPI_DatabaseCompleto.xml", xml, ".xml", "File XML (*.xml)");
        }

        private async void EsportaTutto_Txt_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new SwapiDbContext())
            {
                var films = db.Films.ToList();
                var personaggi = db.Personaggi.ToList();
                var pianeti = db.Pianeti.ToList();
                var navicelle = db.Navicelle.ToList();
                var veicoli = db.Veicoli.ToList();
                var specie = db.Specie.ToList();
                string txt = "=== ARCHIVIO COMPLETO HOLOCRON ===\n\n" + ConvertiInTxt(films) + ConvertiInTxt(personaggi) + ConvertiInTxt(pianeti) + ConvertiInTxt(navicelle) + ConvertiInTxt(veicoli) + ConvertiInTxt(specie);
                await SalvaFileAsync("SWAPI_DatabaseCompleto.txt", txt, ".txt", "File di testo (*.txt)");
            }
        }

        #endregion

        #region Utility e Salvataggio File

        private object GetDatiCategoriaAttuale()
        {
            using (var db = new SwapiDbContext())
            {
                return _categoriaSelezionata switch
                {
                    "Films" => db.Films.ToList(),
                    "Personaggi" => db.Personaggi.ToList(),
                    "Pianeti" => db.Pianeti.ToList(),
                    "Navicelle" => db.Navicelle.ToList(),
                    "Veicoli" => db.Veicoli.ToList(),
                    "Specie" => db.Specie.ToList(),
                    _ => new List<object>()
                };
            }
        }

        private string ConvertiInXml<T>(T oggetto)
        {
            if (oggetto == null) return string.Empty;

            var serializer = new XmlSerializer(oggetto.GetType());

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, oggetto);
                return writer.ToString();
            }
        }

        private string ConvertiInTxt(object dati)
        {
            var sb = new StringBuilder();

            switch (dati)
            {
                case IEnumerable<FilmCache> films:
                    sb.AppendLine("=== ARCHIVIO FILM OFFLINE ===");
                    foreach (var f in films)
                    {
                        sb.AppendLine($"ID: {f.Id} | Titolo: {f.Title} | Regista: {f.Director} | Episodio: {f.EpisodeId} | Data Uscita: {f.ReleaseDate} | Salvato il: {f.SavedAt}");
                    }
                    break;

                case IEnumerable<PersonaggioCache> personaggi:
                    sb.AppendLine("=== ARCHIVIO PERSONAGGI OFFLINE ===");
                    foreach (var p in personaggi)
                    {
                        sb.AppendLine($"ID: {p.Id} | Nome: {p.Name} | Genere: {p.Gender} | Anno Nascita: {p.BirthYear} | Altezza: {p.Height} cm | Salvato il: {p.SavedAt}");
                    }
                    break;

                case IEnumerable<PianetaCache> pianeti:
                    sb.AppendLine("=== ARCHIVIO PIANETI OFFLINE ===");
                    foreach (var p in pianeti)
                    {
                        sb.AppendLine($"ID: {p.Id} | Nome: {p.Name} | Clima: {p.Climate} | Terreno: {p.Terrain} | Popolazione: {p.Population} | Salvato il: {p.SavedAt}");
                    }
                    break;

                case IEnumerable<NavicellaCache> navicelle:
                    sb.AppendLine("=== ARCHIVIO NAVICELLE OFFLINE ===");
                    foreach (var n in navicelle)
                    {
                        sb.AppendLine($"ID: {n.Id} | Nome: {n.Name} | Modello: {n.Model} | Classe: {n.StarshipClass} | Costo: {n.CostInCredits} | Salvato il: {n.SavedAt}");
                    }
                    break;

                case IEnumerable<VeicoloCache> veicoli:
                    sb.AppendLine("=== ARCHIVIO VEICOLI OFFLINE ===");
                    foreach (var v in veicoli)
                    {
                        sb.AppendLine($"ID: {v.Id} | Nome: {v.Name} | Modello: {v.Model} | Classe: {v.VehicleClass} | Costo: {v.CostInCredits} | Salvato il: {v.SavedAt}");
                    }
                    break;

                case IEnumerable<SpecieCache> specie:
                    sb.AppendLine("=== ARCHIVIO SPECIE OFFLINE ===");
                    foreach (var s in specie)
                    {
                        sb.AppendLine($"ID: {s.Id} | Nome: {s.Name} | Classificazione: {s.Classification} | Lingua: {s.Language} | Salvato il: {s.SavedAt}");
                    }
                    break;

                default:
                    sb.AppendLine("Nessun dato disponibile per l'esportazione.");
                    break;
            }

            return sb.ToString();
        }

        private async System.Threading.Tasks.Task SalvaFileAsync(string nomePredefinito, string contenuto, string estensione, string descrizioneFiltro)
        {
            var savePicker = new FileSavePicker();

            // Necessario per WinUI 3 per agganciare il finestra di dialogo
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow.Instance);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add(descrizioneFiltro, new List<string>() { estensione });
            savePicker.SuggestedFileName = nomePredefinito;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                await FileIO.WriteTextAsync(file, contenuto);
            }
        }

        #endregion
    }
}