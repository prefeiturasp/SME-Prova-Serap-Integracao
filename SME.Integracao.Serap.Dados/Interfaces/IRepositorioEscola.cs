using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioEscola
    {
        Task<IEnumerable<string>> ObterCodigoEscolasAtivas();
    }
}
