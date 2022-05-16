using Dapper.FluentMap.Dommel.Mapping;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class SysUnidadeAdministrativaContatoMap : DommelEntityMap<SysUnidadeAdministrativaContato>
    {
        public SysUnidadeAdministrativaContatoMap()
        {
            ToTable("SYS_UnidadeAdministrativaContato");
            Map(c => c.EntId).ToColumn("ent_id");
            Map(c => c.UadId).ToColumn("uad_id");
            Map(c => c.UacId).ToColumn("uac_id").IsKey();
            Map(c => c.TmcId).ToColumn("tmc_id");
            Map(c => c.Contato).ToColumn("uac_contato");
            Map(c => c.Situacao).ToColumn("uac_situacao");
            Map(c => c.DataCriacao).ToColumn("uac_dataCriacao");
            Map(c => c.DataAlteracao).ToColumn("uac_dataAlteracao");
        }
    }
}
