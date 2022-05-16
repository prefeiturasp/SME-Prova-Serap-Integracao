using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterEnderecosCoreSsoQueryHandler : IRequestHandler<ObterEnderecosCoreSsoQuery, IEnumerable<EndEndereco>>
    {
        private readonly IRepositorioEndEndereco repositorioEndereco;

        public ObterEnderecosCoreSsoQueryHandler(IRepositorioEndEndereco repositorioEndereco)
        {
            this.repositorioEndereco = repositorioEndereco ?? throw new ArgumentNullException(nameof(repositorioEndereco));
        }

        public async Task<IEnumerable<EndEndereco>> Handle(ObterEnderecosCoreSsoQuery request, CancellationToken cancellationToken)
            => await repositorioEndereco.ObterEnderecosCoreSso();
    }
}
