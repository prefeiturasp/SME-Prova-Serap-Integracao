
namespace SME.Integracao.Serap.Dados
{
    internal static class QueriesSetor
    {
		internal static string DeclararVariaveis()
		{
			return @"
						declare @tua_id_setor uniqueidentifier, @ent_id uniqueidentifier, @distrito uniqueidentifier
						SELECT top 1 @tua_id_setor = tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'setor';
						SELECT top 1 @ent_id = ent_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_Entidade] WHERE LOWER(ent_sigla) = 'smesp';
						SELECT top 1 @distrito = tua_id FROM [@linkedServerSME].[CoreSSO].[dbo].[SYS_TipoUnidadeAdministrativa] WHERE LOWER(tua_nome) = 'distrito';
				";
		}

		internal static string GerarTabelasTemporarias()
		{
			return @"
						;with SETOR as (
							SELECT
								RTRIM(LTRIM(LEFT(nm, LEN(nm)-1))) AS nm_distrito
								, nm_micro_regiao AS nm_setor
								, cd_micro_regiao
								, cd_setor
								, LEFT ( CD_SETOR,2) AS CD_DISTRITO
							FROM 
								(
									SELECT
										LEFT(nm_micro_regiao, CHARINDEX('SETOR', nm_micro_regiao)) AS nm
										, CHARINDEX('SETOR', nm_micro_regiao) AS START
										, nm_micro_regiao
										, RIGHT('0000' + CONVERT(VARCHAR(10), cd_setor_distrito), 4) AS cd_setor
										, CASE WHEN LEN(cd_micro_regiao) =1  THEN CAST('0'+CAST (cd_micro_regiao AS VARCHAR(20)) AS VARCHAR(20)) ELSE CAST(cd_micro_regiao AS VARCHAR(20)) END AS cd_micro_regiao
									FROM
										micro_regiao
					
								) AS distrito
							WHERE
								start != 0)
							,DISTRITO as (
							SELECT
								uad_id
								, uad_codigo
								, uad_nome
							FROM
								[@linkedServerSME].[CoreSSO].[dbo].[SYS_UnidadeAdministrativa] uad
							WHERE
								tua_id = @distrito)
							,tmp_distrito_setor as (
							SELECT
								setor.nm_distrito
								, setor.nm_setor
								, setor.cd_setor
								, distrito.uad_id AS uad_idDistrito
								, @tua_id_setor AS tua_id_setor
								, @ent_id AS ent_id
								, 1 AS uad_situacao
							FROM SETOR AS setor
								INNER JOIN
								DISTRITO AS distrito
									ON (distrito.uad_codigo COLLATE SQL_Latin1_General_CP1_CI_AS = setor.cd_distrito COLLATE SQL_Latin1_General_CP1_CI_AS))
					";
		}

		internal static string ObterDadosParaInserirAlterar()
		{
			return @"
						SELECT DISTINCT nm_distrito NomeDistrito, nm_setor NomeSetor, cd_setor CodigoSetor, uad_idDistrito UadIdDistrito, tua_id_setor TuaIdSetor,
							ent_id EntId, uad_situacao Situacao, v_setor.cd_endereco_grh CodigoEnderecoGrh
							FROM tmp_distrito_setor
						inner join v_unidade_educacao_dados_gerais v_setor
						ON tmp_distrito_setor.cd_setor = v_setor.cd_unidade_educacao
					";
		}
	}
}
