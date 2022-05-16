using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao.Queries
{
  public  class BuscaValorMaiorIdQuery : IRequest<int>
    {
        public BuscaValorMaiorIdQuery()
        {

        }
    }
}
