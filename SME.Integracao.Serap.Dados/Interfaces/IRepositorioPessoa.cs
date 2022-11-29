using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioPessoa
    {
        Task<bool> CriarOuLimparTempDadosPessoa();
        Task<long> ObterTotalPessoasTratar();
        Task<bool> InserirAtualizarDadosPessoa(int numeroPagina, long numeroRegistros);
    }
}
