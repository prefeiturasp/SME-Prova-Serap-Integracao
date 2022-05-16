using SME.Integracao.Serap.Infra;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioGeralCoreSso
    {
        Task<ParametrosCoreSsoDto> ObterParametrosCoreSso();
        Task<ParametrosTipoMeioContatoCoreSsoDto> ObterParametrosTipoMeioContatoCoreSso();
    }
}
