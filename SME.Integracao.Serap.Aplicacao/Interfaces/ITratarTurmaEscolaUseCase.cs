using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface ITratarTurmaEscolaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
