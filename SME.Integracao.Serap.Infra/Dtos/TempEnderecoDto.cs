using System;

namespace SME.Integracao.Serap.Infra
{
    public class TempEnderecoDto
    {
        public TempEnderecoDto()
        {

        }

        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public Guid EndId { get; set; }
        public string CdNrEndereco { get; set; }
        public string ComplementoEndereco { get; set; }
        public string Logradouro { get; set; }
        
    }
}
