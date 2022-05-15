using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirUnidadeAdministrativaEnderecoCommand : IRequest<SysUnidadeAdministrativaEndereco>
    {
        public InserirUnidadeAdministrativaEnderecoCommand(SysUnidadeAdministrativaEndereco unidadeAdministrativaEndereco)
        {
            UnidadeAdministrativaEndereco = unidadeAdministrativaEndereco;
        }

        public SysUnidadeAdministrativaEndereco UnidadeAdministrativaEndereco { get; set; }
    }
}
