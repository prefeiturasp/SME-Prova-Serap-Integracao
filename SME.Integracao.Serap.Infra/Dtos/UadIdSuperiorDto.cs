using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Infra
{ 
   public  class UadIdSuperiorDto
    {
        public Guid UadId { get; set; }
        public Guid  UadIdSuperior { get; set; }
        public string UadNome { get; set; }
        public Guid TuaId { get; set; }

    }
}
