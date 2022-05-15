using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
    public class InserirEscEscolaCommandHandler : IRequestHandler<InserirEscEscolaCommand, object>
    {
        private readonly IRepositorioEscEscola repositorioEscEscola;

        public InserirEscEscolaCommandHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ?? throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<object> Handle(InserirEscEscolaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioEscEscola.InserirEscola(request.Escola);
        }
    }
}
