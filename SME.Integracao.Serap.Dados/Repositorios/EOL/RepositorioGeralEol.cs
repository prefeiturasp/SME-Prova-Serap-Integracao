using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioGeralEol : RepositorioEOL, IRepositorioGeralEol
    {
        public RepositorioGeralEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<TipoTurno>> ObterTipoTurnoEol()
		{

			using var conn = ObterConexao();
			try
			{
				var query = "select cd_tipo_turno Id, dc_tipo_turno Nome from tipo_turno";

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

        public async Task<bool> CarregarTempDadosPessoaCoreSSO()
        {
            using var conn = ObterConexao();
            try
            {
                string linkedServerSME = ObterLinkedServerSME();
                var query = @"insert into [@linkedServerSME].[CoreSSO].[dbo].[TEMP_DADOS_PESSOA]
									(nm_pessoa, dt_nascimento_pessoa, cd_sexo_pessoa, cd_cpf_pessoa, cd_registro_funcional,
											cd_cargo_base_servidor, lotacao, origem, cd_cargo, dc_cargo, cd_situacao_funcional,
											dc_situacao_funcional, pass, dt_inicio)
									select nm_pessoa, dt_nascimento_pessoa, cd_sexo_pessoa, cd_cpf_pessoa, cd_registro_funcional,
											cd_cargo_base_servidor, lotacao, origem, cd_cargo, dc_cargo, cd_situacao_funcional,
											dc_situacao_funcional, pass, dt_inicio	   
										from (SELECT cast(srv.nm_pessoa as varchar(70)) as nm_pessoa, srv.dt_nascimento_pessoa, srv.cd_sexo_pessoa,
													cast(srv.cd_cpf_pessoa as varchar(11)) as cd_cpf_pessoa,
													cast(cgs.cd_registro_funcional as varchar(7)) as cd_registro_funcional, cgs.cd_cargo_base_servidor,
													cast(cgs.lotacao as varchar(6)) as lotacao, cgs.origem, cgs.cd_cargo,
													cast(cgs.dc_cargo as varchar(50)) as dc_cargo, cgs.cd_situacao_funcional,
													cast(cgs.dc_situacao_funcional as varchar(40)) as dc_situacao_funcional,
													CAST('aaa' as VARCHAR(256)) as pass, ISNULL(cgs.dt_inicio, GETDATE()) AS dt_inicio
												FROM (SELECT cd_registro_funcional, cd_cargo_base_servidor, lotacao, 0 AS origem, cb.cd_cargo, cb.dc_cargo,
															cb.cd_situacao_funcional, cb.dc_situacao_funcional, cb.dt_inicio
														FROM dbo.v_cargobase_sme_serap cb
														UNION ALL
													SELECT cb.cd_registro_funcional, cs.cd_cargo_base_servidor, cs.cd_unidade_local_servico, 1 AS origem,
															cs.cd_cargo, cs.dc_cargo, NULL AS cd_situacao_funcional, NULL AS dc_situacao_funcional, NULL
														FROM dbo.v_cargosobreposto_sme_serap cs
															INNER JOIN dbo.v_cargobase_sme_serap cb
															ON cb.cd_cargo_base_servidor = cs.cd_cargo_base_servidor
														UNION ALL
													SELECT srv.cd_registro_funcional, cb.cd_cargo_base_servidor, lotacao, 0 AS origem, cb.cd_cargo,
															cb.dc_cargo, isnull(cb.cd_situacao_funcional,1) as  cd_situacao_funcional,
															cb.dc_situacao_funcional, cb.dt_inicio
														FROM v_servidor_sme_serap srv 
															inner join v_cargobase_sme_serap cb
															ON srv.cd_registro_funcional = cb.cd_registro_funcional
															INNER JOIN [@linkedServerSME].[GestaoPedagogica].[dbo].[RHU_Cargo]
															ON cb.cd_cargo = RHU_Cargo.crg_codigo
															AND isnull(cb.cd_situacao_funcional,1) = RHU_Cargo.tvi_id
														WHERE dc_cargo <> 'COORDENADOR PEDAGOGICO'
														AND crg_situacao <> 3 AND crg_cargoDocente = 1
														AND srv.cd_registro_funcional NOT IN 
															(SELECT rf
																FROM (select prf.cd_registro_funcional rf, gc.cd_cargo_base_servidor
																		from v_cadastro_professor_sme_serap prf 
																			inner join v_grade_curricular_sme_serap gc
																			on prf.cd_registro_funcional = gc.rf
																			inner join v_unidade_educacao_dados_gerais esc
																			on gc.cd_escola COLLATE Latin1_General_CI_AS = esc.cd_unidade_educacao
																		where esc. dc_tipo_unidade_educacao = 'ESCOLA' 
																		and esc.sg_tp_escola in ('CEU EMEF','EMEF','EMEFM','EMEBS','CIEJA')
																	group by prf.cd_registro_funcional, gc.cd_cargo_base_servidor) prof
																where prof.rf = srv.cd_registro_funcional
																and prof.cd_cargo_base_servidor = cb.cd_cargo_base_servidor)) cgs
													INNER JOIN dbo.v_servidor_sme_serap srv
													ON srv.cd_registro_funcional = cgs.cd_registro_funcional
												WHERE lotacao IS NOT NULL
												AND dt_nascimento_pessoa IS NOT NULL) tabela
										group by nm_pessoa, dt_nascimento_pessoa, cd_sexo_pessoa, cd_cpf_pessoa, cd_registro_funcional,
											cd_cargo_base_servidor, lotacao, origem, cd_cargo, dc_cargo, cd_situacao_funcional,
											dc_situacao_funcional, pass, dt_inicio";

                query = query.Replace("@linkedServerSME", linkedServerSME);
                await conn.ExecuteAsync(query, commandTimeout: 60000);
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
