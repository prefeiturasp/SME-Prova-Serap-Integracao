using MediatR;
using SME.Integracao.Serap.Dominio;
using System;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterProcessoPorIdQuery : IRequest<ProcessoSyncTurmas>
    {
        public ObterProcessoPorIdQuery(Guid processoId)
        {
            ProcessoId = processoId;
        }

        public Guid ProcessoId { get; set; }
    }
}
