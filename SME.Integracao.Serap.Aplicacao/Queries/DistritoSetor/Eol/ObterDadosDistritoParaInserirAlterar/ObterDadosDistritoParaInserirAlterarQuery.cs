using MediatR;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;


namespace SME.Integracao.Serap.Aplicacao
{
    public class ObterDadosDistritoParaInserirAlterarQuery : IRequest<IEnumerable<DadosDistritoDto>>
    {
        public ObterDadosDistritoParaInserirAlterarQuery()
        {

        }
    }
}
