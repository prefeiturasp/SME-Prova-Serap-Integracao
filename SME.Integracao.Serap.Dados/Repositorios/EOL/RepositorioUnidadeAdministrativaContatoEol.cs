using Dapper;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioUnidadeAdministrativaContatoEol : RepositorioEOL, IRepositorioUnidadeAdministrativaContatoEol
    {

        public RepositorioUnidadeAdministrativaContatoEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task MergeEolCoreSsoUnidadeAdministrativaContato()
        {

            using var conn = ObterConexao();
            try
            {
                string linkedServerSME = ObterLinkedServerSME();

                var query = @"DECLARE @tmc_fone UNIQUEIDENTIFIER, @tmc_mail UNIQUEIDENTIFIER, @tua_id UNIQUEIDENTIFIER
							select @tmc_fone = tmc_id from [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoMeioContato] where tmc_nome = 'TELEFONE' and tmc_situacao = 1
							select @tmc_mail = tmc_id from [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoMeioContato] where tmc_nome = 'E-MAIL' and tmc_situacao = 1
							select @tua_id = tua_id from [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] where tua_nome = 'Escola' and tua_situacao = 1

							MERGE INTO [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] _target
							USING (SELECT ent_id, uad_id, isnull(uac_id, newid()) uac_id, tmc_id, uac_contato
									 from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_mail as tmc_id, 
												  uac_contato = CASE WHEN PATINDEX('%@%',dcu.dc_dispositivo) <> 0 THEN dcu.dc_dispositivo
																ELSE nm_contato END,
												  ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
													ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
											 from v_disp_contato_unidades dcu inner join
												  [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												  on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
												  and @tua_id = uad.tua_id
												  LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] uac with (nolock)
												   on uad.ent_id = uac.ent_id
												  and uad.uad_id = uac.uad_id
												  and @tmc_mail = uac.tmc_id
											where uad.uad_situacao = 1
											  and dcu.dc_tipo_dispositivo_comunicacao = 'EMAIL') dados
									   where ord_dispositivo = 1) _source
							 ON _target.ent_id = _source.ent_id
							AND _target.uad_id = _source.uad_id
							AND _target.uac_id = _source.uac_id
							WHEN MATCHED and ((_target.uac_situacao = 3) or (_target.uac_contato <> _source.uac_contato)) THEN
								 UPDATE SET uac_contato = _source.uac_contato,
											uac_situacao = 1,
											uac_dataAlteracao = GETDATE()
							WHEN NOT MATCHED THEN
								 INSERT (ent_id, uad_id, uac_id, tmc_id, uac_contato, uac_situacao, uac_dataCriacao, uac_dataAlteracao)
								 VALUES (_source.ent_id, _source.uad_id, _source.uac_id, _source.tmc_id, _source.uac_contato,
										 1, GETDATE(), GETDATE());";

                await conn.ExecuteAsync(query, new { linkedServerSME }, commandTimeout: 600);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoSecretariaTelefoneFixoVoz(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();

				var query = @"select ent_id EntId, uad_id UadId, isnull(uac_id, newid()) uac_id UacId, tmc_id TmcId, uac_contato UacContato
								  from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_fone as tmc_id, 
											   dcu.dc_dispositivo as uac_contato,
											   ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
															ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
										  from v_disp_contato_unidades dcu inner join
											   [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
											   and @tua_id = uad.tua_id
											   LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] uac with (nolock)
												on uad.ent_id = uac.ent_id
											   and uad.uad_id = uac.uad_id
											   and @tmc_fone = uac.tmc_id
										 where uad.uad_situacao = 1
										   and dcu.dc_tipo_dispositivo_comunicacao = 'TELEFONE FIXO DE VOZ'
										   and dcu.nm_contato IN ('secretária', 'secretaria')) dados
								 where ord_dispositivo = 1";
				
				return await conn.QueryAsync<TempDispContatoDto>(query, 
								new { 
										linkedServerSME,
										tmc_fone = param.TmcFone,
										tmc_mail = param.TmcMail,
										tua_id = param.TuaId
									},
								commandTimeout: 600);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoTelefoneFixoVoz(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();

				var query = @"select ent_id EntId, uad_id UadId, isnull(uac_id, newid()) uac_id UacId, tmc_id TmcId, uac_contato UacContato
								  from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_fone as tmc_id, 
											   dcu.dc_dispositivo as uac_contato,
											   ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
															ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
										  from v_disp_contato_unidades dcu inner join
											   [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
											   and @tua_id = uad.tua_id
											   LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] uac with (nolock)
												on uad.ent_id = uac.ent_id
											   and uad.uad_id = uac.uad_id
											   and @tmc_fone = uac.tmc_id
										 where uad.uad_situacao = 1
										   and dcu.dc_tipo_dispositivo_comunicacao = 'TELEFONE FIXO DE VOZ') dados
								 where ord_dispositivo = 1";

				return await conn.QueryAsync<TempDispContatoDto>(query,
								new
								{
									linkedServerSME,
									tmc_fone = param.TmcFone,
									tmc_mail = param.TmcMail,
									tua_id = param.TuaId
								},
								commandTimeout: 600);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoFax(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();

				var query = @"select ent_id EntId, uad_id UadId, isnull(uac_id, newid()) uac_id UacId, tmc_id TmcId, uac_contato UacContato
								  from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_fone as tmc_id, 
											   dcu.dc_dispositivo as uac_contato,
											   ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
															ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
										  from v_disp_contato_unidades dcu inner join
											   [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
											   and @tua_id = uad.tua_id
											   LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].SYS_UnidadeAdministrativaContato uac with (nolock)
												on uad.ent_id = uac.ent_id
											   and uad.uad_id = uac.uad_id
											   and @tmc_fone = uac.tmc_id
										 where uad.uad_situacao = 1
										   and dcu.dc_tipo_dispositivo_comunicacao = 'FAX') dados
								 where ord_dispositivo = 1;";

				return await conn.QueryAsync<TempDispContatoDto>(query,
								new
								{
									linkedServerSME,
									tmc_fone = param.TmcFone,
									tmc_mail = param.TmcMail,
									tua_id = param.TuaId
								},
								commandTimeout: 600);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoPabx(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();

				var query = @"select ent_id EntId, uad_id UadId, isnull(uac_id, newid()) uac_id UacId, tmc_id TmcId, uac_contato UacContato
								  from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_fone as tmc_id, 
											   dcu.dc_dispositivo as uac_contato,
											   ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
															ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
										  from v_disp_contato_unidades dcu inner join
											   [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
											   and @tua_id = uad.tua_id
											   LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] uac with (nolock)
												on uad.ent_id = uac.ent_id
											   and uad.uad_id = uac.uad_id
											   and @tmc_fone = uac.tmc_id
										 where uad.uad_situacao = 1
										   and dcu.dc_tipo_dispositivo_comunicacao = 'PABX') dados
								 where ord_dispositivo = 1;";

				return await conn.QueryAsync<TempDispContatoDto>(query,
								new
								{
									linkedServerSME,
									tmc_fone = param.TmcFone,
									tmc_mail = param.TmcMail,
									tua_id = param.TuaId
								},
								commandTimeout: 600);
			}
			catch (System.Exception ex)
			{
				throw ex;
			}
			finally
			{
				conn.Close();
				conn.Dispose();
			}
		}

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosContatoPublico(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();

				var query = @"select ent_id EntId, uad_id UadId, isnull(uac_id, newid()) uac_id UacId, tmc_id TmcId, uac_contato UacContato
								  from (select UAD.ent_id, UAD.uad_id, uac.uac_id, @tmc_fone as tmc_id, 
											   dcu.dc_dispositivo as uac_contato,
											   ROW_NUMBER() OVER (PARTITION BY UAD.ent_id, UAD.uad_id
															ORDER BY dcu.dc_dispositivo) AS ord_dispositivo
										  from v_disp_contato_unidades dcu inner join
											   [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad with (nolock)
												on dcu.cd_unidade_educacao COLLATE SQL_Latin1_General_CP1_CI_AS = uad.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
											   and @tua_id = uad.tua_id
											   LEFT JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativaContato] uac with (nolock)
												on uad.ent_id = uac.ent_id
											   and uad.uad_id = uac.uad_id
											   and @tmc_fone = uac.tmc_id
										 where uad.uad_situacao = 1
										   and dcu.dc_tipo_dispositivo_comunicacao = 'PUBLICO') dados
								 where ord_dispositivo = 1;";

				return await conn.QueryAsync<TempDispContatoDto>(query,
								new
								{
									linkedServerSME,
									tmc_fone = param.TmcFone,
									tmc_mail = param.TmcMail,
									tua_id = param.TuaId
								},
								commandTimeout: 600);
			}
			catch (System.Exception ex)
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
