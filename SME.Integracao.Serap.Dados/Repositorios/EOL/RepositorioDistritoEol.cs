using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioDistritoEol : RepositorioEOL, IRepositorioDistritoEol
    {
        public RepositorioDistritoEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<DadosDistritoDto>> ObterDadosDistritos()
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();
				var query = new StringBuilder();

				query.AppendLine(QueriesDistrito.CriarTmpCoreSMEUnidadeEducacaoDadosGerais());
				query.AppendLine(QueriesDistrito.CarregarTmpCoreSMEUnidadeEducacaoDadosGerais());
				query.AppendLine(QueriesDistrito.CriarECarregarTempDre());
				query.AppendLine(QueriesDistrito.DeclararVariaveis());
				query.AppendLine(QueriesDistrito.ObterDadosParaInserirAlterar());
				query.AppendLine(QueriesDistrito.RemoverTabelasTemporarias());

				query = query.Replace("@linkedServerSME", linkedServerSME);
				return await conn.QueryAsync<DadosDistritoDto>(query.ToString(),
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
