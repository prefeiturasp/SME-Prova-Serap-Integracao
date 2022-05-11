

namespace SME.Integracao.Serap.Dados
{
    internal static class QueriesDistrito
    {
        internal static string CriarTmpCoreSMEUnidadeEducacaoDadosGerais()
        {
            return @"
                				IF EXISTS ( SELECT  *
							FROM    sys.objects
							WHERE   object_id = OBJECT_ID(N'[dbo].[tmp_CoreSME_unidade_educacao_dados_gerais]')
									AND type IN ( N'U' ) ) 
					DROP TABLE [dbo].[tmp_CoreSME_unidade_educacao_dados_gerais]
				GO

				CREATE TABLE [dbo].[tmp_CoreSME_unidade_educacao_dados_gerais]
					(
					[cd_unidade_educacao] [char](6) NOT NULL,
					[tp_unidade_administrativa] [int] NULL,
					[dc_tipo_unidade_educacao] [varchar](25) NULL,
					[sg_tp_escola] [char](12) NULL,
					[nm_unidade_educacao] [varchar](60) NULL,
					[tp_situacao_unidade] [int] NULL,
					[sg_tipo_situacao_unidade] [varchar](10) NULL,
					[cd_unidade_administrativa_referencia] [char](6) NULL,
					[sg_unidade_administrativa] [varchar](18) NULL,
					[cd_cie_unidade_educacao] [int] NULL,
					[cd_endereco_grh] [varchar](15) NULL,
					[tp_logradouro] [int] NULL,
					[dc_tp_logradouro] [varchar](20) NULL,
					[nm_logradouro] [varchar](60) NULL,
					[cd_nr_endereco] [char](6) NULL,
					[dc_complemento_endereco] [varchar](30) NULL,
					[nm_bairro] [varchar](40) NULL,
					[cd_cep] [int] NULL,
					[nm_distrito_mec] [varchar](100) NULL,
					[nm_micro_regiao] [varchar](40) NULL,
					[cd_setor_distrito] [smallint] NULL,
					[tp_localizacao_endereco] [char](1) NULL,
					[dc_sub_prefeitura] [varchar](35) NULL,
					[cd_coordenada_geo_x] [decimal](9, 6) NULL,
					[cd_coordenada_geo_y] [decimal](9, 6) NULL,
					[tp_local_funcionamento_escola] [int] NULL,
					[dc_tipo_local_funcionamento_escola] [varchar](50) NULL,
					[tp_dependencia_administrativa] [int] NULL,
					[dc_tipo_dependencia_administrativa] [varchar](40) NULL,
					[tp_forma_ocupacao_predio] [int] NULL,
					[dc_tipo_forma_ocupacao_predio] [varchar](40) NULL,
					[tp_proprietario] [int] NULL,
					[dc_tipo_proprietario] [varchar](40) NULL
					)
				ON  [PRIMARY]
            ";
        }

        internal static string CarregarTmpCoreSMEUnidadeEducacaoDadosGerais()
        {
            return @"
				insert into tmp_CoreSME_unidade_educacao_dados_gerais
				SELECT cast(cd_unidade_educacao as varchar(6)) as cd_unidade_educacao
					  ,tp_unidade_administrativa
					  ,cast(dc_tipo_unidade_educacao as varchar(25)) as dc_tipo_unidade_educacao
					  ,cast(sg_tp_escola as varchar(12)) as sg_tp_escola
					  ,cast(nm_unidade_educacao as varchar(60)) as nm_unidade_educacao
					  ,tp_situacao_unidade
					  ,cast(sg_tipo_situacao_unidade as varchar(10)) as sg_tipo_situacao_unidade
					  ,cast(cd_unidade_administrativa_referencia as varchar(6)) as cd_unidade_administrativa_referencia
					  ,cast(sg_unidade_administrativa as varchar(18)) as sg_unidade_administrativa
					  ,cd_cie_unidade_educacao
					  ,cast(cd_endereco_grh as varchar(15)) as cd_endereco_grh
					  ,tp_logradouro
					  ,cast(dc_tp_logradouro as varchar(20)) as dc_tp_logradouro
					  ,cast(nm_logradouro as varchar(60)) as nm_logradouro
					  ,cast(cd_nr_endereco as varchar(6)) as cd_nr_endereco
					  ,cast(dc_complemento_endereco as varchar(30)) as dc_complemento_endereco
					  ,cast(nm_bairro as varchar(40)) as nm_bairro
					  ,cd_cep
					  ,cast(nm_distrito_mec as varchar(100)) as nm_distrito_mec
					  ,cast(nm_micro_regiao as varchar(40)) as nm_micro_regiao
					  ,cd_setor_distrito
					  ,cast(tp_localizacao_endereco as varchar(1)) as tp_localizacao_endereco
					  ,cast(dc_sub_prefeitura as varchar(35)) as dc_sub_prefeitura
					  ,cd_coordenada_geo_x
					  ,cd_coordenada_geo_y
					  ,tp_local_funcionamento_escola
					  ,cast(dc_tipo_local_funcionamento_escola as varchar(50)) as dc_tipo_local_funcionamento_escola
					  ,tp_dependencia_administrativa
					  ,cast(dc_tipo_dependencia_administrativa as varchar(40)) as dc_tipo_dependencia_administrativa
					  ,tp_forma_ocupacao_predio
					  ,cast(dc_tipo_forma_ocupacao_predio as varchar(40)) as dc_tipo_forma_ocupacao_predio
					  ,tp_proprietario
					  ,cast(dc_tipo_proprietario as varchar(40)) as dc_tipo_proprietario
				  FROM v_unidade_educacao_dados_gerais
            ";
        }

