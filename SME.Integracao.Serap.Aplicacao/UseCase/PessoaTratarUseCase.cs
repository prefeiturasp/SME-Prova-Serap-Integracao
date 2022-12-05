using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class PessoaTratarUseCase : AbstractUseCase, IPessoaTratarUseCase
    {
        public PessoaTratarUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var paginacao = mensagemRabbit.ObterObjetoMensagem<PaginacaoDto>();
                if (paginacao == null) return false;
                await mediator.Send(new InserirAtualizarDadosPessoaCommand(paginacao));
                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [PESSOA TRATAR] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                throw ex;
            }
        }
    }
}
