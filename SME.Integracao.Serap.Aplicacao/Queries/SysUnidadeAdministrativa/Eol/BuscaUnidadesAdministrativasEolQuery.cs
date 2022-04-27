using MediatR;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
   public class BuscaUnidadesAdministrativasEolQuery : IRequest<IEnumerable<UnidadeEducacaoDadosGeraisDto>>
    {
        public BuscaUnidadesAdministrativasEolQuery()
        {

        }
    }
  
}
