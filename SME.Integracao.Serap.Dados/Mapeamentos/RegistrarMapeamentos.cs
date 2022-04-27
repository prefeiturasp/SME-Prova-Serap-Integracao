using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class RegistrarMapeamentos
    {
        public static void Registrar()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new SysUnidadeAdministrativaMap());

                config.ForDommel();
            });
        }
    }
}
