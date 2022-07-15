using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioProcessoSyncTurmas
    {
        Task<bool> InserirProcesso(ProcessoSyncTurmas pro);
        Task<bool> AlterarProcesso(ProcessoSyncTurmas pro);
        Task<ProcessoSyncTurmas> ObterProcesso(Guid proId);
        Task<bool> InserirEscolaProcesso(EscolaSyncTurmas esc);
        Task<IEnumerable<EscolaSyncTurmas>> ObterEscolasProcesso(Guid proId, int? qtde = null);
        Task<bool> ExcluirEscolasProcesso(Guid proId, string[] codigosEscolas);
    }
}
