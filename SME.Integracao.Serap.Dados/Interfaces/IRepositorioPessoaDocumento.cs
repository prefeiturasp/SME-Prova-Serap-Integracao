using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioPessoaDocumento
    {
        Task<bool> InserirAtualizarPessoaDocumento(int numeroPagina, long numeroRegistros);
    }
}
