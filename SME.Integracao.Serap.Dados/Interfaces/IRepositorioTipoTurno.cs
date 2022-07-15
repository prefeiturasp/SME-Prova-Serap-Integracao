using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioTipoTurno
    {
        Task<IEnumerable<TipoTurno>> ObterTipoTurno();
        Task<bool> InserirTipoTurno(TipoTurno tipoTurno);
        Task<bool> ExcluirTipoTurnoPorId(int tipoTurnoId);
    }
}
