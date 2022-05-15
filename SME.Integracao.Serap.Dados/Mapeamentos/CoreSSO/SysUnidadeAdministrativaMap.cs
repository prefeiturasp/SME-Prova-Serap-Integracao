using Dapper.FluentMap.Dommel.Mapping;
using SME.Integracao.Serap.Dominio;


namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class SysUnidadeAdministrativaMap : DommelEntityMap<SysUnidadeAdministrativa>
    {
        public SysUnidadeAdministrativaMap()
        {
            ToTable("SYS_UnidadeAdministrativa");
            Map(c => c.EntidadeId).ToColumn("ent_id");
            Map(c => c.Id).ToColumn("uad_id").IsKey();
            Map(c => c.TuaId).ToColumn("tua_id");
            Map(c => c.Codigo).ToColumn("uad_codigo");
            Map(c => c.Nome).ToColumn("uad_nome");
            Map(c => c.Sigla).ToColumn("uad_sigla");
            Map(c => c.SuperiorId).ToColumn("uad_idSuperior");
            Map(c => c.Situacao).ToColumn("uad_situacao");
            Map(c => c.DataCriacao).ToColumn("uad_dataCriacao");
            Map(c => c.DataAlteracao).ToColumn("uad_dataAlteracao");
            Map(c => c.Integridade).ToColumn("uad_integridade");
            Map(c => c.CodigoIntegracao).ToColumn("uad_codigoIntegracao");
            Map(c => c.CodigoInep).ToColumn("uad_codigoInep");
        }
    }
}