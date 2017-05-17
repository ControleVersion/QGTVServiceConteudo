using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServicoTVCorporativa
{
    class Slider
    {
        public struct Propriedades
        {
            public int id { get; set; }
            public DateTime dataInicial { get; set; }
            public DateTime dataFinal { get; set; }
        }

        public struct dados
        {
            public string src { get; set; }
            public string titulo { get; set; }
        }

        public Propriedades lerXML(string xml)
        {
            Propriedades oPropriedades = new Propriedades();
            XmlDocument documento = new XmlDocument();
            documento.LoadXml(xml);
            XmlNode xmlDataInicial = documento.DocumentElement.SelectSingleNode("datainicial");
            oPropriedades.dataInicial = Convert.ToDateTime(xmlDataInicial.InnerText);
            XmlNode xmlDataFinal = documento.DocumentElement.SelectSingleNode("dataFinal");
            oPropriedades.dataFinal = Convert.ToDateTime(xmlDataFinal.InnerText);
            return oPropriedades;
        }

        public List<Propriedades> listaSliders()
        {
            int slider = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["nodeIdImagensTVCorporativa"]);
            List<Propriedades> lista = new List<Propriedades>();
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbContentXml in db.cmsContentXml
                         join tbUmbracoNode in db.umbracoNode
                         on tbContentXml.nodeId equals tbUmbracoNode.id
                         where tbUmbracoNode.parentID == slider
                         select new
                         {
                             nodeId = tbContentXml.nodeId,
                             xml = tbContentXml.xml
                         }).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                Propriedades item = new Propriedades();
                item = this.lerXML(query[i].xml);
                item.id = query[i].nodeId;
                if ((DateTime.Now >= item.dataInicial) && (DateTime.Now <= item.dataFinal))
                {
                    lista.Add(item);
                }
                this.insereHistoricoSlider(item.id);
            }
            return lista;
        }

        /// <summary>
        /// Insere o histórico de exibição dos sliders no banco de dados
        /// </summary>
        /// <param name="nodeId"></param>
        private void insereHistoricoSlider(int nodeId)
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            db.pr_insereHistoricoExibicaoSliders(nodeId);
        }

        public string coletaCaminhoDaImagemPorJSON(string json)
        {
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return result.src;
        }

        public dados coletaImagemDoSlider(string xml)
        {
            XmlDocument documento = new XmlDocument();
            documento.LoadXml(xml);
            XmlNode xmlImage = documento.DocumentElement.SelectSingleNode("image");
            string json = xmlImage.InnerText;
            string src = this.coletaCaminhoDaImagemPorJSON(json);
            dados dado = new dados();
            XmlNode xmlTitulo = documento.DocumentElement.SelectSingleNode("titulo");
            dado.titulo = xmlTitulo.InnerText;
            dado.src = src;
            return dado;
        }

        public List<dados> listaImagensPorSlider(int id)
        {
            List<dados> lista = new List<dados>();
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var query = (from tbCmsContentXml in db.cmsContentXml
                         join tbUmbracoNode in db.umbracoNode
                         on tbCmsContentXml.nodeId equals tbUmbracoNode.id
                         where tbUmbracoNode.parentID == id
                         select new
                         {
                             nodeId = tbCmsContentXml.nodeId,
                             xml = tbCmsContentXml.xml
                         }).OrderByDescending(tbContentXml => tbContentXml.nodeId).ToList();
            for (int i = 0; i < query.Count; i++)
            {
                string xml;
                xml = query[i].xml;
                lista.Add(this.coletaImagemDoSlider(xml));
            }
            return lista;
        }

        public void populaSlidersNaFila()
        {
            cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
            var transacao = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            List<Propriedades> sliders = this.listaSliders();
            try
            {
                int b = 0;
                for (int i = 0; i < sliders.Count; i++)
                {
                    List<dados> lista = new List<dados>();
                    lista = this.listaImagensPorSlider(sliders[i].id);
                    int itemFilaId = Convert.ToInt32(db.pr_insereItemFila(3, null, null, null, null, null, null, null, null, null, false).ToList()[0].id);
                    for (int j = 0; j < lista.Count; j++)
                    {
                        db.pr_insereSliderFila(
                            itemFilaId,
                            sliders[i].dataInicial,
                            sliders[i].dataFinal,
                            "QUEIROZ GALVÃO",
                            lista[j].titulo,
                            lista[j].src,
                            null
                            );
                    }
                }
                transacao.Commit();
            }
            catch (Exception Ex)
            {
                transacao.Rollback();
            }
        }


    }
}
