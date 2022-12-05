USE [msdb]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[SP_INT_EOL_MERGE_V_SERVIDOR_SME_SERAP]
	@db_origem VARCHAR(100),
	@db_destino VARCHAR(100)
AS
BEGIN

EXEC (
	'MERGE ' + @db_destino + '.[dbo].[v_servidor_sme_serap] destino
USING ' + @db_origem + '.[dbo].[v_servidor_mstech] as origem
	ON (origem.cd_registro_funcional = destino.cd_registro_funcional
	and origem.situacao = destino.situacao)
WHEN MATCHED
	THEN UPDATE SET 
 destino.nm_pessoa				  = origem.nm_pessoa
,destino.dt_nascimento_pessoa	  = origem.dt_nascimento_pessoa
,destino.cd_sexo_pessoa			  = origem.cd_sexo_pessoa
,destino.nr_rg_pessoa			  = origem.nr_rg_pessoa
,destino.cd_complemento_rg		  = origem.cd_complemento_rg
,destino.codigo_orgao_emissor_rg  = origem.codigo_orgao_emissor_rg
,destino.dt_emissao_rg			  = origem.dt_emissao_rg
,destino.sg_uf_rg				  = origem.sg_uf_rg
,destino.cd_cpf_pessoa			  = origem.cd_cpf_pessoa
,destino.situacao				  = origem.situacao
WHEN NOT MATCHED BY TARGET 
		THEN
	INSERT  (cd_registro_funcional
			 ,nm_pessoa
			 ,dt_nascimento_pessoa
			 ,cd_sexo_pessoa
			 ,nr_rg_pessoa
			 ,cd_complemento_rg
			 ,codigo_orgao_emissor_rg
			 ,dt_emissao_rg
			 ,sg_uf_rg
			 ,cd_cpf_pessoa
			 ,situacao)		
		 VALUES
			 (origem.cd_registro_funcional
			 ,origem.nm_pessoa
			 ,origem.dt_nascimento_pessoa
			 ,origem.cd_sexo_pessoa
			 ,origem.nr_rg_pessoa
			 ,origem.cd_complemento_rg
			 ,origem.codigo_orgao_emissor_rg
			 ,origem.dt_emissao_rg
			 ,origem.sg_uf_rg
			 ,origem.cd_cpf_pessoa
			 ,origem.situacao)		      
WHEN NOT MATCHED BY SOURCE 
	THEN DELETE;');

END;

