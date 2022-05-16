using MediatR;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterParametrosCoreSsoQuery : IRequest<ParametrosCoreSsoDto>
    {
        public ObterParametrosCoreSsoQuery()
        {

        }
    }
}
