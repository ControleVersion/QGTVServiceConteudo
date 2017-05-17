using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    class Conteudo
    {

        /// <summary>
        /// Esvazia as tabelas de integração
        /// </summary>
        public static void esvaziaDadosIntegracao()
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_deletaTabelaIntegracao();
        }

        /// <summary>
        /// Gera o conteúdo no formato JSON que já está populado no banco de dados
        /// </summary>
        /// <returns></returns>
        public static string conteudoJson()
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbConteudo in db.tvQueirozGalvaoConteudo
                         select tbConteudo).ToList();
            List<Noticia> lista = new List<Noticia>();
            for (int i = 0; i < query.Count; i++)
            {
                Noticia item = new Noticia();
                item.id = query[i].id;
                item.tipo = Convert.ToInt32(query[i].tvQueirozGalvaoConteudoTipoId);
                item.ordem = Convert.ToInt32(query[i].ordem);
                item.dataFinal = Convert.ToDateTime(query[i].dataFinal);
                item.dataInicial = Convert.ToDateTime(query[i].dataInicial);
                item.canal = query[i].canal;
                item.titulo = query[i].titulo;
                item.conteudo = query[i].conteudo;
                item.urlImagem = query[i].urlImagem;
                item.urlVideo = query[i].urlVideo;
                lista.Add(item);
            }
            string conteudoFormatado = JsonConvert.SerializeObject(lista);
            return conteudoFormatado;
        }

        /// <summary>
        /// Gera o conteúdo de imagens no formato json que está populado no banco de dados
        /// </summary>
        /// <returns></returns>
        public static string conteudoImagemJson()
        {
            List<Imagem> lista = new List<Imagem>();
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbImagens in db.tvQueirozGalvaoConteudoSlider
                         select tbImagens).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                Imagem item = new Imagem();
                item.id = query[i].id;
                item.tvQueirozGalvaoConteudoId = query[i].tvQueirozGalvaoConteudoId;
                item.dataInicial = Convert.ToDateTime(query[i].dataInicial);
                item.dataFinal = Convert.ToDateTime(query[i].dataFinal);
                item.canal = query[i].canal;
                item.titulo = query[i].titulo;
                item.urlImagem = query[i].urlImagem;
                lista.Add(item);
            }
            string json = JsonConvert.SerializeObject(lista);
            return json;
        }
    }
}
