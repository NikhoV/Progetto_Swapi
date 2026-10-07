using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Progetto2.Models
{
    public class PianetaSwapi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rotation_period")]
        public string RotationPeriod { get; set; }

        [JsonPropertyName("orbital_period")]
        public string OrbitalPeriod { get; set; }

        [JsonPropertyName("diameter")]
        public string Diameter { get; set; }

        [JsonPropertyName("climate")]
        public string Climate { get; set; }

        [JsonPropertyName("gravity")]
        public string Gravity { get; set; }

        [JsonPropertyName("terrain")]
        public string Terrain { get; set; }

        [JsonPropertyName("surface_water")]
        public string SurfaceWater { get; set; }

        [JsonPropertyName("population")]
        public string Population { get; set; }

        [JsonPropertyName("residents")]
        public List<string> Residents { get; set; }

        [JsonPropertyName("films")]
        public List<string> Films { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        // Proprietà di supporto per il BINDING XAML
        public string InfoClimaTerreno => $"Clima: {Climate} | Terreno: {Terrain}";
        public string InfoAstro => $"Popolazione: {Population} | Diametro: {Diameter} km | Gravità: {Gravity}";
        public string ResidentiCount => $"Residenti noti: {Residents?.Count ?? 0}";
        public string FilmCount => $"Film: {Films?.Count ?? 0}";
    }
}
