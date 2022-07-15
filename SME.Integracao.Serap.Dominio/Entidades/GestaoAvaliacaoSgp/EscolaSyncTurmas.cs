using System;

namespace SME.Integracao.Serap.Dominio
{
    public class EscolaSyncTurmas
    {
        public EscolaSyncTurmas()
        {

        }

        public EscolaSyncTurmas(Guid proId, string codigoEscola)
        {
            ProId = proId;
            CodigoEscola = codigoEscola;
            DataAlteracao = DataCriacao = DateTime.Now;
        }

        public Guid ProId { get; set; }
        public string CodigoEscola { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
