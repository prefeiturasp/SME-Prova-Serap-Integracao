using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterDadosContatosQueryHandler : IRequestHandler<ObterDadosContatosQuery, IEnumerable<TempDispContatoDto>>
    {
        private readonly IRepositorioGeralCoreSso repositorioGeralCoreSso;
        private readonly IRepositorioUnidadeAdministrativaContatoEol repositorioUacEol;

        public ObterDadosContatosQueryHandler(IRepositorioGeralCoreSso repositorioGeralCoreSso, IRepositorioUnidadeAdministrativaContatoEol repositorioUacEol)
        {
            this.repositorioGeralCoreSso = repositorioGeralCoreSso ??
                                          throw new ArgumentNullException(nameof(repositorioGeralCoreSso));
            this.repositorioUacEol = repositorioUacEol ??
                              throw new ArgumentNullException(nameof(repositorioUacEol));
        }

        public async Task<IEnumerable<TempDispContatoDto>> Handle(ObterDadosContatosQuery request, CancellationToken cancellationToken)
        {
            var listaRetorno = new List<TempDispContatoDto>();

            var param = await repositorioGeralCoreSso.ObterParametrosTipoMeioContatoCoreSso();

            var contatosEmail = await repositorioUacEol.ObterDadosEmail(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosEmail);

            var contatosSecretariaTelefoneFixoVoz = await repositorioUacEol.ObterDadosContatoSecretariaTelefoneFixoVoz(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosSecretariaTelefoneFixoVoz);

            var contatosTelefoneFixoVoz = await repositorioUacEol.ObterDadosContatoTelefoneFixoVoz(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosTelefoneFixoVoz);

            var contatosFax = await repositorioUacEol.ObterDadosContatoFax(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosFax);

            var contatosPabx = await repositorioUacEol.ObterDadosContatoPabx(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosPabx);

            var contatosPublico = await repositorioUacEol.ObterDadosContatoPabx(param);
            listaRetorno = IncluirContatosListaRetorno(listaRetorno, contatosPublico);

            return listaRetorno.AsEnumerable();
        }

        private List<TempDispContatoDto> IncluirContatosListaRetorno(List<TempDispContatoDto> listaRetorno, IEnumerable<TempDispContatoDto> contatosIncluir)
        {
            if (!listaRetorno.Any())
                return contatosIncluir.ToList();

            var retorno = listaRetorno;
            var incluir = contatosIncluir.Where(c => !listaRetorno.Any(ret => ret.UadId == c.UadId)).ToList();
            retorno.AddRange(incluir);

            return retorno;
        }
    }
}
