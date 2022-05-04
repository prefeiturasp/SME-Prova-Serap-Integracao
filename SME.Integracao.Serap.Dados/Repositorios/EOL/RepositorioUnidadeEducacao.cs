using Dapper;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

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

                var query = @"SELECT top (1000)
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

                return await conn.QueryAsync<UnidadeEducacaoDadosGeraisDto>(query, new { linkedServerSME }, commandTimeout: 600);
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
