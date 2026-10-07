using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Progetto2.Models
{
    public class PersonaggioSwapi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("height")]
        public string Height { get; set; }

        [JsonPropertyName("mass")]
        public string Mass { get; set; }

        [JsonPropertyName("hair_color")]
        public string HairColor { get; set; }

        [JsonPropertyName("skin_color")]
        public string SkinColor { get; set; }

        [JsonPropertyName("eye_color")]
        public string EyeColor { get; set; }

        [JsonPropertyName("birth_year")]
        public string BirthYear { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("homeworld")]
        public string Homeworld { get; set; }

        [JsonPropertyName("films")]
        public List<string> Films { get; set; }

        [JsonPropertyName("species")]
        public List<string> Species { get; set; }

        [JsonPropertyName("vehicles")]
        public List<string> Vehicles { get; set; }

        [JsonPropertyName("starships")]
        public List<string> Starships { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        // Proprietà di supporto per il BINDING XAML
        public string InfoAnagrafiche => $"Nascita: {BirthYear} | Genere: {Gender}";
        public string InfoFisiche => $"Altezza: {Height} cm | Peso: {Mass} kg | Occhi: {EyeColor} | Capelli: {HairColor}";
        public string FilmCount => $"Film: {Films?.Count ?? 0}";
        public string VeicoliCount => $"Veicoli: {Vehicles?.Count ?? 0}";
        public string NavicelleCount => $"Navi: {Starships?.Count ?? 0}";
        public string SpecieCount => $"Specie: {Species?.Count ?? 0}";
    }
}
