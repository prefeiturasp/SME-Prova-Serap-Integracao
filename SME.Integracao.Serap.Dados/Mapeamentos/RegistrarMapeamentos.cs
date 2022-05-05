using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new SysUnidadeAdministrativaMap());
                config.AddMap(new EndEnderecoMap());
                config.AddMap(new SysUnidadeAdministrativaEnderecoMap());

                config.ForDommel();
            });
        }
    }
}
