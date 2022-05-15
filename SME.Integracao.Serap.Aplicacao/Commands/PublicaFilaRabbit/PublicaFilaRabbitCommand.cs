using MediatR;

namespace SME.Integracao.Serap.Aplicacao
{
    public class PublicaFilaRabbitCommand : IRequest<bool>
    {
        public string NomeFila { get; private set; }
        public string NomeRota { get; private set; }
        public object Mensagem { get; private set; }

        public PublicaFilaRabbitCommand(string nomeFila, object mensagem = null)
        {
            Mensagem = mensagem;
            NomeFila = nomeFila;
            NomeRota = nomeFila;
        }
    }
}