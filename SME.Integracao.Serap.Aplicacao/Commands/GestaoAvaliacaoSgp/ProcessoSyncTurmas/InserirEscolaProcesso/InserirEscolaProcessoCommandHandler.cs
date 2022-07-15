using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirEscolaProcessoCommandHandler : IRequestHandler<InserirEscolaProcessoCommand, bool>
    {

        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public InserirEscolaProcessoCommandHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }
        public async Task<bool> Handle(InserirEscolaProcessoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProcessoSyncTurmas.InserirEscolaProcesso(request.Escola);
            return true;
        }
    }
}
