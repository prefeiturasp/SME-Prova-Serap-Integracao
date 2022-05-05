using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterParametrosCoreSsoQueryHandler : IRequestHandler<ObterParametrosCoreSsoQuery, ParametrosCoreSsoDto>
    {
        
        private readonly IRepositorioGeralCoreSso repositorioGeralCoreSso;

        public ObterParametrosCoreSsoQueryHandler(IRepositorioGeralCoreSso repositorioGeralCoreSso)
        {
            this.repositorioGeralCoreSso = repositorioGeralCoreSso ??
                                          throw new ArgumentNullException(nameof(repositorioGeralCoreSso));
        }

        public async Task<ParametrosCoreSsoDto> Handle(ObterParametrosCoreSsoQuery request,
            CancellationToken cancellationToken)
            => await repositorioGeralCoreSso.ObterParametrosCoreSso();
    }
}