        internal static string CriarECarregarTempDre()
        {
            return @"
					DECLARE @dre AS TABLE (cd_unidade_educacao VARCHAR(20))

					INSERT INTO @dre (cd_unidade_educacao)
					select une.cd_unidade_educacao 
					from tmp_CoreSME_unidade_educacao_dados_gerais une
					INNER JOIN
					(SELECT cd_unidade_administrativa_referencia
						FROM tmp_CoreSME_unidade_educacao_dados_gerais AS vue
						WHERE tp_situacao_unidade = 1
						AND sg_tp_escola IS NOT NULL
						AND sg_tp_escola NOT IN ('ESC.PART.')
						AND sg_tp_escola NOT IN ('E TECNICA')
						GROUP BY cd_unidade_administrativa_referencia) AS dre
					ON une.cd_unidade_educacao = dre.cd_unidade_administrativa_referencia
				";
        }

		internal static string DeclararVariaveis()
		{
			return @"
						DECLARE @ent_id UNIQUEIDENTIFIER, @tua_id_distrito UNIQUEIDENTIFIER, @tua_id_setor UNIQUEIDENTIFIER,
								@qtdSetorInicio INT, @qtdSetorFinal INT, @tua_id_dre UNIQUEIDENTIFIER
    
					SET @ent_id = (SELECT ent_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_Entidade] WHERE LOWER(ent_sigla) = 'smesp')
					SET @tua_id_distrito = (SELECT tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'distrito')
					SET @tua_id_setor = (SELECT tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'setor')
					SET @tua_id_dre = (SELECT tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'diretoria regional de educação')
				";
		}		

		internal static string ObterDadosParaInserirAlterar()
		{
			return @"
						SELECT dis.nm_distrito NomeDistrito, dis.cd_distrito CodigoDistrito, dis.tua_id_distrito TuaIdDistrito, dis.uad_situacao Situacao,
							dis.ent_id EntId, dis.cd_dre CodigoDre, dis.uad_idDre UadIdDre, dis.cd_endereco_grh CodigoEnderecoGrh
						FROM (SELECT DISTINCT ent_id, @tua_id_distrito AS tua_id_distrito, dis.cd_distrito_mec AS cd_distrito,
									dis.nm_distrito_mec AS nm_distrito, uad_superior.uad_id AS uad_idDre,
									cd_unidade_administrativa_referencia AS cd_dre, 1 AS uad_situacao, esc.cd_endereco_grh
								FROM distrito_mec dis
									INNER JOIN tmp_coreSME_unidade_educacao_dados_gerais esc
									ON dis.cd_distrito_mec = LEFT(esc.cd_setor_distrito,2)
									INNER JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad_superior
									ON esc.cd_unidade_administrativa_referencia COLLATE SQL_Latin1_General_CP1_CI_AS = uad_superior.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS
									LEFT JOIN (SELECT  uad_.uad_id AS uad_idFilho, uad_.uad_codigo AS uad_codigoFilho,
														uad_pai.uad_id AS uad_idPai, uad_pai.uad_codigo AS uad_codigoPai
												FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad_
														INNER JOIN [@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad_pai
														ON uad_.uad_idSuperior = uad_pai.uad_id) uad
									ON CAST(dis.cd_distrito_mec AS VARCHAR(20)) = uad.uad_codigoFilho
									AND uad.uad_codigoPai COLLATE SQL_Latin1_General_CP1_CI_AS = esc.cd_unidade_administrativa_referencia COLLATE SQL_Latin1_General_CP1_CI_AS
							WHERE uad.uad_codigoFilho IS NULL
								AND esc.cd_unidade_administrativa_referencia IN
									(SELECT cd_unidade_educacao
										FROM @dre)) AS dis
						GROUP BY dis.nm_distrito, dis.cd_distrito, dis.tua_id_distrito, dis.uad_situacao, dis.ent_id,
								dis.cd_dre, dis.uad_idDre, dis.cd_endereco_grh
					";
		}

		internal static string RemoverTabelasTemporarias()
		{
			return @"
						IF EXISTS ( SELECT  *
							FROM    sys.objects
							WHERE   object_id = OBJECT_ID(N'[dbo].[tmp_CoreSME_unidade_educacao_dados_gerais]')
									AND type IN ( N'U' ) ) 
						DROP TABLE [dbo].[tmp_CoreSME_unidade_educacao_dados_gerais]
						GO
				";
		}		
	}
}
