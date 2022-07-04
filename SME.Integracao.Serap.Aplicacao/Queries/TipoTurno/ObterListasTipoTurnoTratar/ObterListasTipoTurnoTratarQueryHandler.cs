using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterListasTipoTurnoTratarQueryHandler : IRequestHandler<ObterListasTipoTurnoTratarQuery, (IEnumerable<TipoTurno> Inserir, IEnumerable<TipoTurno> Excluir)>
    {

        private readonly IRepositorioTipoTurno repositorioTipoTurno;
        private readonly IRepositorioGeralEol repositorioGeralEol;

        public ObterListasTipoTurnoTratarQueryHandler(IRepositorioTipoTurno repositorioTipoTurno, IRepositorioGeralEol repositorioGeralEol)
        {
            this.repositorioTipoTurno = repositorioTipoTurno ?? throw new ArgumentNullException(nameof(repositorioTipoTurno));
            this.repositorioGeralEol = repositorioGeralEol ?? throw new ArgumentNullException(nameof(repositorioGeralEol));
        }

        public async Task<(IEnumerable<TipoTurno> Inserir, IEnumerable<TipoTurno> Excluir)> Handle(ObterListasTipoTurnoTratarQuery request, CancellationToken cancellationToken)
        {
            var tipoTurnoEol = await repositorioGeralEol.ObterTipoTurnoEol();
            var tipoTurnoSerap = await repositorioTipoTurno.ObterTipoTurno();

            var inserir = tipoTurnoEol.Where(eol => !tipoTurnoSerap.Any(s => s.Nome == eol.Nome));
            var excluir = tipoTurnoSerap.Where(s => !tipoTurnoEol.Any(eol => eol.Nome == s.Nome));

            return (inserir, excluir);
        }
    }
}
