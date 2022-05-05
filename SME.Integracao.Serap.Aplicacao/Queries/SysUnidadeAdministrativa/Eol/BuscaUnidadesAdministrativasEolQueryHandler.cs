
using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class BuscaUnidadesAdministrativasEolQueryHandler :
        IRequestHandler<BuscaUnidadesAdministrativasEolQuery, IEnumerable<UnidadeEducacaoDadosGeraisDto>>
    {
        private readonly IRepositorioUnidadeEducacao repositorioUnidadeEducacao;

        public BuscaUnidadesAdministrativasEolQueryHandler(IRepositorioUnidadeEducacao repositorioUnidadeEducacao)
        {
            this.repositorioUnidadeEducacao = repositorioUnidadeEducacao ??
                                          throw new ArgumentNullException(nameof(repositorioUnidadeEducacao));
        }

        public async Task<IEnumerable<UnidadeEducacaoDadosGeraisDto>> Handle(BuscaUnidadesAdministrativasEolQuery request,
            CancellationToken cancellationToken)
            => await repositorioUnidadeEducacao.BuscaUnidadeEducacaoDadosGerais();
    }
}
