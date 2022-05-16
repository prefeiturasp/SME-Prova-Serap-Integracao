using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
  public  interface IRepositorioUnidadeEducacao
    {
        Task<IEnumerable<UnidadeEducacaoDadosGeraisDto>> BuscaUnidadeEducacaoDadosGerais();
    }
}
