using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public interface ITurmaEscolaSyncUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
