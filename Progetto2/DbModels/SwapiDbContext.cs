using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto2.DbModels
{
    public class SwapiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }


        public DbSet<FilmCache> Films { get; set; }
        public DbSet<PersonaggioCache> Personaggi { get; set; }
        public DbSet<PianetaCache> Pianeti { get; set; }
        public DbSet<NavicellaCache> Navicelle { get; set; }
        public DbSet<VeicoloCache> Veicoli { get; set; }
        public DbSet<SpecieCache> Specie { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Definisce il nome del file SQLite locale
            optionsBuilder.UseSqlite("Data Source=swapi.db");
        }
    }
}
