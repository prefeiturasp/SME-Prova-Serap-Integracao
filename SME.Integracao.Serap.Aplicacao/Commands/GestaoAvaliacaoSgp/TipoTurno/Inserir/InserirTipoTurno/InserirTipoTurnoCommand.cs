using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirTipoTurnoCommand : IRequest<bool>
    {
        public InserirTipoTurnoCommand(TipoTurno tipoTurno)
        {
            TipoTurno = tipoTurno;
        }

        public TipoTurno TipoTurno { get; set; }
    }
}
