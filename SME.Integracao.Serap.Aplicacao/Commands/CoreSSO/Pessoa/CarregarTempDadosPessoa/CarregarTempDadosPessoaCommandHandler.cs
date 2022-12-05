using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class CarregarTempDadosPessoaCommandHandler : IRequestHandler<CarregarTempDadosPessoaCommand, bool>
    {

        private readonly IRepositorioPessoa repositorioPessoa;
        private readonly IRepositorioGeralEol repositorioGeralEol;

        public CarregarTempDadosPessoaCommandHandler(IRepositorioPessoa repositorioPessoa, IRepositorioGeralEol repositorioGeralEol)
        {
            this.repositorioPessoa = repositorioPessoa ?? throw new ArgumentNullException(nameof(repositorioPessoa));
            this.repositorioGeralEol = repositorioGeralEol ?? throw new ArgumentNullException(nameof(repositorioGeralEol));
        }
        public async Task<bool> Handle(CarregarTempDadosPessoaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPessoa.CriarOuLimparTempDadosPessoa();
            await repositorioGeralEol.CarregarTempDadosPessoaCoreSSO();
            return true;
        }
    }
}
