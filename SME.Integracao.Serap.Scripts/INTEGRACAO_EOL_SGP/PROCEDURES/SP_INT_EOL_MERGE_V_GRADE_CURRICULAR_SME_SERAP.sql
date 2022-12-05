USE [msdb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_INT_EOL_MERGE_V_GRADE_CURRICULAR_SME_SERAP]
	@db_origem VARCHAR(100),
	@db_destino VARCHAR(100)
AS
BEGIN

	exec('MERGE ' + @db_destino + '.[dbo].[v_grade_curricular_sme_serap] destino
USING ' + @db_origem + '.[dbo].[v_grade_curricular] as origem
	ON (origem.cd_escola = destino.cd_escola
	and origem.cd_turma_escola = destino.cd_turma_escola
	and origem.cd_cargo_base_servidor = destino.cd_cargo_base_servidor
	and origem.cod_comp_curr = destino.cod_comp_curr
	and origem.rf = destino.rf
	)
WHEN MATCHED
	THEN UPDATE SET 
  destino.carga_horaria	= origem.carga_horaria 
WHEN NOT MATCHED BY TARGET 
		THEN
	INSERT  ( cd_escola
			 ,cd_turma_escola
			 ,cd_cargo_base_servidor
			 ,cod_comp_curr
			 ,carga_horaria
			 ,rf
			 )		
		 VALUES
			 (origem.cd_escola
			 ,origem.cd_turma_escola
			 ,origem.cd_cargo_base_servidor
			 ,origem.cod_comp_curr
			 ,origem.carga_horaria
			 ,origem.rf
			 )		      
WHEN NOT MATCHED BY SOURCE 
	THEN DELETE;');

END;