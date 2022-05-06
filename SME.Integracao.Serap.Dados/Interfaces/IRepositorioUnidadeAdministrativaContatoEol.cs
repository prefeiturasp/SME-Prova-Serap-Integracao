using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioUnidadeAdministrativaContatoEol
    {
        Task MergeEolCoreSsoUnidadeAdministrativaContato();
        Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoSecretariaTelefoneFixoVoz(ParametrosTipoMeioContatoCoreSsoDto param);
        Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoTelefoneFixoVoz(ParametrosTipoMeioContatoCoreSsoDto param);
        Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoFax(ParametrosTipoMeioContatoCoreSsoDto param);
        Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoPabx(ParametrosTipoMeioContatoCoreSsoDto param);
        Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoPublico(ParametrosTipoMeioContatoCoreSsoDto param);
    }
}
