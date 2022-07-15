using MediatR;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterCodigoEscolasAtivasQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigoEscolasAtivasQuery()
        {

        }
    }
}
