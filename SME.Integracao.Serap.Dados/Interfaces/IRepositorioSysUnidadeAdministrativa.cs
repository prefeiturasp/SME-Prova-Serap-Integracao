using SME.Integracao.Serap.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioSysUnidadeAdministrativa
    {
        Task AtualizaSysUnidadeAdministativa();

        Task<IEnumerable<SysUnidadeAdministrativa>> CarregaSysUnidadeAdministrativas();

        Task<object> InserirUnidadeAdministrativa(SysUnidadeAdministrativa novaUnidadeAdministrativa);

        Task AtualizarDistritoSetor(SysUnidadeAdministrativa distritoSetor);

    }
}