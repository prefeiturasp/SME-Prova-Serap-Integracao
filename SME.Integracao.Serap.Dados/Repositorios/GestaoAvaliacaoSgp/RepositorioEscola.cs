using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioEscola : RepositorioGestaoAvaliacaoSgp, IRepositorioEscola
	{
        public RepositorioEscola(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<string>> ObterCodigoEscolasAtivas()
		{

			using var conn = ObterConexao();
			try
			{
				var query = "select esc_codigo from ESC_Escola where esc_situacao = 1";

				return await conn.QueryAsync<string>(query.ToString(), commandTimeout: 600);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}
	}
}
