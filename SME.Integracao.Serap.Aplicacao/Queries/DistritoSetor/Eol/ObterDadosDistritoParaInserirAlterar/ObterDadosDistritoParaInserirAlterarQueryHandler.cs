using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterDadosDistritoParaInserirAlterarQueryHandler :
        IRequestHandler<ObterDadosDistritoParaInserirAlterarQuery, IEnumerable<DadosDistritoDto>>
    {

        private readonly IRepositorioDistritoEol repositorioDistritoEol;

        public ObterDadosDistritoParaInserirAlterarQueryHandler(IRepositorioDistritoEol repositorioDistritoEol)
        {
            this.repositorioDistritoEol = repositorioDistritoEol ?? throw new ArgumentNullException(nameof(repositorioDistritoEol));
        }

        public async Task<IEnumerable<DadosDistritoDto>> Handle(ObterDadosDistritoParaInserirAlterarQuery request,
            CancellationToken cancellationToken)
            => await repositorioDistritoEol.ObterDadosDistritos();
    }
}
