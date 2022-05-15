using System;

namespace SME.Integracao.Serap.Infra
{
    public class DadosSetorDto
    {
        public DadosSetorDto()
        {

        }

        public string NomeDistrito { get; set; }
        public string NomeSetor { get; set; }
        public string CodigoSetor { get; set; }
        public Guid UadIdDistrito { get; set; }
        public Guid TuaIdSetor { get; set; }
        public Guid EntId { get; set; }
        public int Situacao { get; set; }
        public string CodigoEnderecoGrh { get; set; }

    }
}
