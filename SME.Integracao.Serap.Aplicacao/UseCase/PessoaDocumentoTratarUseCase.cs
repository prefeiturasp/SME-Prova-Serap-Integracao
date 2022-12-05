using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class PessoaDocumentoTratarUseCase : AbstractUseCase, IPessoaDocumentoTratarUseCase
    {
        public PessoaDocumentoTratarUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var paginacao = mensagemRabbit.ObterObjetoMensagem<PaginacaoDto>();
                if (paginacao == null) return false;
                await mediator.Send(new InserirAtualizarPessoaDocumentoCommand(paginacao));
                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [PESSOA DOCUMENTO TRATAR] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                throw ex;
            }
        }
    }
}
