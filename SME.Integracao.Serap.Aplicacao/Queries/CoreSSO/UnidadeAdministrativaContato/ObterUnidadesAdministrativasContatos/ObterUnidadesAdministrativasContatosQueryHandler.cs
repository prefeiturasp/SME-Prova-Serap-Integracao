using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterUnidadesAdministrativasContatosQueryHandler :
        IRequestHandler<ObterUnidadesAdministrativasContatosQuery, IEnumerable<SysUnidadeAdministrativaContato>>
    {

        private readonly IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato;

        public ObterUnidadesAdministrativasContatosQueryHandler(IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato)
        {
            this.repositorioUnidadeAdministrativaContato = repositorioUnidadeAdministrativaContato ??
                                          throw new ArgumentNullException(nameof(repositorioUnidadeAdministrativaContato));
        }

        public async Task<IEnumerable<SysUnidadeAdministrativaContato>> Handle(ObterUnidadesAdministrativasContatosQuery request,
                        CancellationToken cancellationToken)
            => await repositorioUnidadeAdministrativaContato.ObterUnidadesAdministrativasContatos();
    }
}
