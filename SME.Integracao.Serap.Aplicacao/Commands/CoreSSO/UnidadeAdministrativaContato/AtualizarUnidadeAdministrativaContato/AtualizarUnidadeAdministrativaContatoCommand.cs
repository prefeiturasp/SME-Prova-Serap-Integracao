using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarUnidadeAdministrativaContatoCommand : IRequest<bool>
    {
        public AtualizarUnidadeAdministrativaContatoCommand(SysUnidadeAdministrativaContato unidadeAdministrativaContato)
        {
            UnidadeAdministrativaContato = unidadeAdministrativaContato;
        }

        public SysUnidadeAdministrativaContato UnidadeAdministrativaContato { get; set; }

    }
}
