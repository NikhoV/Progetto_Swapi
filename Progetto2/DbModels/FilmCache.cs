using System;

namespace Progetto2.DbModels
{
    public class FilmCache
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string Producer { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; } = DateTime.Now;
    }
}