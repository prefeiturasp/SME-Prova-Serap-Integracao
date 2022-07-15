using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirTipoTurnoCommandHandler : IRequestHandler<InserirTipoTurnoCommand, bool>
    {

        private readonly IRepositorioTipoTurno repositorioTipoTurno;

        public InserirTipoTurnoCommandHandler(IRepositorioTipoTurno repositorioTipoTurno)
        {
            this.repositorioTipoTurno = repositorioTipoTurno ?? throw new ArgumentNullException(nameof(repositorioTipoTurno));
        }

        public async Task<bool> Handle(InserirTipoTurnoCommand request, CancellationToken cancellationToken)
        {
            await repositorioTipoTurno.InserirTipoTurno(request.TipoTurno);
            return true;
        }
    }
}
