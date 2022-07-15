using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirEscolaProcessoCommand : IRequest<bool>
    {
        public InserirEscolaProcessoCommand(EscolaSyncTurmas escola)
        {
            Escola = escola;
        }

        public EscolaSyncTurmas Escola { get; set; }
    }
}
