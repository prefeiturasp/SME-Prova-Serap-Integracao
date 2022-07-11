using SME.Integracao.Serap.Infra;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioGestaoAvaliacaoSgp
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioGestaoAvaliacaoSgp(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        protected IDbConnection ObterConexao()
        {
            var conexao = new SqlConnection(connectionStringOptions.GestaoAvaliacaoSgp);
            conexao.Open();
            return conexao;
        }        
    }
}
