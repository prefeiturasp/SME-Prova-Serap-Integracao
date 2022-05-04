using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirEnderecoCommandHandler : IRequestHandler<InserirEnderecoCommand, bool>
    {
        private readonly IRepositorioEndEndereco repositorioEndEndereco;

        public InserirEnderecoCommandHandler(IRepositorioEndEndereco repositorioEndEndereco)
        {
            this.repositorioEndEndereco = repositorioEndEndereco ?? throw new ArgumentNullException(nameof(repositorioEndEndereco));
        }

        public async Task<bool> Handle(InserirEnderecoCommand request, CancellationToken cancellationToken)
        {
            await repositorioEndEndereco.InserirEndereco(request.Endereco);
            return true;
        }
    }
}
