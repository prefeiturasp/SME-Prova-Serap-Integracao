using MediatR;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirAtualizarDadosPessoaCommand : IRequest<bool>
    {
        public InserirAtualizarDadosPessoaCommand(PaginacaoDto paginacao)
        {
            Paginacao = paginacao;
        }

        public PaginacaoDto Paginacao { get; set; }
    }
}
