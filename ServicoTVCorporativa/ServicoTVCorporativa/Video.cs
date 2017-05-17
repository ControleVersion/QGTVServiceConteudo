using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServicoTVCorporativa
{
    class Video
    {
        public struct xmlVideoUmbraco
        {
            public int id { get; set; }
            public string xml { get; set; }
        }

        public struct video
        {
            public string titulo { get; set; }
            public DateTime dataInicial { get; set; }
            public DateTime dataFinal { get; set; }
            public int tempoDuracao { get; set; }
            public string url { get; set; }
        }

        /// <summary>
        /// Coleta o xml com o conteúdo dos vídeos ativos da TV Corporativa
        /// </summary>
        /// <returns></returns>
        public List<xmlVideoUmbraco> sqlColetaVideos()
        {
            int nodeIdVideo = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["nodeIdVideosTVCorporativa"]);
            List<xmlVideoUmbraco> lista = new List<xmlVideoUmbraco>();
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbContentXML in db.cmsContentXml
                         join tbUmbracoNode in db.umbracoNode
                         on tbContentXML.nodeId equals tbUmbracoNode.id
                         where tbUmbracoNode.parentID == nodeIdVideo
                         select new
                         {
                             nodeId = tbContentXML.nodeId,
                             xml = tbContentXML.xml,
                             dtPublicacao = tbUmbracoNode.createDate
                         }).OrderByDescending(tbUmbracoNode => tbUmbracoNode.dtPublicacao).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                xmlVideoUmbraco item = new xmlVideoUmbraco();
                item.id = query[i].nodeId;
                item.xml = query[i].xml;
                lista.Add(item);
                this.insereHistoricoVideos(item.id);
            }
            return lista;
        }

        /// <summary>
        /// Insere o histórico de exibição dos vídeos no banco de dados
        /// </summary>
        /// <param name="nodeId"></param>
        private void insereHistoricoVideos(int nodeId)
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_insereHistoricoExibicaoVideos(nodeId);
        }


        /// <summary>
        /// gera a struct com um item de vídeo
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public video coletaVideo(string xml)
        {
            XmlDocument documento = new XmlDocument();
            documento.LoadXml(xml);
            XmlNode xmlDataInicial = documento.DocumentElement.SelectSingleNode("/Video/dataInicial");
            XmlNode xmlDataFinal = documento.DocumentElement.SelectSingleNode("/Video/dataFinal");
            XmlNode xmlTempoDeDuracaoDoVideo = documento.DocumentElement.SelectSingleNode("/Video/tempoDeDuracaoDoVideo");
            XmlNode xmlUrlVideo = documento.DocumentElement.SelectSingleNode("/Video/urlvideo");
            XmlNode xmlTitulo = documento.DocumentElement.SelectSingleNode("/Video/titulo");
            video item = new video();
            item.dataInicial = Convert.ToDateTime(xmlDataInicial.InnerText);
            item.dataFinal = Convert.ToDateTime(xmlDataFinal.InnerText);
            item.tempoDuracao = Convert.ToInt32(xmlTempoDeDuracaoDoVideo.InnerText);
            item.url = xmlUrlVideo.InnerText;
            item.titulo = xmlTitulo.InnerText;
            return item;
        }

        /// <summary>
        /// Monta a lista de vídeos para que sejam imputados na tabela de integração
        /// </summary>
        /// <returns></returns>
        public List<video> montaListaVideos()
        {
            List<xmlVideoUmbraco> lista = new List<xmlVideoUmbraco>();
            List<video> listaDeVideos = new List<video>();
            lista = this.sqlColetaVideos();
            for (int i = 0; i < lista.Count; i++)
            {
                video item = new video();
                item = this.coletaVideo(lista[i].xml);
                if ((DateTime.Now >= item.dataInicial) && (DateTime.Now <= item.dataFinal))
                {
                    listaDeVideos.Add(item);
                }
            }
            return listaDeVideos;
        }

        /// <summary>
        /// Insere um item na tabela temporária
        /// </summary>
        /// <param name="tvQueirozGalvaoConteudoTipoId"></param>
        /// <param name="ordem"></param>
        /// <param name="dataIncial"></param>
        /// <param name="dataFinal"></param>
        /// <param name="canal"></param>
        /// <param name="titulo"></param>
        /// <param name="conteudo"></param>
        /// <param name="urlImagem"></param>
        /// <param name="urlVideo"></param>
        /// <param name="tempoExibicao"></param>
        /// <param name="flExibido"></param>
        public void insereConteudo(int tvQueirozGalvaoConteudoTipoId, int ordem, DateTime dataIncial, DateTime dataFinal, string canal, string titulo, string conteudo, string urlImagem, string urlVideo, int tempoExibicao, bool flExibido)
        {
            //Inserção do conteúdo na tabela de integração
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
            //Fim inserção do conteúdo na tabela de integração
            //Inserção do conteúdo na tabela de histórico
            //Fim inserção do conteúdo na tabela de histórico
        }

        /// <summary>
        /// Insere todos os vídeos na tabela temporária
        /// </summary>
        /// <param name="ordem"></param>
        public void populaFilaVideos()
        {
            List<video> videos = this.montaListaVideos();
            for (int i = 0; i < videos.Count; i++)
            {
                this.insereConteudo(
                        4,
                        0,
                        videos[i].dataInicial,
                        videos[i].dataFinal,
                        "TV CORPORATIVA",
                        videos[i].titulo.ToUpper(),
                        null,
                        null,
                        videos[i].url,
                        videos[i].tempoDuracao,
                        false
                    );
            }
        }




    }
}
