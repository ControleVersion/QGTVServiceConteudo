using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    class Imagem
    {
        public int id {get; set;}
        public int tvQueirozGalvaoConteudoId {get; set;}
        public DateTime dataInicial {get; set;}
        public DateTime dataFinal {get; set;}
        public string canal {get; set;}
        public string titulo {get; set; }
        public string urlImagem {get; set;}
    }
}
