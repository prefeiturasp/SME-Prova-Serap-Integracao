using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface ITratarTipoTurnoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
