using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITratarEnderecoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
