using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
   public interface IRepositorioEscEscola
    {
        Task<IEnumerable<EscEscola>> BuscaEscolas();

        Task<object> InserirEscola(EscEscola escola);
    }
}
