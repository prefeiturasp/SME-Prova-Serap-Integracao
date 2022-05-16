using MediatR;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao
{ 
   public class BuscaUadIdSuperiorQuery : IRequest<UadIdSuperiorDto>

    {
        public BuscaUadIdSuperiorQuery(Guid uadId)
        {
            UadId = uadId;
        }

        public Guid UadId { get; set; }

    }
}
