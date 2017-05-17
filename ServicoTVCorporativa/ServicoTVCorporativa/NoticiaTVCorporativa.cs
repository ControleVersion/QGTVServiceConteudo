using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServicoTVCorporativa
{
    class NoticiaTVCorporativa
    {
        public struct xmlNoticiaUmbraco
        {
            public int id { get; set; }
            public string xml { get; set; }
        }

        public struct noticiaCMSTVCorporativa
        {
            public string titulo { get; set; }
            public string imagem { get; set; }
            public string conteudo { get; set; }
            public DateTime dataInicial { get; set; }
            public DateTime dataFinal { get; set; }
        }

        /// <summary>
        /// Carrega a lista de xml's de conteúdos direto do CMS
        /// </summary>
        /// <returns></returns>
        public List<xmlNoticiaUmbraco> sqlNoticiasQueirozGalvao()
        {
            int nodeId = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["nodeIdNoticiasTVCorporativa"]);
            List<xmlNoticiaUmbraco> lista = new List<xmlNoticiaUmbraco>();
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbContentXML in db.cmsContentXml
                         join tbUmbracoNode in db.umbracoNode
                         on tbContentXML.nodeId equals tbUmbracoNode.id
                         where tbUmbracoNode.parentID == nodeId
                         select new
                         {
                             nodeId = tbContentXML.nodeId,
                             xml = tbContentXML.xml,
                             dtPublicacao = tbUmbracoNode.createDate
                         }).OrderByDescending(tbUmbracoNode => tbUmbracoNode.dtPublicacao).ToList();
            if (query.Count > 0)
            {
                for (int i = 0; i < query.Count; i++)
                {
                    xmlNoticiaUmbraco item = new xmlNoticiaUmbraco();
                    item.id = query[i].nodeId;
                    item.xml = query[i].xml;
                    lista.Add(item);
                    this.insereHistoricoNoticiaTVCorporativa(item.id);
                }
            }
            return lista;
        }

        /// <summary>
        /// Insere o histórico de exibição das notícias da TV Corporativa no banco de dados
        /// </summary>
        /// <param name="nodeId"></param>
        private void insereHistoricoNoticiaTVCorporativa(int nodeId)
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_insereHistoricoExibicaoNoticiaTvCorporativa(nodeId);
        }

        /// <summary>
        /// Utilizado para pegar o caminho da imagem do CMS que fica contida dentro de uma string JSON
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string coletaCaminhoImagemJSON(string json)
        {
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return result.src;
        }

        /// <summary>
        /// Extrai o xml retornado do CMS e o coloca na estrutura noticiaCMSTVCorporativa para depois montar uma lista 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private noticiaCMSTVCorporativa coletaConteudoXML(string xml)
        {
            XmlDocument documento = new XmlDocument();
            documento.LoadXml(xml);
            XmlNode JsonImagem = documento.DocumentElement.SelectSingleNode("/NoticiaQueirozGalvao/imagemLateral");
            string urlImagem = this.coletaCaminhoImagemJSON(JsonImagem.InnerText);
            XmlNode xmlTitulo = documento.DocumentElement.SelectSingleNode("/NoticiaQueirozGalvao/titulo");
            string titulo = xmlTitulo.InnerText;
            XmlNode xmlConteudo = documento.DocumentElement.SelectSingleNode("/NoticiaQueirozGalvao/conteudo");
            string conteudo = xmlConteudo.InnerText;
            XmlNode xmlDtInicial = documento.DocumentElement.SelectSingleNode("/NoticiaQueirozGalvao/dataInicial");
            XmlNode xmlDtFinal = documento.DocumentElement.SelectSingleNode("/NoticiaQueirozGalvao/dataFinal");
            noticiaCMSTVCorporativa noticia = new noticiaCMSTVCorporativa();
            noticia.imagem = urlImagem;
            noticia.titulo = titulo;
            noticia.conteudo = conteudo;
            noticia.dataInicial = Convert.ToDateTime(xmlDtInicial.InnerText);
            noticia.dataFinal = Convert.ToDateTime(xmlDtFinal.InnerText);
            return noticia;
        }

        /// <summary>
        /// Coleta a lista de conteúdos da TV Corporativa
        /// </summary>
        /// <returns></returns>
        public List<noticiaCMSTVCorporativa> carregaListaDeNoticias()
        {
            List<noticiaCMSTVCorporativa> lista = new List<noticiaCMSTVCorporativa>();
            List<xmlNoticiaUmbraco> xmls = new List<xmlNoticiaUmbraco>();
            xmls = this.sqlNoticiasQueirozGalvao();
            for (int i = 0; i < xmls.Count; i++)
            {
                noticiaCMSTVCorporativa item = new noticiaCMSTVCorporativa();
                item = this.coletaConteudoXML(xmls[i].xml);
                lista.Add(item);
            }
            return lista;
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
        /// Insere os conteúdos publicados no CMS da TV Corporativa numa tabela temporaria
        /// </summary>
        public void populaTabelaIntegracao()
        {
            Log.Add(DateTime.Now.ToString() + ": Coletando lista de nótícias da TV CORPORATIVA");
            List<noticiaCMSTVCorporativa> lista = this.carregaListaDeNoticias();
            Log.Add(DateTime.Now.ToString() + ": Notícias coletadas com sucesso.");
            Log.Add(DateTime.Now.ToString() + ": Iniciando população da tabela de integração");
            Log.Add(DateTime.Now.ToString() + ": " + lista.Count.ToString() + "registros encontrados/n/n");
            for (int i = 0; i < lista.Count; i++)
            {
                try
                {
                    this.populaNoticiaEmTabelaDeIntegracao(
                        1,
                        0,
                        lista[i].dataInicial,
                        lista[i].dataFinal,
                        "TV CORPORATIVA",
                        lista[i].titulo.ToUpper(),
                        lista[i].conteudo,
                        lista[i].imagem,
                        null,
                        0,
                        false
                    );
                }
                catch (Exception Ex)
                {
                    Log.Add(DateTime.Now.ToString() + ": Erro ao inserir conteúdo. >> " + Ex.Message);
                }
            }
        }

    }
}
