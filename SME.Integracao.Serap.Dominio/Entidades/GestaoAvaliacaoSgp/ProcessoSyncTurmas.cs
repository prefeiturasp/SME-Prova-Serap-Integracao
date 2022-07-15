using System;

namespace SME.Integracao.Serap.Dominio
{
    public class ProcessoSyncTurmas
    {
        public ProcessoSyncTurmas()
        {

        }

        public ProcessoSyncTurmas(Guid id)
        {
            Id = id;
            Situacao = 0;
            DataAlteracao = DataCriacao = DateTime.Now;
        }

        public Guid Id { get; set; }
        public int Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
