using MediatR;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterEscolasProcessoQuery : IRequest<IEnumerable<EscolaSyncTurmas>>
    {
        public ObterEscolasProcessoQuery(Guid processoId, int? qtdeEscolas)
        {
            ProcessoId = processoId;
            QtdeEscolas = qtdeEscolas;
        }

        public Guid ProcessoId { get; set; }
        public int? QtdeEscolas { get; set; }
    }
}
