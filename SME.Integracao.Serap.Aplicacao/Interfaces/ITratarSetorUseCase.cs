using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITratarSetorUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
