using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface IPessoaSyncUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
