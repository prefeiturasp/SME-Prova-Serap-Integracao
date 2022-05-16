using Dapper;
using SME.Integracao.Serap.Infra;
using SME.Integracao.Serap.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioUnidadeEducacao : RepositorioEOL, IRepositorioUnidadeEducacao
    {
        public RepositorioUnidadeEducacao(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<UnidadeEducacaoDadosGeraisDto>> BuscaUnidadeEducacaoDadosGerais()
        {

            using var conn = ObterConexao();
            try
            {
                string linkedServerSME = ObterLinkedServerSME();

                var query = $@"SELECT
                            		cast(cd_unidade_educacao as varchar(6)) as				 CodigoUnidadeEducacao      
                            		, cast(dc_tipo_unidade_educacao as varchar(25)) as		 DescricaoTipoUnidadeEducacao
                            		, cast(sg_tp_escola as varchar(12)) as					 SiglaTipoEscola            
                            		, cast(nm_unidade_educacao as varchar(60)) as			 NomeUnidadeEducacao        
                            		,	tp_logradouro as									 TipoLogradouro             
                            		, cast(nm_logradouro as varchar(60)) as					 NomeLogradouro             
                            		, cast(cd_nr_endereco as varchar(6))					 CodigoNrEndereco           
                            		, cast(dc_complemento_endereco as varchar(30)) as		 DescricaoComplementoEndereco
                            		, cast(nm_bairro as varchar(40)) as						 NomeBairro                 
                            		, right('00000000' + CONVERT(VARCHAR(8), cd_cep),8) as	 CodigoCep                  
                            		, cast(nm_distrito_mec as varchar(100)) as				 NomeDistritoMec            
                            		, cast(nm_micro_regiao as varchar(40)) as				 NomeMicroRegiao            
                            		, cd_setor_distrito										 CodigoSetorDistrito        
                            		, cast(dc_sub_prefeitura as varchar(35)) as				 DescricaoSubPrefeitura     
                            	    ,  setor.uad_id			                 as 			 UadIdSuperior						                            
                            		, CASE sg_tipo_situacao_unidade	 						  
                            			WHEN 'ATIVA' THEN 1									            
                            		ELSE
                            			3         
                            		END AS                                                    SituacaoUnidadeEducacao 
                            	
	
                              		, (SELECT top 1 tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'escola') AS TuaIdEscola
                              		, (SELECT top 1 ent_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_Entidade] WHERE LOWER(ent_sigla) = 'smesp')                   AS EntId
                              		, cast(cd_unidade_administrativa_referencia as varchar(6))                                                                  as CodigodUnidadeAdministrativaRef
                              	FROM
                              		v_unidade_educacao_dados_gerais ueg WITH(READUNCOMMITTED)
                              		INNER JOIN
                              		(
                              			SELECT
                              				uad_id
                              				, uad_codigo
                              				, uad_nome
                              			FROM
                              				[@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] WITH(READUNCOMMITTED)
                              			WHERE
                              				tua_id = (SELECT tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'setor')
                              		) AS setor
                              			--ON (setor.uad_nome = ueg.nm_micro_regiao)
                              			ON (setor.uad_codigo = ueg.cd_setor_distrito)
                              	
                              		--AND sg_tipo_situacao_unidade = 'ATIVA'
                              	GROUP BY
                              		cd_unidade_educacao 
                              		, dc_tipo_unidade_educacao
                              		, sg_tp_escola
                              		, nm_unidade_educacao
                              		, tp_logradouro 
                              		, nm_logradouro
                              		, cd_nr_endereco
                              		, dc_complemento_endereco
                              		, nm_bairro
                              		, cd_cep
                              		, nm_distrito_mec 
                              		, nm_micro_regiao 
                              		, cd_setor_distrito
                              		, dc_sub_prefeitura
                              		, setor.uad_id 
                              		,sg_tipo_situacao_unidade
                              		, cd_unidade_administrativa_referencia
                              ";

                query = query.Replace("@linkedServerSME", linkedServerSME);
                return await conn.QueryAsync<UnidadeEducacaoDadosGeraisDto>(query, commandTimeout: 600);
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

        public async Task<IEnumerable<EscolaDto>> BuscaEscolas()
        {

            using var conn = ObterConexao();
            try
            {
                string linkedServerSME = ObterLinkedServerSME();
                var query = @"
                                   SELECT  
                                            uad.ent_id  as EntId,
                            				uad.uad_id  as UadId ,
                            				tua_id   as TuaId,
                            				uad_codigo as UadCodigo ,
                            				rtrim(ltrim(esc_pr.sg_tp_escola)) + ' - ' + uad_nome as UadNome,
                            				uad_situacao as UadSituacao,
                            				CASE WHEN esc_pr.dc_tipo_dependencia_administrativa = 'MUNICIPAL'
                            					 THEN 1
                            					 WHEN esc_pr.dc_tipo_dependencia_administrativa = 'PRIVADA'
                            					 THEN 2
                            					 ELSE 1
                            				END AS TreId,
                                            uad_idSuperior as UadIdSuperior
                      
                            	  FROM  [10.49.19.159\SQLSERVERHOMOLOG].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad
                            		   INNER JOIN v_unidade_educacao_dados_gerais esc_pr ON uad.uad_codigo = esc_pr.cd_unidade_educacao
                            	  WHERE     uad.ent_id = '6CF424DC-8EC3-E011-9B36-00155D033206'
                            				AND tua_id = 'e33ef3ba-e4ca-479e-85f1-ed10fd2c0579'
                            				
                            				and dc_tipo_unidade_educacao = 'ESCOLA'
                            				AND sg_tp_escola IN ('EMEF','EMEFM','CEU EMEF','EMEBS','CIEJA',
                            									 'EMEI','CECI','CEMEI','CEI DIRET', --Adicionado by Rodrigo em 2016
                            									 'CCI/CIPS', 'CEI INDIR', 'CEU CEI', 'CEU EMEI', 'CR.P.CONV') --Adicionado by Pedro em 2017
                            	  GROUP BY 	uad.ent_id ,
                            				uad.uad_id ,
                            				tua_id ,
                            				uad_codigo ,
                            				rtrim(ltrim(esc_pr.sg_tp_escola)) + ' - ' + uad_nome ,
                            				uad_situacao,
                            				esc_pr.dc_tipo_dependencia_administrativa,
                            				sg_tp_escola,
                                            uad_idSuperior
                            	UNION
                            	  SELECT uad.ent_id, uad.uad_id, tua_id, uad_codigo,	'CEU GESTAO - ' + uad_nome as uad_nome,	uad_situacao,
                            			 CASE WHEN esc_pr.dc_tipo_dependencia_administrativa = 'MUNICIPAL' THEN 1
                            				  WHEN esc_pr.dc_tipo_dependencia_administrativa = 'PRIVADA'   THEN 2
                            			      ELSE 1
                            			  END AS tre_id,
										  uad_idSuperior
                            	    FROM  [10.49.19.159\SQLSERVERHOMOLOG].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad
                            		     INNER JOIN v_unidade_educacao_dados_gerais esc_pr ON uad.uad_codigo = esc_pr.cd_unidade_educacao
                            		     INNER JOIN  [10.49.19.159\SQLSERVERHOMOLOG].[Manutencao].[dbo].[UNIDADESADM_CEU_GESTAO_PTRF] cg on cg.cd_unidade_educacao = esc_pr.cd_unidade_educacao
                            		 
                            	   WHERE uad.ent_id = '6CF424DC-8EC3-E011-9B36-00155D033206'
                            			 AND tua_id = 'e33ef3ba-e4ca-479e-85f1-ed10fd2c0579'
                            			 
                            	   GROUP BY uad.ent_id, uad.uad_id,	tua_id, uad_codigo,	uad_nome, uad_situacao,
                            				esc_pr.dc_tipo_dependencia_administrativa, sg_tp_escola, uad_idSuperior
                            	UNION
                            	  SELECT uad.ent_id, uad.uad_id, tua_id, uad_codigo,	'E TECNICA - ' + uad_nome as uad_nome,	uad_situacao,
                            			 CASE WHEN esc_pr.dc_tipo_dependencia_administrativa = 'MUNICIPAL' THEN 1
                            				  WHEN esc_pr.dc_tipo_dependencia_administrativa = 'PRIVADA'   THEN 2
                            			      ELSE 1
                            			  END AS tre_id,
										  uad_idSuperior
                            	    FROM  [10.49.19.159\SQLSERVERHOMOLOG].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad
                            		     INNER JOIN v_unidade_educacao_dados_gerais esc_pr ON uad.uad_codigo = esc_pr.cd_unidade_educacao
                            	   WHERE esc_pr.cd_unidade_educacao = '200242' 
                            	     AND tua_id = 'e33ef3ba-e4ca-479e-85f1-ed10fd2c0579'
                            		
                            	   GROUP BY uad.ent_id, uad.uad_id,	tua_id, uad_codigo,	uad_nome, uad_situacao,
                            				esc_pr.dc_tipo_dependencia_administrativa, sg_tp_escola,  uad_idSuperior";

                return await conn.QueryAsync<EscolaDto>(query, new { linkedServerSME }, commandTimeout: 600);

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


        public async Task<UadIdSuperiorDto> ObterUadIdSuperior(Guid uadId)
        {

            using var conn = ObterConexao();
            try
            {
                string linkedServerSME = ObterLinkedServerSME();

                var query = @"SELECT uad_id as UadId 
                                     ,uad_idSuperior as UadIdSuperior
                                     ,uad_nome as UadNome
                                     , tua_id as TuaId
                               FROM [10.49.19.159\SQLSERVERHOMOLOG].[CoreSSO].[dbo].[Sys_UnidadeAdministrativa]

                               Where uad_id = @uadId";



                return await conn.QueryFirstOrDefaultAsync<UadIdSuperiorDto>(query.ToString(),
                                new
                                {
                                    uadId
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
