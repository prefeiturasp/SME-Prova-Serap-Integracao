using System;

namespace SME.Integracao.Serap.Dominio
{
    public class SysUnidadeAdministrativaContato
    {
        public SysUnidadeAdministrativaContato()
        {

        }

        public SysUnidadeAdministrativaContato(Guid entId, Guid uadId, Guid tmcId, string contato)
        {
            EntId = entId;
            UadId = uadId;
            UacId = Guid.NewGuid();
            TmcId = tmcId;
            Contato = contato;
            Situacao = 1;
            DataCriacao = DataAlteracao = DateTime.Now;
        }

        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public Guid UacId { get; set; }
        public Guid TmcId { get; set; }
        public string Contato { get; set; }
        public int Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }        
    }
}
