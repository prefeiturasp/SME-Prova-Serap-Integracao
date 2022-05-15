using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITratarDistritoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
