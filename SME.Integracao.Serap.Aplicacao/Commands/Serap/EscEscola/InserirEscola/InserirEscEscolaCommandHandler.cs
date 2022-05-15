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
    public class InserirEscEscolaCommandHandler : IRequestHandler<InserirEscEscolaCommand, EscEscola>
    {
        private readonly IRepositorioEscEscola repositorioEscEscola;

        public InserirEscEscolaCommandHandler(IRepositorioEscEscola repositorioEscEscola)
        {
            this.repositorioEscEscola = repositorioEscEscola ?? throw new ArgumentNullException(nameof(repositorioEscEscola));
        }

        public async Task<EscEscola> Handle(InserirEnderecoCommand request, CancellationToken cancellationToken)
        {
            return (EndEndereco)await repositorioEscEscola.InserirEndereco(request.Endereco);


        }
    }
}
