using MediatR;
using SME.Integracao.Serap.Aplicacao.Commands;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.UseCase
{
    public class TestCommandUseCase : ITestCommandUseCase
    {
        private readonly IMediator mediator;

        public TestCommandUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var mensagem = $"WORKER INTEGRAÇÃO SUCESSO - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new TestCommand());
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, "TABELAS: SYS_TipoUnidadeAdministrativa, SYS_UnidadeAdministrativa atualizadas com sucesso"));
                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";

                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }
    }

}

