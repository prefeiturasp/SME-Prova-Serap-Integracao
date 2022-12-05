using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface IPessoaTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
