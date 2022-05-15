using MediatR;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterEnderecosCoreSsoQuery : IRequest<IEnumerable<EndEndereco>>
    {
        public ObterEnderecosCoreSsoQuery()
        {

        }
    }
}
