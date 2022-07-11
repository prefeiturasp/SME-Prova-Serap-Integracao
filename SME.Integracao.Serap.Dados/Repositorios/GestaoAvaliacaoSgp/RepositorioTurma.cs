using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioTurma : RepositorioGestaoAvaliacaoSgp, IRepositorioTurma
    {
        public RepositorioTurma(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task CriarTempTurmasEol()
		{

			using var conn = ObterConexao();
			try
			{

				var query = QueriesTurma.CriarTempTurmasEol();
				await conn.ExecuteAsync(query.ToString(), commandTimeout: 6000);
				
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

		public async Task UpdatesTempTurmasEol()
		{

			using var conn = ObterConexao();
			try
			{

				var query = QueriesTurma.UpdatesTempTurmasEol();
				await conn.ExecuteAsync(query.ToString(), commandTimeout: 6000);

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

		public async Task TratarTurmasEscola(string codigoEscola, int anoBase)
		{

			using var conn = ObterConexao();
			try
			{

				var query = "EXEC [SP_TRATAR_TURMAS_ESCOLA] @codigo_escola = @codigoEscola, @ano = @anoBase";
				await conn.ExecuteAsync(query.ToString(), new { codigoEscola, anoBase }, commandTimeout: 6000);

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
