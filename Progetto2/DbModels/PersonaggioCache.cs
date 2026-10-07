using System;

namespace Progetto2.DbModels
{
    public class PersonaggioCache
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Mass { get; set; } = string.Empty;
        public string HairColor { get; set; } = string.Empty;
        public string SkinColor { get; set; } = string.Empty;
        public string EyeColor { get; set; } = string.Empty;
        public string BirthYear { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}