using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
  public  class AtualizarEscEscolaCommandHandler : IRequestHandler<AtualizarEscEscolaCommand, bool>
    {
        private readonly IRepositorioEscEscola repositorioEscEscola;

        public AtualizarEscEscolaCommandHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ?? throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<bool> Handle(AtualizarEscEscolaCommand request, CancellationToken cancellationToken)
        {
            await repositorioEscEscola.AtualizarEscola(request.Escola);
            return true;
        }
    }
}
