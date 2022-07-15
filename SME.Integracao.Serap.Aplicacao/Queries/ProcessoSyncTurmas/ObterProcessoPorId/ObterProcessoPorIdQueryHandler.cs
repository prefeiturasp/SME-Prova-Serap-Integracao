using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterProcessoPorIdQueryHandler : IRequestHandler<ObterProcessoPorIdQuery, ProcessoSyncTurmas>
    {
        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public ObterProcessoPorIdQueryHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }

        public async Task<ProcessoSyncTurmas> Handle(ObterProcessoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProcessoSyncTurmas.ObterProcesso(request.ProcessoId);
        }
    }
}
