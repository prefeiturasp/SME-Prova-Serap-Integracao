using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ExcluirTipoTurnoPorIdCommand : IRequest<bool>
    {
        public ExcluirTipoTurnoPorIdCommand(int tipoTurnoId)
        {
            TipoTurnoId = tipoTurnoId;
        }

        public int TipoTurnoId { get; set; }
    }
}
