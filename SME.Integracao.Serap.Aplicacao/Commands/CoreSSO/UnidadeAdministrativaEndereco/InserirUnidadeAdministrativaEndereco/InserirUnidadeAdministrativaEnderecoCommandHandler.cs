using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirUnidadeAdministrativaEnderecoCommandHandler : IRequestHandler<InserirUnidadeAdministrativaEnderecoCommand, SysUnidadeAdministrativaEndereco>
    {
        private readonly IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco;

        public InserirUnidadeAdministrativaEnderecoCommandHandler(IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco)
        {
            this.repositorioSysUnidadeAdministrativaEndereco = repositorioSysUnidadeAdministrativaEndereco ?? throw new ArgumentNullException(nameof(repositorioSysUnidadeAdministrativaEndereco));
        }

        public async Task<SysUnidadeAdministrativaEndereco> Handle(InserirUnidadeAdministrativaEnderecoCommand request, CancellationToken cancellationToken)
        {
            return (SysUnidadeAdministrativaEndereco)await repositorioSysUnidadeAdministrativaEndereco.InserirUnidadeAdministrativaEndereco(request.UnidadeAdministrativaEndereco);
        }
    }
}
