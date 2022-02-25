using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SME.Integracao.Serap.Infra;
using System;

using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{ 
    public class SalvarLogViaRabbitCommandHandler : IRequestHandler<SalvarLogViaRabbitCommand, bool>
    {
        private readonly ConfiguracaoRabbitLogOptions configuracaoRabbitOptions;
        

        public SalvarLogViaRabbitCommandHandler(ConfiguracaoRabbitLogOptions configuracaoRabbitOptions)
        {
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
        }
        public Task<bool> Handle(SalvarLogViaRabbitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = JsonConvert.SerializeObject(new LogMensagem(request.Mensagem,
                                                                           request.Observacao,
                                                                           request.Projeto,
                                                                           request.Rastreamento,
                                                                           request.ExcecaoInterna), new JsonSerializerSettings
                                                                           {
                                                                               NullValueHandling = NullValueHandling.Ignore

                                                                           });

                var body = Encoding.UTF8.GetBytes(mensagem);

                PublicarMensagem(body);

                return Task.FromResult(true);
            }
            catch (System.Exception)
            {
                return Task.FromResult(false);
            }
        }
        private void PublicarMensagem(byte[] body)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
            };

            using (var conexaoRabbit = factory.CreateConnection())
            {
                using (IModel _channel = conexaoRabbit.CreateModel())
                {
                    var props = _channel.CreateBasicProperties();

                    _channel.BasicPublish(ExchangeRabbit.SerapLogs, RotasRabbitLogs.RotaLogs, props, body);
                }
            }
        }
    }
    public class LogMensagem
    {
        public LogMensagem(string mensagem, string observacao, string projeto, string rastreamento, string excecaoInterna)
        {
            Mensagem = mensagem;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            ExcecaoInterna = excecaoInterna;
            DataHora = DateTime.Now;
        }

        public string Mensagem { get; set; }
        public string Nivel { get; set; }
        public string Contexto { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public string ExcecaoInterna { get; set; }
        public DateTime DataHora { get; set; }

    }
}
