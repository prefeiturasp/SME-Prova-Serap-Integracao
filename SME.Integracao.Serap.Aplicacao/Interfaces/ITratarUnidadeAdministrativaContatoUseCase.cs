using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITratarUnidadeAdministrativaContatoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
