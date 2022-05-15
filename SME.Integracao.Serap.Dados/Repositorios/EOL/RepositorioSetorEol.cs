using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioSetorEol : RepositorioEOL, IRepositorioSetorEol
    {
        public RepositorioSetorEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<DadosSetorDto>> ObterDadosSetores()
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();
				var query = new StringBuilder();

				query.AppendLine(QueriesSetor.DeclararVariaveis());
				query.AppendLine(QueriesSetor.GerarTabelasTemporarias());
				query.AppendLine(QueriesSetor.ObterDadosParaInserirAlterar());

				query = query.Replace("@linkedServerSME", linkedServerSME);
				return await conn.QueryAsync<DadosSetorDto>(query.ToString(),
								new
								{
									linkedServerSME
								},
								commandTimeout: 600);
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
