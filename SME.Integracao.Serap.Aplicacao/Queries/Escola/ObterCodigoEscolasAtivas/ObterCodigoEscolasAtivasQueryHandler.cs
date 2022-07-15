using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterCodigoEscolasAtivasQueryHandler : IRequestHandler<ObterCodigoEscolasAtivasQuery, IEnumerable<string>>
    {

        private readonly IRepositorioEscola repositorioEscola;

        public ObterCodigoEscolasAtivasQueryHandler(IRepositorioEscola repositorioEscola)
        {
            this.repositorioEscola = repositorioEscola ?? throw new ArgumentNullException(nameof(repositorioEscola));
        }

        public async Task<IEnumerable<string>> Handle(ObterCodigoEscolasAtivasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEscola.ObterCodigoEscolasAtivas();
        }
    }
}
