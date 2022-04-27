using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Infra
{
    public class UnidadeEducacaoDadosGeraisDto
    {
        public string CodigoUnidadeEducacao { get; set; }
        public string DescricaoTipoUnidadeEducacao { get; set; }
        public string SiglaTipoEscola { get; set; }
        public string NomeUnidadeEducacao { get; set; }
        public int TipoLogradouro { get; set; }
        public string NomeLogradouro { get; set; }
        public string CodigoNrEndereco { get; set; }
        public string DescricaoComplementoEndereco { get; set; }
        public string NomeBairro { get; set; }
        public int CodigoCep { get; set; }
        public string NomeDistritoMec { get; set; }
        public string NomeMicroRegiao { get; set; }
        public int CodigoSetorDistrito { get; set; }
        public string DescricaoSubPrefeitura { get; set; }
        public string SiglaTipoSituacaoUnidade { get; set; }
        public int SituacaoUnidadeEducacao { get; set; }
        public Guid TuaIdEscola { get; set; }
        public Guid EntId { get; set; }
        public string CodigodUnidadeAdministrativaRef { get; set; }

        public Guid UadIdSuperior { get; set; }
    }
}
