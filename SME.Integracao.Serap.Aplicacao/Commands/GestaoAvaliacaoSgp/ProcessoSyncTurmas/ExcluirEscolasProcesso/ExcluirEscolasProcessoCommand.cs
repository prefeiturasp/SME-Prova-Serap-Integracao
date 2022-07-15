using MediatR;
using System;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ExcluirEscolasProcessoCommand : IRequest<bool>
    {
        public ExcluirEscolasProcessoCommand(Guid processoId, string[] codigosEscolas)
        {
            ProcessoId = processoId;
            CodigosEscolas = codigosEscolas;
        }

        public Guid ProcessoId { get; set; }
        public string[] CodigosEscolas { get; set; }
    }
}
