using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class BuscaUadIdSuperiorQueryHandler : IRequestHandler<BuscaUadIdSuperiorQuery, UadIdSuperiorDto>
    {

        private readonly IRepositorioUnidadeEducacao repositorioUnidadeEducacao;
        public BuscaUadIdSuperiorQueryHandler(IRepositorioUnidadeEducacao repositorioUnidadeEducacao)
        {
            this.repositorioUnidadeEducacao = repositorioUnidadeEducacao ??
                                          throw new ArgumentNullException(nameof(repositorioUnidadeEducacao));
        }

        public async Task<UadIdSuperiorDto> Handle(BuscaUadIdSuperiorQuery request,
            CancellationToken cancellationToken)
            => await repositorioUnidadeEducacao.ObterUadIdSuperior(request.UadId);
    }
}