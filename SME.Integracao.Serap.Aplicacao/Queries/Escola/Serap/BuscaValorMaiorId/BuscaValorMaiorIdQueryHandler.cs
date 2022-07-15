using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Queries
{
    public class BuscaValorMaiorIdQueryHandler : IRequestHandler<BuscaValorMaiorIdQuery, int>
    {

        private readonly IRepositorioEscEscola repositorioEscEscola;

        public BuscaValorMaiorIdQueryHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ??
                                          throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<int> Handle(BuscaValorMaiorIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioEscEscola.BuscaMaiorIdEscolas();
    }
}