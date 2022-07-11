using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarTurmasPorEscolaCommandHandler : IRequestHandler<TratarTurmasPorEscolaCommand, bool>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public TratarTurmasPorEscolaCommandHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<bool> Handle(TratarTurmasPorEscolaCommand request, CancellationToken cancellationToken)
        {
            await repositorioTurma.TratarTurmasEscola(request.CodigoEscola, request.AnoBase);
            return true;
        }
    }
}
