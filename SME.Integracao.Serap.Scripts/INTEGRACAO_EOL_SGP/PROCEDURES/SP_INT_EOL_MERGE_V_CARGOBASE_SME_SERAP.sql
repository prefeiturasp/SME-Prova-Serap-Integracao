USE [msdb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_INT_EOL_MERGE_V_CARGOBASE_SME_SERAP]
	@db_origem VARCHAR(100),
	@db_destino VARCHAR(100)
AS
BEGIN
	
	EXEC ('MERGE ' + @db_destino + '.[dbo].[v_cargobase_sme_serap] destino
USING ' + @db_origem + '.[dbo].[v_cargobase_mstech] as origem
	ON (origem.cd_registro_funcional = destino.cd_registro_funcional
	and origem.cd_cargo_base_servidor = destino.cd_cargo_base_servidor)
WHEN NOT MATCHED BY TARGET 
		THEN
	INSERT  (   cd_registro_funcional
				,cd_cargo_base_servidor
				,cd_lotacao_cl
				,cd_vinculo_sigpec
				,cd_cargo
				,dc_cargo
				,cd_situacao_funcional
				,dc_situacao_funcional
				,lotacao
				,dt_inicio
				,cd_motivo_desligamento
				,dt_atualizacao_tabela
				,dt_inicio_exercicio
				,dt_fim_nomeacao
				,Tot_Pts_Col1
				,Tot_Pts_Col2
				,aulas_atrib
				,jex_atrib
				,Cod_Jorn
				,Sigla)		
		 VALUES
			 (
				origem.cd_registro_funcional
				,origem.cd_cargo_base_servidor
				,origem.cd_lotacao_cl
				,origem.cd_vinculo_sigpec
				,origem.cd_cargo
				,origem.dc_cargo
				,origem.cd_situacao_funcional
				,origem.dc_situacao_funcional
				,origem.lotacao
				,origem.dt_inicio
				,origem.cd_motivo_desligamento
				,origem.dt_atualizacao_tabela
				,origem.dt_inicio_exercicio
				,origem.dt_fim_nomeacao
				,origem.Tot_Pts_Col1
				,origem.Tot_Pts_Col2
				,origem.aulas_atrib
				,origem.jex_atrib
				,origem.Cod_Jorn
				,origem.Sigla)		      
WHEN NOT MATCHED BY SOURCE 
	THEN DELETE;');
END;