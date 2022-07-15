using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SME.Integracao.Serap.Dados.Repositorios
{
    public class RepositorioSerap
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioSerap(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        protected IDbConnection ObterConexao()
        {
            var conexao = new SqlConnection(connectionStringOptions.Serap);
            conexao.Open();
            return conexao;
        }

        protected string ObterLinkedServerSME()
        {
            return connectionStringOptions.LinkedServerSME;
        }

    }
}
