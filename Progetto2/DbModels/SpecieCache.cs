using System;

namespace Progetto2.DbModels
{
    public class SpecieCache
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string AverageHeight { get; set; } = string.Empty;
        public string SkinColors { get; set; } = string.Empty;
        public string HairColors { get; set; } = string.Empty;
        public string EyeColors { get; set; } = string.Empty;
        public string AverageLifespan { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}