using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirProcessoCommandHandler : IRequestHandler<InserirProcessoCommand, bool>
    {

        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public InserirProcessoCommandHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }

        public async Task<bool> Handle(InserirProcessoCommand request, CancellationToken cancellationToken)
        {
            var processo = new ProcessoSyncTurmas(request.ProcessoId);
            await repositorioProcessoSyncTurmas.InserirProcesso(processo);
            return true;
        }
    }
}
