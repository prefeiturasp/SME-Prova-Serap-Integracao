using System;

namespace SME.Integracao.Serap.Dominio
{
    public class SysUnidadeAdministrativaEndereco
    {
        public SysUnidadeAdministrativaEndereco()
        {

        }

        public SysUnidadeAdministrativaEndereco(Guid entId, Guid uadId, Guid endId, string numero, string complemento)
        {
            EntId = entId;
            UadId = uadId;
            UaeId = Guid.NewGuid();
            EndId = endId;
            Numero = numero;
            Complemento = complemento;
            Situacao = 1;
            EnderecoPrincipal = null;
            Latitude = null;
            Longitude = null;
            DataCriacao = DataAlteracao = DateTime.Now;
        }

        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public Guid UaeId { get; set; }
        public Guid EndId { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public int Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool? EnderecoPrincipal { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }        

    }
}
