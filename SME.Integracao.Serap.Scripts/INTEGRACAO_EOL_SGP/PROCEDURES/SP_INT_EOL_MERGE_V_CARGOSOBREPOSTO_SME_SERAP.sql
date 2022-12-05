USE [msdb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_INT_EOL_MERGE_V_CARGOSOBREPOSTO_SME_SERAP]
	@db_origem VARCHAR(100),
	@db_destino VARCHAR(100)
AS
BEGIN

	EXEC ('MERGE ' + @db_destino + '.[dbo].[v_cargosobreposto_sme_serap] destino
USING ' + @db_origem + '.[dbo].[v_cargosobreposto_mstech] as origem
	ON (origem.cd_cargo_base_servidor = destino.cd_cargo_base_servidor
	and origem.cd_cargo = destino.cd_cargo
	and origem.cd_unidade_local_servico = destino.cd_unidade_local_servico)
WHEN MATCHED
	THEN UPDATE SET 
 destino.dc_cargo				  = origem.dc_cargo
WHEN NOT MATCHED BY TARGET 
		THEN
	INSERT  (cd_cargo_base_servidor
			 ,cd_cargo
			 ,dc_cargo
			 ,cd_unidade_local_servico
			 )		
		 VALUES
			 (origem.cd_cargo_base_servidor
			  ,origem.cd_cargo
			  ,origem.dc_cargo
			  ,origem.cd_unidade_local_servico
			 )		      
WHEN NOT MATCHED BY SOURCE 
	THEN DELETE;');

END;