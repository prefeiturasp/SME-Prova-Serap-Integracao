using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirEnderecoCommandHandler : IRequestHandler<InserirEnderecoCommand, EndEndereco>
    {
        private readonly IRepositorioEndEndereco repositorioEndEndereco;

        public InserirEnderecoCommandHandler(IRepositorioEndEndereco repositorioEndEndereco)
        {
            this.repositorioEndEndereco = repositorioEndEndereco ?? throw new ArgumentNullException(nameof(repositorioEndEndereco));
        }

        public async Task<EndEndereco> Handle(InserirEnderecoCommand request, CancellationToken cancellationToken)
        {
            return (EndEndereco)await repositorioEndEndereco.InserirEndereco(request.Endereco);
        }
    }
}
