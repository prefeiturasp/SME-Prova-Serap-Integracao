using MediatR;
using SME.Integracao.Serap.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao.Queries
{
   public class BuscaEscolasEolCoreQuery : IRequest<IEnumerable<EscolasDto>>
    {
        public BuscaEscolasEolCoreQuery()
        {

        }
    }
   
}
