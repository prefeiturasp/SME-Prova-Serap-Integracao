using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterTotalPessoasTratarQueryHandler : IRequestHandler<ObterTotalPessoasTratarQuery, long>
    {

        private readonly IRepositorioPessoa repositorioPessoa;

        public ObterTotalPessoasTratarQueryHandler(IRepositorioPessoa repositorioPessoa)
        {
            this.repositorioPessoa = repositorioPessoa ?? throw new ArgumentNullException(nameof(repositorioPessoa));
        }
        public async Task<long> Handle(ObterTotalPessoasTratarQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPessoa.ObterTotalPessoasTratar();
        }
    }
}
