using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioTurma
    {
        Task CriarTempTurmasEol();
        Task UpdatesTempTurmasEol();
        Task TratarTurmasEscola(string codigoEscola, int anoBase);
    }
}
