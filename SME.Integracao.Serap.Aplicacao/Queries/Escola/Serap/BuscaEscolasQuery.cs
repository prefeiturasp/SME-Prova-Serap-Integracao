using MediatR;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao.Queries
{
    public class BuscaEscolasQuery : IRequest<IEnumerable<EscEscola>>
    {
        public BuscaEscolasQuery()
        {

        }
    }
}

