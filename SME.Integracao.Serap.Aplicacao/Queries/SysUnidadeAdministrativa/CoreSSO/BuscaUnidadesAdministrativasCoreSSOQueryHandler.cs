using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class BuscaUnidadesAdministrativasCoreSSOQueryHandler :
        IRequestHandler<BuscaUnidadesAdministrativasCoreSSOQuery, IEnumerable<SysUnidadeAdministrativa>>
    {
        private readonly IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa;

        public BuscaUnidadesAdministrativasCoreSSOQueryHandler(IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa)
        {
            this.repositorioSysUnidadeAdministrativa = repositorioSysUnidadeAdministrativa ??
                                          throw new ArgumentNullException(nameof(repositorioSysUnidadeAdministrativa));
        }

        public async Task<IEnumerable<SysUnidadeAdministrativa>> Handle(BuscaUnidadesAdministrativasCoreSSOQuery request,
            CancellationToken cancellationToken)
            => await repositorioSysUnidadeAdministrativa.CarregaSysUnidadeAdministrativas();

    }
}
