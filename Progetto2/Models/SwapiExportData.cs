using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Progetto2.Models;
using Progetto2.DbModels;

namespace Progetto2.Models
{
    [XmlRoot("SwapiDatabase")]
    public class SwapiExportData
    {
        public List<FilmCache> Films { get; set; } = new();
        public List<PersonaggioCache> Personaggi { get; set; } = new();
        public List<PianetaCache> Pianeti { get; set; } = new();
        public List<NavicellaCache> Navicelle { get; set; } = new();
        public List<VeicoloCache> Veicoli { get; set; } = new();
        public List<SpecieCache> Specie { get; set; } = new();
    }
}
