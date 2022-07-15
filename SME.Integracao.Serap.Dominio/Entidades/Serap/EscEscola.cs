using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Dominio
{
    public class EscEscola
    {
        public long EscId { get; set; }
        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public string EscCodigo { get; set; }
        public string EscNome { get; set; }
        public int EscSituacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public Guid UadIdSuperiorGestao { get; set; }
        public Guid TuaId { get; set; }
    }
}
