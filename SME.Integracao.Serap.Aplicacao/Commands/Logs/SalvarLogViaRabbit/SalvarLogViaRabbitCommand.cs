using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SME.Integracao.Serap.Aplicacao.Commands.Logs.SalvarLogViaRabbit
{
    public class SalvarLogViaRabbitCommand : IRequest<bool>
    {
        public SalvarLogViaRabbitCommand(string mensagem, string observacao = "", string projeto = "Integração Serap", string rastreamento = "", string excecaoInterna = "")
        {
            Mensagem = mensagem;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            ExcecaoInterna = excecaoInterna;
        }

        public string Mensagem { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public string ExcecaoInterna { get; }
    }
}
