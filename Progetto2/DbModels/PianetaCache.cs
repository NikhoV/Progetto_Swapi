using System;

namespace Progetto2.DbModels
{
    public class PianetaCache
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RotationPeriod { get; set; } = string.Empty;
        public string OrbitalPeriod { get; set; } = string.Empty;
        public string Diameter { get; set; } = string.Empty;
        public string Climate { get; set; } = string.Empty;
        public string Gravity { get; set; } = string.Empty;
        public string Terrain { get; set; } = string.Empty;
        public string Population { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}