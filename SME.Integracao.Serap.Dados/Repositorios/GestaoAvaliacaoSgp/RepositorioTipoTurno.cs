using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioTipoTurno : RepositorioGestaoAvaliacaoSgp, IRepositorioTipoTurno
    {
        public RepositorioTipoTurno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<TipoTurno>> ObterTipoTurno()
		{

			using var conn = ObterConexao();
			try
			{
				var query = "select ttn_id Id, ttn_nome Nome from ACA_TipoTurno where ttn_situacao = 1";

				return await conn.QueryAsync<TipoTurno>(query.ToString(), commandTimeout: 600);
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

		public async Task<bool> InserirTipoTurno(TipoTurno tipoTurno)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"declare @id int = (select max(ttn_id) + 1 from ACA_TipoTurno)
								INSERT INTO ACA_TipoTurno(ttn_id, ttn_nome, ttn_situacao, ttn_dataCriacao, ttn_dataAlteracao)
								VALUES(@id, @Nome, 1, getdate(), getdate())";

				await conn.ExecuteAsync(query.ToString(), new { tipoTurno.Nome }, commandTimeout: 600);
				return true;
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

		public async Task<bool> ExcluirTipoTurnoPorId(int tipoTurnoId)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"update ACA_TipoTurno set ttn_situacao = 3 where ttn_id = @tipoTurnoId";

				await conn.ExecuteAsync(query.ToString(), new { tipoTurnoId }, commandTimeout: 600);
				return true;
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
