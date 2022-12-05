USE [msdb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SP_INT_EOL_MERGE_V_CADASTRO_PROFESSOR_SME_SERAP]
	@db_origem VARCHAR(100),
	@db_destino VARCHAR(100)
AS
BEGIN

	exec('MERGE ' + @db_destino + '.[dbo].[v_cadastro_professor_sme_serap] destino
USING ' + @db_origem + '.[dbo].[v_cadastro_professor] as origem
	ON (origem.cd_unidade_educacao = destino.cd_unidade_educacao
	and origem.cd_cargo_base_servidor = destino.cd_cargo_base_servidor
	and origem.cd_registro_funcional = destino.cd_registro_funcional
	and origem.cd_cpf_pessoa = destino.cd_cpf_pessoa
	)
WHEN MATCHED
	THEN UPDATE SET 
  destino.nm_pessoa		  = origem.nm_pessoa
 ,destino.carga_horaria	  = origem.carga_horaria
 ,destino.hora_atividade  = origem.hora_atividade
 ,destino.dc_cargo		  = origem.dc_cargo
 ,destino.cd_cargo		  = origem.cd_cargo
WHEN NOT MATCHED BY TARGET 
		THEN
	INSERT  (cd_unidade_educacao
			 ,cd_cargo_base_servidor
			 ,nm_pessoa
			 ,carga_horaria
			 ,hora_atividade
			 ,cd_registro_funcional
			 ,dc_cargo
			 ,cd_cargo
			 ,cd_cpf_pessoa
			 )		
		 VALUES
			 (origem.cd_unidade_educacao
			 ,origem.cd_cargo_base_servidor
			 ,origem.nm_pessoa
			 ,origem.carga_horaria
			 ,origem.hora_atividade
			 ,origem.cd_registro_funcional
			 ,origem.dc_cargo
			 ,origem.cd_cargo
			 ,origem.cd_cpf_pessoa
			 )		      
WHEN NOT MATCHED BY SOURCE 
	THEN DELETE;');


END;