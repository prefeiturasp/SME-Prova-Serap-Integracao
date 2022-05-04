using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarEnderecoCommandHandler : IRequestHandler<AtualizarEnderecoCommand, bool>
    {
        private readonly IRepositorioEndEndereco repositorioEndEndereco;

        public AtualizarEnderecoCommandHandler(IRepositorioEndEndereco repositorioEndEndereco)
        {
            this.repositorioEndEndereco = repositorioEndEndereco ?? throw new ArgumentNullException(nameof(repositorioEndEndereco));
        }

        public async Task<bool> Handle(AtualizarEnderecoCommand request, CancellationToken cancellationToken)
        {
            await repositorioEndEndereco.AtualizarEndereco(request.Endereco);
            return true;
        }
    }
}
