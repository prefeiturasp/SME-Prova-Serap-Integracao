using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
    public class InserirEscEscolaCommandHandler : IRequestHandler<InserirEscEscolaCommand, bool>
    {
        private readonly IRepositorioEscEscola repositorioEscEscola;

        public InserirEscEscolaCommandHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ?? throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<bool> Handle(InserirEscEscolaCommand request, CancellationToken cancellationToken)
        {
             await repositorioEscEscola.InserirEscola(request.Escola);
            return true;
        }
    }
}
