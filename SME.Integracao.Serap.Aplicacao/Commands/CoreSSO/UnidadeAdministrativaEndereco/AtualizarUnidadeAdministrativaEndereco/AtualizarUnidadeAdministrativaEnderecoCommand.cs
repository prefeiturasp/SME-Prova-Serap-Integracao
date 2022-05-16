using MediatR;
using SME.Integracao.Serap.Dominio;


namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarUnidadeAdministrativaEnderecoCommand : IRequest<bool>
    {
        public AtualizarUnidadeAdministrativaEnderecoCommand(SysUnidadeAdministrativaEndereco unidadeAdministrativaEndereco)
        {
            UnidadeAdministrativaEndereco = unidadeAdministrativaEndereco;
        }

        public SysUnidadeAdministrativaEndereco UnidadeAdministrativaEndereco { get; set; }
    }
}
