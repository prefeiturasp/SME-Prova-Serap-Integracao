using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioGeralEol
    {
        Task<IEnumerable<TipoTurno>> ObterTipoTurnoEol();
        Task<bool> CarregarTempDadosPessoaCoreSSO();
    }
}
