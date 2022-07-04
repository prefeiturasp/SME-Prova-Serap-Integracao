using MediatR;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.UseCase
{
    public abstract class AbstractUseCase
    {
        protected readonly IMediator mediator;

        public AbstractUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task RegistrarLogErro(string msg, Exception ex)
        {
            try
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(msg, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
