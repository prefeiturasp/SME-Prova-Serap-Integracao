using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ExcluirTipoTurnoPorIdCommandHandler : IRequestHandler<ExcluirTipoTurnoPorIdCommand, bool>
    {

        private readonly IRepositorioTipoTurno repositorioTipoTurno;

        public ExcluirTipoTurnoPorIdCommandHandler(IRepositorioTipoTurno repositorioTipoTurno)
        {
            this.repositorioTipoTurno = repositorioTipoTurno ?? throw new ArgumentNullException(nameof(repositorioTipoTurno));
        }

        public async Task<bool> Handle(ExcluirTipoTurnoPorIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioTipoTurno.ExcluirTipoTurnoPorId(request.TipoTurnoId);
            return true;
        }
    }
}
