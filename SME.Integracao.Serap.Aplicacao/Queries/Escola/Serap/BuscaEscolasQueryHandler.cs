using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace SME.Integracao.Serap.Aplicacao.Queries
{
  public  class BuscaEscolasQueryHandler : IRequestHandler<BuscaEscolasQuery, IEnumerable<EscEscola>>
    {

        private readonly IRepositorioEscEscola repositorioEscEscola;

        public BuscaEscolasQueryHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ??
                                          throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<IEnumerable<EscEscola>> Handle(BuscaEscolasQuery request,
            CancellationToken cancellationToken)
            => await repositorioEscEscola.BuscaEscolas();
    }
}
