using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var createOrderTimer = new System.Timers.Timer();
            createOrderTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            createOrderTimer.Interval = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["tempoReprocessamento"]);
            createOrderTimer.Enabled = true;
            createOrderTimer.AutoReset = true;
            createOrderTimer.Start();
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            this.Motor();
        }

        /// <summary>
        /// A idéia do serviço é de preparar uma só aplicação para olhar para a tabela de conteúdos
        /// No início a montagem da fila era de responsabilidade de cada instância da TV
        /// Assim como não havia uma tabela de histórico e nem normalizada estava
        /// Quando era iniciada, e quando duas ou mais instâncias da TV estavam iniciadas, obviamente havia um conflito e sujeira no banco de dados por causa da concorrência não gerenciada
        /// O serviço faz:
        /// - Mantém um semáforo para indicar quando o serviço está em execução
        ///     - Asim se uma instância da TV estiver pronta para ler o conteúdo, ela ignora e repete a fila
        /// - Limpa as tabelas de integração
        /// - Limpa os documentos JSON populados para gerar a base da fila de conteúdos
        /// - Coleta o conteúdo a ser publicado de cada editoria (caso haja uma nova editoria, deve-se criar uma classe para segmentação do conteúdo)
        /// - Popula o conteúdo na tabela de integração
        /// - Popula o histórico de exibição de conteúdo segmentado por editoria
        /// - Gera um arquivo JSON para conteúdo geral
        /// - Gera um arquivo JSON para imagens dos sliders contidos no JSON de conteúdo geral
        /// - Mantém um log para cada fluxo de processamento onde o caminho está indicado no app.config
        /// - O tempo de repetição do processo está configurado no app config e é gerenciado pelo método OnStart desta mesma classe / arquivo 
        /// - É importante ressaltar que o serviço monta os arquivos JSON olhando para as tabelas de integração que são constantemente expurgadas, mas
        ///     - Mantemos tabelas de histórico
        ///     - a Tabela de integrção somente é vista pelo serviço, mas nenhuma aplicação / processo olha para ela
        /// </summary>
        public void Motor()
        {
            Log.Add(DateTime.Now.ToString() + ": PROCESSO INICIADO");
            //Atualizando o semáforo para definir que o serviço está rodando
            //Essa atividade faz com que caso uma tv venha a consultar o json, espere até que o serviço tenha atualizado o json
                Log.Add(DateTime.Now.ToString() + ": Atualizando o semáforo para ativo");
                cetv_DesenvolvimentoEntities db = new cetv_DesenvolvimentoEntities();
                try
                {
                    db.pr_atualizaSemaforo("ATIVO");
                }
                catch (Exception Ex)
                {
                    Log.Add(DateTime.Now.ToString() + ": Erro: "+Ex.Message);
                }
                Log.Add(DateTime.Now.ToString() + ": Semáforo atualizado");
            //Fim Atualização do semáforo
            Log.Add(DateTime.Now.ToString() + ": Limpando o json");
            Log.Clear(System.Configuration.ConfigurationSettings.AppSettings["json"]);
            Log.Clear(System.Configuration.ConfigurationSettings.AppSettings["jsonImagem"]);
            Log.Add(DateTime.Now.ToString() + ": Limpando a tabela de integração");
            Conteudo.esvaziaDadosIntegracao();
            //Montagem da Fila de Conteúdos
                //Conteúdo de Notícias do Grupo Canal Energia
                    Log.Add(DateTime.Now.ToString() + ": Coletando o conteúdo de Notícias Canal Energia Via WEB API");
                    NoticiaCanalEnergia canalEnergia = new NoticiaCanalEnergia();
                    canalEnergia.populaConteudoEmTabelaDeIntegracao();
                    Log.Add(DateTime.Now.ToString() + ": População concluída");
                //Fim Conteúdo de Notícias do Grupo Canal Energia
                //Conteúdo de Notícias do CMS Umbraco
                    Log.Add(DateTime.Now.ToString() + ": Coletando o conteúdo de Notícias do CMS");
                    NoticiaTVCorporativa tvCorporativaNoticia = new NoticiaTVCorporativa();
                    tvCorporativaNoticia.populaTabelaIntegracao();
                    Log.Add(DateTime.Now.ToString() + ": População concluída");
                //Conteúdo de Notícias do CMS Umbraco
                //Conteúdo de Vídeos do CMS Umbraco
                    Video tvCorporativaVideo = new Video();
                    Log.Add(DateTime.Now.ToString() + ": Coletando o conteúdo de Vídeos do CMS");
                    tvCorporativaVideo.populaFilaVideos();
                    Log.Add(DateTime.Now.ToString() + ": População concluída");
                //Fim Conteúdo de Vídeos do CMS Umbraco
                //Conteúdo de Imagens do CMS Umbraco
                    Log.Add(DateTime.Now.ToString() + ": Coletando o conteúdo de Imagens do CMS");
                    Slider tvCorporativaSlider = new Slider();
                    tvCorporativaSlider.populaSlidersNaFila();
                    Log.Add(DateTime.Now.ToString() + ": População concluída");
                //Fim Conteúdo de Imagens do CMS Umbraco
            //Fim Montagem da Fila de Conteúdos
            //Geração dos Arquivos JSON
                string conteudo = Conteudo.conteudoJson();
                Log.Add(conteudo, System.Configuration.ConfigurationSettings.AppSettings["json"]);
                Log.Add(DateTime.Now.ToString() + ": Conteúdo Geral armazenado");
                string conteudoImagem = Conteudo.conteudoImagemJson();
                Log.Add(conteudoImagem, System.Configuration.ConfigurationSettings.AppSettings["jsonImagem"]);
                Log.Add(DateTime.Now.ToString() + ": Conteúdo de imagens armazenado");
            //Fim Geração dos Arquivos JSON
            //Atualizando o semáforo para INATIVO
                Log.Add(DateTime.Now.ToString() + ": Atualizando o semáforo para inativo");
                db.pr_atualizaSemaforo("INATIVO");
                Log.Add(DateTime.Now.ToString() + ": Semáforo atualizado");
            //Fim Atualização do semáforo
            Log.Add("-------------------------------------------------------------");
        }

        protected override void OnStop()
        {
        }
    }
}
