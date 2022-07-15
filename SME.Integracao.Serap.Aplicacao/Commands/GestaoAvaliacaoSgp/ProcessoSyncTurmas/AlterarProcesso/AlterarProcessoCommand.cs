using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AlterarProcessoCommand : IRequest<bool>
    {
        public AlterarProcessoCommand(ProcessoSyncTurmas processo)
        {
            Processo = processo;
        }

        public ProcessoSyncTurmas Processo { get; set; }
    }
}
