using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ExcluirEscolasProcessoCommandHandler : IRequestHandler<ExcluirEscolasProcessoCommand, bool>
    {

        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public ExcluirEscolasProcessoCommandHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }

        public async Task<bool> Handle(ExcluirEscolasProcessoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProcessoSyncTurmas.ExcluirEscolasProcesso(request.ProcessoId, request.CodigosEscolas);
            return true;
        }
    }
}
