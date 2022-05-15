using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterDadosSetoresParaInserirAlterarQueryHandler :
        IRequestHandler<ObterDadosSetoresParaInserirAlterarQuery, IEnumerable<DadosSetorDto>>
    {

        private readonly IRepositorioSetorEol repositorioSetorEol;

        public ObterDadosSetoresParaInserirAlterarQueryHandler(IRepositorioSetorEol repositorioSetorEol)
        {
            this.repositorioSetorEol = repositorioSetorEol ?? throw new ArgumentNullException(nameof(repositorioSetorEol));
        }

        public async Task<IEnumerable<DadosSetorDto>> Handle(ObterDadosSetoresParaInserirAlterarQuery request,
            CancellationToken cancellationToken)
            => await repositorioSetorEol.ObterDadosSetores();
    }
}
