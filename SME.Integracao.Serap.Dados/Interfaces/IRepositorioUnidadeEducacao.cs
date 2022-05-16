using SME.Integracao.Serap.Infra;
using SME.Integracao.Serap.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
  public  interface IRepositorioUnidadeEducacao
    {
        Task<IEnumerable<UnidadeEducacaoDadosGeraisDto>> BuscaUnidadeEducacaoDadosGerais();
        Task<IEnumerable<EscolaDto>> BuscaEscolas();
        Task<UadIdSuperiorDto> ObterUadIdSuperior(Guid uadId);
    }
}
