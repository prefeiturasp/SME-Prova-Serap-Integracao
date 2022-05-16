using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirUnidadeAdministrativaContatoCommand : IRequest<SysUnidadeAdministrativaContato>
    {
        public InserirUnidadeAdministrativaContatoCommand(SysUnidadeAdministrativaContato unidadeAdministrativaContato)
        {
            UnidadeAdministrativaContato = unidadeAdministrativaContato;
        }

        public SysUnidadeAdministrativaContato UnidadeAdministrativaContato { get; set; }
    }
}
