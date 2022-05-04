using MediatR;
using System;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterCidIdQuery : IRequest<Guid>
    {
        public ObterCidIdQuery()
        {

        }
    }
}
