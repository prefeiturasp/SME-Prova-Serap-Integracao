using Dapper;
using SME.Integracao.Serap.Infra;
using System;
using System.Data;
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

		public async Task UpdatesTempTurmasEol(string codigoEscola)
		{

			using var conn = ObterConexao();
			var tran = conn.BeginTransaction();
			try
			{

				var query = QueriesTurma.UpdatesTempTurmasEol();
				await conn.ExecuteAsync(query.ToString(), new { codigoEscola }, transaction: tran, commandTimeout: 6000);
				if (tran != null)
					tran.Commit();
			}
			catch (Exception ex)
			{
				if (tran != null)
					tran.Rollback();

				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public async Task RemoverDadosTempTurmasEolPorEscola(string codigoEscola)
		{

			using var conn = ObterConexao();
			var tran = conn.BeginTransaction();
			try
			{

				var query = "delete from TEMP_TURMAS_EOL where cd_escola = @codigoEscola";
				await conn.ExecuteAsync(query.ToString(), new { codigoEscola }, transaction: tran, commandTimeout: 6000);
				if (tran != null)
					tran.Commit();
			}
			catch (Exception ex)
			{
				if (tran != null)
					tran.Rollback();

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
			var tran = conn.BeginTransaction();
			try
			{

				var proc = "[SP_TRATAR_TURMAS_ESCOLA]";
				var parametros = new { codigo_escola = codigoEscola, ano = anoBase };
				await conn.ExecuteAsync(proc, parametros, transaction: tran, commandType: CommandType.StoredProcedure, commandTimeout: 6000);

				if (tran != null)
					tran.Commit();
			}
			catch (Exception ex)
			{
				if (tran != null)
					tran.Rollback();

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
