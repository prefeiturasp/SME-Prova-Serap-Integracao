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
            await mediator.Send(new TestCommand());
            return true;
        }
    }

}

