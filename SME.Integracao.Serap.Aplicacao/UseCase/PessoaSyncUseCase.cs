using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class PessoaSyncUseCase : AbstractUseCase, IPessoaSyncUseCase
    {

        public PessoaSyncUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var processoId = Guid.NewGuid();
                await mediator.Send(new InserirProcessoCommand(processoId));

                await mediator.Send(new CarregarTempDadosPessoaCommand());
                await mediator.Send(new InserirAtualizarDadosPessoaCommand());

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [SYNC PESSOA] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                throw ex;
            }
        }
    }
}
