using MediatR;
using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;


namespace SME.Integracao.Serap.Aplicacao
{
  public  class BuscaUnidadesAdministrativasCoreSSOQuery : IRequest<IEnumerable<SysUnidadeAdministrativa>>
    {
        public BuscaUnidadesAdministrativasCoreSSOQuery()
        {

        }
    }
}
