using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioSysUnidadeAdministrativaContato
    {
        Task<object> InserirUnidadeAdministrativaContato(SysUnidadeAdministrativaContato uac);
        Task AtualizarUnidadeAdministrativaContato(SysUnidadeAdministrativaContato uac);
        Task<IEnumerable<SysUnidadeAdministrativaContato>> ObterUnidadesAdministrativasContatos();
    }
}
