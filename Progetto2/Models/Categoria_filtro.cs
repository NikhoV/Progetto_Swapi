using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progetto2.Models
{
    public class CategoriaFiltroArgs
    {
        public string OrigineCorrelato { get; set; }
        public string MessaggioDescrizione { get; set; }
        public List<string> Urls { get; set; }
    }
}
