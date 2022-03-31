using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados.Repositorios.EOL
{
    public class RepositorioEOL
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioEOL(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }
        
    }
}
