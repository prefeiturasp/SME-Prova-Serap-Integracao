using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterUnidadesAdministrativasEnderecosQueryHandler :
        IRequestHandler<ObterUnidadesAdministrativasEnderecosQuery, IEnumerable<SysUnidadeAdministrativaEndereco>>
    {

        private readonly IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco;

        public ObterUnidadesAdministrativasEnderecosQueryHandler(IRepositorioSysUnidadeAdministrativaEndereco repositorioSysUnidadeAdministrativaEndereco)
        {
            this.repositorioSysUnidadeAdministrativaEndereco = repositorioSysUnidadeAdministrativaEndereco ??
                                          throw new ArgumentNullException(nameof(repositorioSysUnidadeAdministrativaEndereco));
        }

        public async Task<IEnumerable<SysUnidadeAdministrativaEndereco>> Handle(ObterUnidadesAdministrativasEnderecosQuery request,
                        CancellationToken cancellationToken)
            => await repositorioSysUnidadeAdministrativaEndereco.ObterSysUnidadesAdministrativasEnderecos();

    }
}
