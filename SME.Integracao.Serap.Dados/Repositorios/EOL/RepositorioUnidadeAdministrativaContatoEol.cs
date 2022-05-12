using Dapper;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioUnidadeAdministrativaContatoEol : RepositorioEOL, IRepositorioUnidadeAdministrativaContatoEol
    {

        public RepositorioUnidadeAdministrativaContatoEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<TempDispContatoDto>> ObterDadosEmail(ParametrosTipoMeioContatoCoreSsoDto param)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();
				var query = @"								
								SELECT ent_id EntId, uad_id UadId, isnull(uac_id, newid()) UacId, tmc_id TmcId, uac_contato UacContato
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
								where ord_dispositivo = 1
							";

				return await conn.QueryAsync<TempDispContatoDto>(query,
								new
								{
									linkedServerSME,
									tmc_mail = param.TmcMail,
									tua_id = param.TuaId
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
										tua_id = param.TuaId
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
									tua_id = param.TuaId
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
									tua_id = param.TuaId
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
									tua_id = param.TuaId
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
									tua_id = param.TuaId
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
