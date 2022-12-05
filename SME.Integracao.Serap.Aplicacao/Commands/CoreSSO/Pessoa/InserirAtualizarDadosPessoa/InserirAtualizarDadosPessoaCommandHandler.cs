using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirAtualizarDadosPessoaCommandHandler : IRequestHandler<InserirAtualizarDadosPessoaCommand, bool>
    {

        private readonly IRepositorioPessoa repositorioPessoa;

        public InserirAtualizarDadosPessoaCommandHandler(IRepositorioPessoa repositorioPessoa)
        {
            this.repositorioPessoa = repositorioPessoa ?? throw new ArgumentNullException(nameof(repositorioPessoa));
        }
        public async Task<bool> Handle(InserirAtualizarDadosPessoaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioPessoa.InserirAtualizarDadosPessoa(request.Paginacao.NumeroPagina, request.Paginacao.NumeroRegistros);
        }
    }
}
