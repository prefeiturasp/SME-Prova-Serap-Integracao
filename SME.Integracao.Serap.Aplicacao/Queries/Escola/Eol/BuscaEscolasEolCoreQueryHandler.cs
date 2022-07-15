
using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Queries
{
  public  class BuscaEscolasEolCoreQueryHandler : IRequestHandler<BuscaEscolasEolCoreQuery, IEnumerable<EscolaDto>>
    {

        private readonly IRepositorioUnidadeEducacao repositorioUnidadeEducacao;
        public BuscaEscolasEolCoreQueryHandler(IRepositorioUnidadeEducacao repositorioUnidadeEducacao)
        {
            this.repositorioUnidadeEducacao = repositorioUnidadeEducacao ??
                                          throw new ArgumentNullException(nameof(repositorioUnidadeEducacao));
        }

        public async Task<IEnumerable<EscolaDto>> Handle(BuscaEscolasEolCoreQuery request,
            CancellationToken cancellationToken)
            => await repositorioUnidadeEducacao.BuscaEscolas();
    }
  
}
