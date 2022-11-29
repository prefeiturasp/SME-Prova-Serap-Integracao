using MediatR;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirAtualizarPessoaDocumentoCommand : IRequest<bool>
    {
        public InserirAtualizarPessoaDocumentoCommand(PaginacaoDto paginacao)
        {
            Paginacao = paginacao;
        }
        public PaginacaoDto Paginacao { get; set; }
    }
}
