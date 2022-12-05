using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirAtualizarPessoaDocumentoCommandHandler : IRequestHandler<InserirAtualizarPessoaDocumentoCommand, bool>
    {

        private readonly IRepositorioPessoaDocumento repositorioPessoaDocumento;

        public InserirAtualizarPessoaDocumentoCommandHandler(IRepositorioPessoaDocumento repositorioPessoaDocumento)
        {
            this.repositorioPessoaDocumento = repositorioPessoaDocumento ?? throw new ArgumentNullException(nameof(repositorioPessoaDocumento));
        }

        public async Task<bool> Handle(InserirAtualizarPessoaDocumentoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioPessoaDocumento.InserirAtualizarPessoaDocumento(request.Paginacao.NumeroPagina, request.Paginacao.NumeroRegistros);
        }
    }
}
