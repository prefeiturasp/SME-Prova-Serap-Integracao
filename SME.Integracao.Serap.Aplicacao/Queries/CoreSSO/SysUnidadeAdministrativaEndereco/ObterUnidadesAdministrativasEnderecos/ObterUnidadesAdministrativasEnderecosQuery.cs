using MediatR;
using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterUnidadesAdministrativasEnderecosQuery : IRequest<IEnumerable<SysUnidadeAdministrativaEndereco>>
    {
        public ObterUnidadesAdministrativasEnderecosQuery()
        {

        }
    }
}
