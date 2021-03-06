﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServicoTVCorporativa
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class cetv_DesenvolvimentoEntities : DbContext
    {
        public cetv_DesenvolvimentoEntities()
            : base("name=cetv_DesenvolvimentoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tvQueirozGalvaoConteudo> tvQueirozGalvaoConteudo { get; set; }
        public virtual DbSet<tvQueirozGalvaoConteudoSlider> tvQueirozGalvaoConteudoSlider { get; set; }
        public virtual DbSet<tvQueirozGalvaoConteudoTipo> tvQueirozGalvaoConteudoTipo { get; set; }
        public virtual DbSet<cmsContentXml> cmsContentXml { get; set; }
        public virtual DbSet<umbracoNode> umbracoNode { get; set; }
        public virtual DbSet<noticiaCanalEnergiaHistorico> noticiaCanalEnergiaHistorico { get; set; }
        public virtual DbSet<NoticiaTVCorporativaHistorico> NoticiaTVCorporativaHistorico { get; set; }
        public virtual DbSet<SlidersTVCorporativaHistorico> SlidersTVCorporativaHistorico { get; set; }
        public virtual DbSet<VideosTvCorporativaHistorico> VideosTvCorporativaHistorico { get; set; }
    
        public virtual int pr_deletaTabelaIntegracao()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_deletaTabelaIntegracao");
        }
    
        public virtual ObjectResult<pr_insereItemFila_Result> pr_insereItemFila(Nullable<int> tvQueirozGalvaoConteudoTipoId, Nullable<int> ordem, Nullable<System.DateTime> dataInicial, Nullable<System.DateTime> dataFinal, string canal, string titulo, string conteudo, string urlImagem, string urlVideo, Nullable<int> tempoExibicao, Nullable<bool> flExibido)
        {
            var tvQueirozGalvaoConteudoTipoIdParameter = tvQueirozGalvaoConteudoTipoId.HasValue ?
                new ObjectParameter("tvQueirozGalvaoConteudoTipoId", tvQueirozGalvaoConteudoTipoId) :
                new ObjectParameter("tvQueirozGalvaoConteudoTipoId", typeof(int));
    
            var ordemParameter = ordem.HasValue ?
                new ObjectParameter("ordem", ordem) :
                new ObjectParameter("ordem", typeof(int));
    
            var dataInicialParameter = dataInicial.HasValue ?
                new ObjectParameter("dataInicial", dataInicial) :
                new ObjectParameter("dataInicial", typeof(System.DateTime));
    
            var dataFinalParameter = dataFinal.HasValue ?
                new ObjectParameter("dataFinal", dataFinal) :
                new ObjectParameter("dataFinal", typeof(System.DateTime));
    
            var canalParameter = canal != null ?
                new ObjectParameter("canal", canal) :
                new ObjectParameter("canal", typeof(string));
    
            var tituloParameter = titulo != null ?
                new ObjectParameter("titulo", titulo) :
                new ObjectParameter("titulo", typeof(string));
    
            var conteudoParameter = conteudo != null ?
                new ObjectParameter("conteudo", conteudo) :
                new ObjectParameter("conteudo", typeof(string));
    
            var urlImagemParameter = urlImagem != null ?
                new ObjectParameter("urlImagem", urlImagem) :
                new ObjectParameter("urlImagem", typeof(string));
    
            var urlVideoParameter = urlVideo != null ?
                new ObjectParameter("urlVideo", urlVideo) :
                new ObjectParameter("urlVideo", typeof(string));
    
            var tempoExibicaoParameter = tempoExibicao.HasValue ?
                new ObjectParameter("tempoExibicao", tempoExibicao) :
                new ObjectParameter("tempoExibicao", typeof(int));
    
            var flExibidoParameter = flExibido.HasValue ?
                new ObjectParameter("flExibido", flExibido) :
                new ObjectParameter("flExibido", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<pr_insereItemFila_Result>("pr_insereItemFila", tvQueirozGalvaoConteudoTipoIdParameter, ordemParameter, dataInicialParameter, dataFinalParameter, canalParameter, tituloParameter, conteudoParameter, urlImagemParameter, urlVideoParameter, tempoExibicaoParameter, flExibidoParameter);
        }
    
        public virtual int pr_insereSliderFila(Nullable<int> queirozGalvaoConteudoId, Nullable<System.DateTime> dataInicial, Nullable<System.DateTime> dataFinal, string canal, string titulo, string urlImagem, Nullable<int> tempoExibicao)
        {
            var queirozGalvaoConteudoIdParameter = queirozGalvaoConteudoId.HasValue ?
                new ObjectParameter("queirozGalvaoConteudoId", queirozGalvaoConteudoId) :
                new ObjectParameter("queirozGalvaoConteudoId", typeof(int));
    
            var dataInicialParameter = dataInicial.HasValue ?
                new ObjectParameter("dataInicial", dataInicial) :
                new ObjectParameter("dataInicial", typeof(System.DateTime));
    
            var dataFinalParameter = dataFinal.HasValue ?
                new ObjectParameter("dataFinal", dataFinal) :
                new ObjectParameter("dataFinal", typeof(System.DateTime));
    
            var canalParameter = canal != null ?
                new ObjectParameter("canal", canal) :
                new ObjectParameter("canal", typeof(string));
    
            var tituloParameter = titulo != null ?
                new ObjectParameter("titulo", titulo) :
                new ObjectParameter("titulo", typeof(string));
    
            var urlImagemParameter = urlImagem != null ?
                new ObjectParameter("urlImagem", urlImagem) :
                new ObjectParameter("urlImagem", typeof(string));
    
            var tempoExibicaoParameter = tempoExibicao.HasValue ?
                new ObjectParameter("tempoExibicao", tempoExibicao) :
                new ObjectParameter("tempoExibicao", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_insereSliderFila", queirozGalvaoConteudoIdParameter, dataInicialParameter, dataFinalParameter, canalParameter, tituloParameter, urlImagemParameter, tempoExibicaoParameter);
        }
    
        public virtual int pr_atualizaSemaforo(string status)
        {
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_atualizaSemaforo", statusParameter);
        }
    
        public virtual int pr_insereHistoricoExibicaoNoticiaTvCorporativa(Nullable<int> nodeId)
        {
            var nodeIdParameter = nodeId.HasValue ?
                new ObjectParameter("nodeId", nodeId) :
                new ObjectParameter("nodeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_insereHistoricoExibicaoNoticiaTvCorporativa", nodeIdParameter);
        }
    
        public virtual int pr_insereHistoricoExibicaoSliders(Nullable<int> nodeId)
        {
            var nodeIdParameter = nodeId.HasValue ?
                new ObjectParameter("nodeId", nodeId) :
                new ObjectParameter("nodeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_insereHistoricoExibicaoSliders", nodeIdParameter);
        }
    
        public virtual int pr_insereHistoricoExibicaoVideos(Nullable<int> nodeId)
        {
            var nodeIdParameter = nodeId.HasValue ?
                new ObjectParameter("nodeId", nodeId) :
                new ObjectParameter("nodeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_insereHistoricoExibicaoVideos", nodeIdParameter);
        }
    
        public virtual int pr_insereHistoricoNoticiaCanalEnergia(Nullable<int> idNoticiaCanalEnergia, string canal, string titulo, string conteudo, string urlImagem)
        {
            var idNoticiaCanalEnergiaParameter = idNoticiaCanalEnergia.HasValue ?
                new ObjectParameter("idNoticiaCanalEnergia", idNoticiaCanalEnergia) :
                new ObjectParameter("idNoticiaCanalEnergia", typeof(int));
    
            var canalParameter = canal != null ?
                new ObjectParameter("canal", canal) :
                new ObjectParameter("canal", typeof(string));
    
            var tituloParameter = titulo != null ?
                new ObjectParameter("titulo", titulo) :
                new ObjectParameter("titulo", typeof(string));
    
            var conteudoParameter = conteudo != null ?
                new ObjectParameter("conteudo", conteudo) :
                new ObjectParameter("conteudo", typeof(string));
    
            var urlImagemParameter = urlImagem != null ?
                new ObjectParameter("urlImagem", urlImagem) :
                new ObjectParameter("urlImagem", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("pr_insereHistoricoNoticiaCanalEnergia", idNoticiaCanalEnergiaParameter, canalParameter, tituloParameter, conteudoParameter, urlImagemParameter);
        }
    }
}
