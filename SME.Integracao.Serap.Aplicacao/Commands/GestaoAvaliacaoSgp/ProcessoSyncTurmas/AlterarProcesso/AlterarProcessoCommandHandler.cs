using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AlterarProcessoCommandHandler : IRequestHandler<AlterarProcessoCommand, bool>
    {

        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public AlterarProcessoCommandHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }

        public async Task<bool> Handle(AlterarProcessoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProcessoSyncTurmas.AlterarProcesso(request.Processo);
            return true;
        }
    }
}
