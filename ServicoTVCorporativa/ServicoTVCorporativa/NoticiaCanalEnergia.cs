using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Services;
using System.Web.Http.SelfHost;
using Newtonsoft.Json;

namespace ServicoTVCorporativa
{
    class NoticiaCanalEnergia
    {
        public struct atributos
        {
            public string chamada { get; set; }
            public string classif1 { get; set; }
            public DateTime data { get; set; }
            public int id { get; set; }
            public string path { get; set; }
            public string titulo { get; set; }
            public string foto { get; set; }
        }

        /// <summary>
        /// Coleta as notícias publicadas das editorias do canal energia viw WebAPI
        /// </summary>
        /// <returns></returns>
        public List<atributos> webApiColetaNoticias()
        {
            List<atributos> listaNoticias = new List<atributos>();
            try
            {
                string url = @"https://novo.canalenergia.com.br/news?token=jYI6729jV29m3ZGTB2L4rVUxAo4qLZ13&limit=16";
                var json = new WebClient().DownloadString(url);
                listaNoticias = JsonConvert.DeserializeObject<List<atributos>>(json);
            }
            catch (Exception Ex)
            {
                Log.Add(DateTime.Now.ToString() + ": Erro: " + Ex.Message);
            }
            return listaNoticias;
        }

        /// <summary>
        /// Insere uma notícia publicada no banco de dados, em tabela de integração
        /// </summary>
        public void populaNoticiaEmTabelaDeIntegracao(int tvQueirozGalvaoConteudoTipoId, int ordem, DateTime dataIncial, DateTime dataFinal, string canal, string titulo, string conteudo, string urlImagem, string urlVideo, int tempoExibicao, bool flExibido)
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_insereItemFila(
                tvQueirozGalvaoConteudoTipoId,
                ordem,
                dataIncial,
                dataFinal,
                canal,
                titulo,
                conteudo,
                urlImagem,
                urlVideo,
                tempoExibicao,
                flExibido
            );
        }

        /// <summary>
        /// Popula todo o conteúdo de notícias na tabela de integração
        /// </summary>
        public void populaConteudoEmTabelaDeIntegracao()
        {
            List<atributos> lista = new List<atributos>();
            Log.Add(DateTime.Now.ToString() + ": Coletando lista de nótícias da WEB API");
            try
            {
                lista = this.webApiColetaNoticias();
                Log.Add(DateTime.Now.ToString() + ": Notícias coletadas com sucesso.");
                Log.Add(DateTime.Now.ToString() + ": Iniciando população da tabela de integração");
                Log.Add(DateTime.Now.ToString() + ": " + lista.Count.ToString() + "registros encontrados/n/n");
                for (int i = 0; i < lista.Count; i++)
                //for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        this.populaNoticiaEmTabelaDeIntegracao(
                                1,
                                0,
                                lista[i].data,
                                lista[i].data.AddDays(3),
                                lista[i].classif1.ToUpper(),
                                lista[i].titulo.ToUpper(),
                                lista[i].chamada,
                                lista[i].foto,
                                null,
                                0,
                                false
                            );
                        this.insereHistoricoExibicaoTvCanalEnergia(lista[i].id, lista[i].classif1, lista[i].titulo, lista[i].chamada, lista[i].foto);
                    }
                    catch (Exception Ex)
                    {
                        Log.Add(DateTime.Now.ToString() + ": Erro ao inserir conteúdo. >> " + Ex.Message);
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Add(DateTime.Now.ToString() + ": Erro ao inserir conteúdo. >> " + Ex.Message);
            }
        }

        /// <summary>
        /// insere o hitórico de exibição dos conteúdos veiculados pela tvCorporativa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="canal"></param>
        /// <param name="titulo"></param>
        /// <param name="conteudo"></param>
        /// <param name="urlImagem"></param>
        private void insereHistoricoExibicaoTvCanalEnergia(int id, string canal, string titulo, string conteudo, string urlImagem)
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_insereHistoricoNoticiaCanalEnergia(id, canal, titulo, conteudo, urlImagem);
        }

    }
}
