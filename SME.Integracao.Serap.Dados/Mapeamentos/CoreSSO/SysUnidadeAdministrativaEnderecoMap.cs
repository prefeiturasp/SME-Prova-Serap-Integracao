using Dapper.FluentMap.Dommel.Mapping;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class SysUnidadeAdministrativaEnderecoMap : DommelEntityMap<SysUnidadeAdministrativaEndereco>
    {
        public SysUnidadeAdministrativaEnderecoMap()
        {
            ToTable("SYS_UnidadeAdministrativaEndereco");
            Map(c => c.EntId).ToColumn("ent_id");
            Map(c => c.UadId).ToColumn("uad_id");
            Map(c => c.UaeId).ToColumn("uae_id");
            Map(c => c.EndId).ToColumn("end_id");
            Map(c => c.Numero).ToColumn("uae_numero");
            Map(c => c.Complemento).ToColumn("uae_complemento");
            Map(c => c.Situacao).ToColumn("uae_situacao");
            Map(c => c.DataCriacao).ToColumn("uae_dataCriacao");
            Map(c => c.DataAlteracao).ToColumn("uae_dataAlteracao");
            Map(c => c.EnderecoPrincipal).ToColumn("uae_enderecoPrincipal");
            Map(c => c.Latitude).ToColumn("uae_latitude");
            Map(c => c.Longitude).ToColumn("uae_longitude");
        }        
    }
}
