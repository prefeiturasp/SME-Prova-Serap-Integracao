using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
 public class AtualizarEscEscolaCommand : IRequest<bool>
    {
        public AtualizarEscEscolaCommand(EscEscola escola)
        {
            Escola = escola;
        }

        public EscEscola Escola { get; set; }
    }
}
