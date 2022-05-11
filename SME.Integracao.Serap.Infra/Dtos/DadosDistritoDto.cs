using System;

namespace SME.Integracao.Serap.Infra
{
    public class DadosDistritoDto
    {
        public DadosDistritoDto()
        {

        }

        public Guid EntId { get; set; }
        public Guid TuaIdDistrito { get; set; }
        public string CodigoDistrito { get; set; }
        public string NomeDistrito { get; set; }
        public string CodigoDre { get; set; }
        public Guid UadIdDre { get; set; }
        public int Situacao { get; set; }
        public string CodigoEnderecoGrh { get; set; }
    }
}
