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
  public  class RepositorioCoreSSOBase
    {

        private readonly ConnectionStringOptions _connectionStrings;

        public RepositorioCoreSSOBase(ConnectionStringOptions connectionStrings)
        {
            _connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        protected IDbConnection ObterConexao()
        {
            
            var conexao = new SqlConnection(_connectionStrings.CoreSSO);
            conexao.Open();
            return conexao;
        }

       
    }
}
