using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioProcessoSyncTurmas : RepositorioGestaoAvaliacaoSgp, IRepositorioProcessoSyncTurmas
    {
        public RepositorioProcessoSyncTurmas(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<bool> InserirProcesso(ProcessoSyncTurmas pro)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"insert into PROCESSO_SYNC_TURMAS (pro_id,pro_situacao,pro_dataCriacao,pro_dataAlteracao) 
								values (@id,@situacao,@dataCriacao,@dataAlteracao)";

				await conn.ExecuteAsync(query.ToString(), new { pro.Id, pro.Situacao, pro.DataCriacao, pro.DataAlteracao }, commandTimeout: 600);

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

		public async Task<bool> AlterarProcesso(ProcessoSyncTurmas pro)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"update PROCESSO_SYNC_TURMAS
								set pro_situacao = @Situacao, pro_dataAlteracao = getdate()
								where pro_id = @Id";

				await conn.ExecuteAsync(query.ToString(), new { pro.Id, pro.Situacao }, commandTimeout: 600);

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

		public async Task<ProcessoSyncTurmas> ObterProcesso(Guid proId)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"select pro_id Id, pro_situacao Situacao, pro_dataCriacao DataCriacao, pro_dataAlteracao DataAlteracao 
								from PROCESSO_SYNC_TURMAS where pro_id = @proId";				

				return await conn.QueryFirstOrDefaultAsync<ProcessoSyncTurmas>(query.ToString(), new { proId }, commandTimeout: 600);
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

		public async Task<bool> InserirEscolaProcesso(EscolaSyncTurmas esc)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"insert into ESCOLA_SYNC_TURMAS (pro_id,codigo_escola,dataCriacao,dataAlteracao) 
								values (@proid,@codigoescola,@dataCriacao,@dataAlteracao)";

				await conn.ExecuteAsync(query.ToString(), new { esc.ProId, esc.CodigoEscola, esc.DataCriacao, esc.DataAlteracao }, commandTimeout: 600);

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

		public async Task<IEnumerable<EscolaSyncTurmas>> ObterEscolasProcesso(Guid proId, int? qtde = null)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @"select pro_id ProId, codigo_escola CodigoEscola, dataCriacao,dataAlteracao
								from ESCOLA_SYNC_TURMAS where pro_id = @proId";

				if (qtde != null && qtde > 0)
                {
					query = @$"select top {qtde}
								pro_id ProId, codigo_escola CodigoEscola, dataCriacao,dataAlteracao
								from ESCOLA_SYNC_TURMAS where pro_id = @proId";
				}

				return await conn.QueryAsync<EscolaSyncTurmas>(query.ToString(), new { proId }, commandTimeout: 600);
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

		public async Task<bool> ExcluirEscolasProcesso(Guid proId, string[] codigosEscolas)
		{

			using var conn = ObterConexao();
			try
			{
				var query = @$"delete from ESCOLA_SYNC_TURMAS
								where pro_id = @proId and codigo_escola in({string.Join(",", codigosEscolas)})";

				await conn.ExecuteAsync(query.ToString(), new { proId }, commandTimeout: 600);

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
