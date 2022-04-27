using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioEOL
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioEOL(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        protected IDbConnection ObterConexao()
        {
            var conexao = new SqlConnection(connectionStringOptions.Eol);
            conexao.Open();
            return conexao;
        }


    }
}
