using MediatR;
using System;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirProcessoCommand : IRequest<bool>
    {
        public InserirProcessoCommand(Guid processoId)
        {
            ProcessoId = processoId;
        }

        public Guid ProcessoId { get; set; }
    }
}
