using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class CarregarTempTurmasPorEscolaCommandHandler : IRequestHandler<CarregarTempTurmasPorEscolaCommand, bool>
    {
        
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IRepositorioTurmaEol repositorioTurmaEol;

        public CarregarTempTurmasPorEscolaCommandHandler(IRepositorioTurma repositorioTurma, IRepositorioTurmaEol repositorioTurmaEol)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioTurmaEol = repositorioTurmaEol ?? throw new ArgumentNullException(nameof(repositorioTurmaEol));
        }

        public async Task<bool> Handle(CarregarTempTurmasPorEscolaCommand request, CancellationToken cancellationToken)
        {            
            await repositorioTurma.CriarTempTurmasEol();
            await repositorioTurma.RemoverDadosTempTurmasEolPorEscola(request.CodigoEscola);
            await repositorioTurmaEol.CarregaTempTurmasEolIntegracao(request.CodigoEscola, request.AnoBase);
            await repositorioTurma.UpdatesTempTurmasEol(request.CodigoEscola);
            return true;
        }
    }
}
