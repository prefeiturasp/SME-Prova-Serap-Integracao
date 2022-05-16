using Dapper.FluentMap.Dommel.Mapping;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class EndEnderecoMap : DommelEntityMap<EndEndereco>
    {
        public EndEnderecoMap()
        {
            ToTable("END_Endereco");
            Map(c => c.Id).ToColumn("end_id");
            Map(c => c.Cep).ToColumn("end_cep");
            Map(c => c.Logradouro).ToColumn("end_logradouro");
            Map(c => c.Bairro).ToColumn("end_bairro");
            Map(c => c.Distrito).ToColumn("end_distrito");
            Map(c => c.Zona).ToColumn("end_zona");
            Map(c => c.CidId).ToColumn("cid_id");
            Map(c => c.Situacao).ToColumn("end_situacao");
            Map(c => c.DataCriacao).ToColumn("end_dataCriacao");
            Map(c => c.DataAlteracao).ToColumn("end_dataAlteracao");
            Map(c => c.Integridade).ToColumn("end_integridade");
        }
    }
}
