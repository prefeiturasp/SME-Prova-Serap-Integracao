using MediatR;
using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterListasTipoTurnoTratarQuery : IRequest<(IEnumerable<TipoTurno> Inserir, IEnumerable<TipoTurno> Excluir)>
    {
        public ObterListasTipoTurnoTratarQuery(){}
    }
}
