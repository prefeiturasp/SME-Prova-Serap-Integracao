using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Infra.Dtos
{
    public class EscolaDto
    {
        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public Guid TuaId { get; set; }
        public string UadCodigo { get; set; }
        public string UadNome { get; set; }
        public int UadSituacao { get; set; }
        public int TreId { get; set; }
        public Guid UadIdSuperior { get; set; }

    }
}
