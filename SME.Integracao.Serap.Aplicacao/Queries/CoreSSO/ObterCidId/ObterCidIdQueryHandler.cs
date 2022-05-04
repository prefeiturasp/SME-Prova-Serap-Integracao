using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterCidIdQueryHandler : IRequestHandler<ObterCidIdQuery, Guid>
    {
        
        private readonly IRepositorioGeralCoreSso repositorioGeralCoreSso;

        public ObterCidIdQueryHandler(IRepositorioGeralCoreSso repositorioGeralCoreSso)
        {
            this.repositorioGeralCoreSso = repositorioGeralCoreSso ??
                                          throw new ArgumentNullException(nameof(repositorioGeralCoreSso));
        }

        public async Task<Guid> Handle(ObterCidIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioGeralCoreSso.ObterCidId();
    }
}
