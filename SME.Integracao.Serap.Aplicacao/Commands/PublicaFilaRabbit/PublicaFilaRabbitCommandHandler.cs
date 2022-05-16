using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Aplicacao
{
    public class PublicaFilaRabbitCommandHandler : IRequestHandler<PublicaFilaRabbitCommand, bool>
    {
        private readonly IModel model;
        private readonly IMediator mediator;

        public PublicaFilaRabbitCommandHandler(IModel model, IMediator mediator)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task<bool> Handle(PublicaFilaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var mensagemStr = $"WORKER INTEGRAÇÃO SUCESSO - {mensagem.CodigoCorrelacao.ToString().Substring(0, 3)}";

                var mensagemJson = JsonSerializer.Serialize(mensagem);
                var body = Encoding.UTF8.GetBytes(mensagemJson);

                var props = model.CreateBasicProperties();
                props.Persistent = true;

                model.BasicPublish(ExchangeRabbit.IntegracaoSerap, request.NomeRota, props, body);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var msg = $"ERRO WORKER INTEGRACAO [PUBLICAR MSG FILA] " +
                            $"- Rota -> {request.NomeRota} Fila -> {request.NomeFila} " +
                            $"- Correlação -> {mensagem.CodigoCorrelacao.ToString().Substring(0, 3)} " +
                            $"- Mensagem -> {mensagem.Mensagem}";
                Task.FromResult(mediator.Send(new SalvarLogViaRabbitCommand(msg, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message)));
                return Task.FromResult(false);
            }
        }
    }
}