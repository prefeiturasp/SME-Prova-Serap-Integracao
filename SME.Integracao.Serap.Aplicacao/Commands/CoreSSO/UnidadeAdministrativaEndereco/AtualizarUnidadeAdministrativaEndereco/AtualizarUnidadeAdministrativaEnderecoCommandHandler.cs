using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarUnidadeAdministrativaEnderecoCommandHandler : IRequestHandler<AtualizarUnidadeAdministrativaEnderecoCommand, bool>
    {
        private readonly IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco;

        public AtualizarUnidadeAdministrativaEnderecoCommandHandler(IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco)
        {
            this.repositorioSysUnidadeAdministrativaEndereco = repositorioSysUnidadeAdministrativaEndereco ?? throw new ArgumentNullException(nameof(repositorioSysUnidadeAdministrativaEndereco));
        }

        public async Task<bool> Handle(AtualizarUnidadeAdministrativaEnderecoCommand request, CancellationToken cancellationToken)
        {
            await repositorioSysUnidadeAdministrativaEndereco.AtualizarUnidadeAdministrativaEndereco(request.UnidadeAdministrativaEndereco);
            return true;
        }
    }
}
