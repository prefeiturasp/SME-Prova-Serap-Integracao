using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioPessoa
    {
        Task<bool> CriarOuLimparTempDadosPessoa();
        Task<bool> InserirAtualizarDadosPessoa();
    }
}
