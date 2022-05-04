using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioEndEndereco
    {
        Task<object> InserirEndereco(EndEndereco endereco);
        Task AtualizarEndereco(EndEndereco endereco);
    }
}
