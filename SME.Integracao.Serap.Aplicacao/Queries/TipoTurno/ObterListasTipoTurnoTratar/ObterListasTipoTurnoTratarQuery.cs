using MediatR;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterListasTipoTurnoTratarQuery : IRequest<(IEnumerable<TipoTurno> Inserir, IEnumerable<TipoTurno> Excluir)>
    {
        public ObterListasTipoTurnoTratarQuery(){}
    }
}
