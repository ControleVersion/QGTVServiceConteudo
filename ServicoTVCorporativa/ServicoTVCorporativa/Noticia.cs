using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    class Noticia
    {
        public int id { get; set; }
        public int ordem { get; set; }
        public int tipo { get; set; }
        public DateTime dataInicial { get; set; }
        public DateTime dataFinal { get; set; }
        public string canal { get; set; }
        public string titulo { get; set; }
        public string conteudo { get; set; }
        public string urlImagem { get; set; }
        public string urlVideo { get; set; }
    }
}
