using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioSysUnidadeAdministrativaEndereco
    {
        Task<object> InserirUnidadeAdministrativaEndereco(SysUnidadeAdministrativaEndereco unidadeAdministrativaEndereco);
        Task AtualizarUnidadeAdministrativaEndereco(SysUnidadeAdministrativaEndereco uae);
        Task<IEnumerable<SysUnidadeAdministrativaEndereco>> ObterSysUnidadesAdministrativasEnderecos();
    }
}
