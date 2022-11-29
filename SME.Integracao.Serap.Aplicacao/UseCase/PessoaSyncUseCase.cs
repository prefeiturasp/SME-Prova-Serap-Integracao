using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Infra;
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

                await mediator.Send(new CarregarTempDadosPessoaCommand());

                var totalRegistros = await mediator.Send(new ObterTotalPessoasTratarQuery());
                var numeroRegistros = 1000;
                var totalPaginas = (int)Math.Ceiling((double)totalRegistros / numeroRegistros);
                for (int i = 1; i < totalPaginas + 1; i++)
                {
                    var msg = new PaginacaoDto(i, numeroRegistros);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PessoaTratar, msg));
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PessoaDocumentoTratar, msg));
                }
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
