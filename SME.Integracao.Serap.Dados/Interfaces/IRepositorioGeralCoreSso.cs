using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioGeralCoreSso
    {
        Task<Guid> ObterCidId();
    }
}
