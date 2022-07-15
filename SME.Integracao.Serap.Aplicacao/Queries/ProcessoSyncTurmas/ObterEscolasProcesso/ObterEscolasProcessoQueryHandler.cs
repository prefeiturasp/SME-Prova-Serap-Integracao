using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterEscolasProcessoQueryHandler : IRequestHandler<ObterEscolasProcessoQuery, IEnumerable<EscolaSyncTurmas>>
    {

        private readonly IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas;

        public ObterEscolasProcessoQueryHandler(IRepositorioProcessoSyncTurmas repositorioProcessoSyncTurmas)
        {
            this.repositorioProcessoSyncTurmas = repositorioProcessoSyncTurmas ?? throw new ArgumentNullException(nameof(repositorioProcessoSyncTurmas));
        }

        public async Task<IEnumerable<EscolaSyncTurmas>> Handle(ObterEscolasProcessoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProcessoSyncTurmas.ObterEscolasProcesso(request.ProcessoId, request.QtdeEscolas);
        }
    }
}
