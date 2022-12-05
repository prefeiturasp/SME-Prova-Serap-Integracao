using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface IPessoaDocumentoTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
