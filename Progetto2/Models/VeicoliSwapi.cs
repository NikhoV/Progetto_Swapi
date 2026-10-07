using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Progetto2.Models
{
    public class VeicoloSwapi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonPropertyName("cost_in_credits")]
        public string CostInCredits { get; set; }

        [JsonPropertyName("length")]
        public string Length { get; set; }

        [JsonPropertyName("max_atmosphering_speed")]
        public string MaxAtmospheringSpeed { get; set; }

        [JsonPropertyName("crew")]
        public string Crew { get; set; }

        [JsonPropertyName("passengers")]
        public string Passengers { get; set; }

        [JsonPropertyName("cargo_capacity")]
        public string CargoCapacity { get; set; }

        [JsonPropertyName("consumables")]
        public string Consumables { get; set; }

        [JsonPropertyName("vehicle_class")]
        public string VehicleClass { get; set; }

        [JsonPropertyName("pilots")]
        public List<string> Pilots { get; set; }

        [JsonPropertyName("films")]
        public List<string> Films { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        // Proprietà di supporto per il BINDING XAML
        public string InfoModello => $"Modello: {Model} | Classe: {VehicleClass} | Produttore: {Manufacturer}";
        public string InfoSpecifiche => $"Lunghezza: {Length}m | Vel. Max: {MaxAtmospheringSpeed} | Costo: {CostInCredits} crediti";
        public string PilotiCount => $"Piloti: {Pilots?.Count ?? 0}";
        public string FilmCount => $"Film: {Films?.Count ?? 0}";
    }
}
