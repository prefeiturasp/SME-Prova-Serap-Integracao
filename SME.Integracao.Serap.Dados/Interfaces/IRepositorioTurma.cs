using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioTurma
    {
        Task CriarTempTurmasEol();
        Task UpdatesTempTurmasEol(string codigoEscola);
        Task RemoverDadosTempTurmasEolPorEscola(string codigoEscola);
        Task TratarTurmasEscola(string codigoEscola, int anoBase);
    }
}
