USE [GestaoAvaliacao_SGP]
GO
/****** Object:  StoredProcedure [dbo].[SP_TRATAR_TURMAS_ESCOLA]    Script Date: 14/07/2022 13:15:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_TRATAR_TURMAS_ESCOLA]
@codigo_escola varchar(10),
@ano INT = null
AS
BEGIN
    
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	CREATE TABLE #DEPARA_TURMA(
		tur_id     BIGINT NOT NULL,
		tur_codigo VARCHAR(30) NOT NULL)

	DECLARE @EscolasComEJA AS TABLE(esc_id BIGINT NOT NULL)
	           
	CREATE TABLE #TUR_Turma(
		tur_id bigint NULL,
		esc_id int NULL,
		tur_codigo varchar(30) COLLATE DATABASE_DEFAULT NULL,
		tur_descricao varchar(2000) COLLATE DATABASE_DEFAULT NULL,
		tur_vagas int NULL,
		tur_minimoMatriculados int NULL,
		tur_duracao tinyint NULL,
		cal_id int NULL,
		tur_docenteEspecialista bit NULL,
		tur_situacao tinyint NULL,
		tur_tipo tinyint NULL,
		tur_dataAlteracao datetime NULL,
		tur_dataEncerramento datetime NULL)

	CREATE TABLE #temp_inserir(
		tur_id bigint NULL,
		tur_codigo varchar(30) COLLATE DATABASE_DEFAULT NULL)
    
	insert into #TUR_Turma (tur_id, esc_id, tur_codigo, tur_descricao, tur_vagas,
			tur_minimoMatriculados, tur_duracao, cal_id, tur_situacao,
			tur_tipo, tur_docenteEspecialista, tur_dataAlteracao, tur_dataEncerramento)
	SELECT isnull(_out.tur_id, -1) as tur_id, _out.esc_id, _out.tur_codigo, _out.tur_descricao,
			_out.tur_vagas, _out.tur_minimoMatriculados, _out.tur_duracao, _out.cal_id,
			_out.tur_situacao, _out.tur_tipo, _out.tur_docenteEspecialista, _out.tur_dataAlteracao, _out.dt_fim
		FROM (SELECT dep_tur.tur_id, es.esc_id,
					cast(tur.cd_turma_escola as varchar (30)) AS tur_codigo,
					tur.dc_turma_escola AS tur_descricao, tur.qt_vaga_oferecida AS tur_vagas,
					tur.qt_vaga_oferecida/4 AS tur_minimoMatriculados, 1 AS tur_duracao, cal.cal_id AS cal_id,
					1 AS tur_situacao, 1 AS tur_tipo,
					0 AS tur_docenteEspecialista, turm.tur_dataAlteracao, tur.dt_fim
				FROM TEMP_TURMAS_EOL tur with (nolock)
					INNER JOIN ACA_CalendarioAnual cal with (nolock) ON cal.cal_ano = tur.an_letivo and cal.cal_situacao = 1
														AND ((tur.dc_etapa_ensino like '%infantil%' and cal.cal_id = 33) 
																or (tur.dc_etapa_ensino like '%EJA%' and tur.dc_tipo_periodicidade = 'SEMESTRAL INICIO NO 1º SEMESTRE' and cal.cal_id = 34)
																or (tur.dc_etapa_ensino like '%EJA%' and tur.dc_tipo_periodicidade = 'SEMESTRAL INICIO NO 2º SEMESTRE' and cal.cal_id = 35)
																or (tur.dc_etapa_ensino not like '%infantil%' 
																	and tur.dc_etapa_ensino not like '%EJA%' and cal.cal_id = 32))
					INNER JOIN ESC_Escola es with (nolock)
					ON (es.esc_codigo COLLATE DATABASE_DEFAULT = tur.cd_escola COLLATE DATABASE_DEFAULT)
					AND es.esc_situacao <> 3                   
					INNER JOIN ACA_TipoTurno ttn with (nolock)
					ON (ttn.ttn_nome COLLATE DATABASE_DEFAULT = tur.dc_tipo_turno COLLATE DATABASE_DEFAULT)                   
					LEFT JOIN #DEPARA_TURMA dep_tur 
					ON CAST(tur.cd_turma_escola AS VARCHAR(30)) COLLATE DATABASE_DEFAULT = dep_tur.tur_codigo
					LEFT JOIN TUR_TURMA turm with (nolock)
					ON dep_tur.tur_codigo = turm.tur_codigo_eol and dep_tur.tur_codigo is not null
					AND 1 = turm.tur_situacao
				where tur.st_turma_escola <> 'E'
				and tur.cd_tipo_turma not in (2, 3)
				and tur.cd_escola = @codigo_escola) AS _out
		GROUP BY isnull(tur_id, -1), esc_id, tur_codigo, tur_descricao, tur_vagas,
				tur_minimoMatriculados, tur_duracao, cal_id,
				tur_situacao, tur_tipo, tur_docenteEspecialista, tur_dataAlteracao, dt_fim
    
	UPDATE #TUR_Turma SET tur_dataAlteracao = GETDATE() where tur_dataAlteracao is null	

	DECLARE @cal_idEF int
	DECLARE @cal_idEI int
	DECLARE @cal_idEJA int
	DECLARE @cal_idEJA2 int
	select top 1 @cal_idEF = cal_id from ACA_CalendarioAnual with (nolock)
		where cal_ano = @ano and cal_descricao not like '%Infant%' and cal_descricao not like '%EJA%'
	select top 1 @cal_idEI = cal_id from ACA_CalendarioAnual with (nolock)
		where cal_ano = @ano and cal_descricao like '%Infant%'
	select top 1 @cal_idEJA = cal_id from ACA_CalendarioAnual with (nolock)
		where cal_ano = @ano and cal_descricao like '%1°%EJA%'
	select top 1 @cal_idEJA2 = cal_id from ACA_CalendarioAnual with (nolock)
		where cal_ano = @ano and cal_descricao like '%2°%EJA%'
	 
	
	insert into #temp_inserir
	select t.tur_id, tmp.tur_codigo	
	from #TUR_Turma tmp
	left join TUR_TURMA t on t.tur_codigo_eol = tmp.tur_codigo and t.tur_codigo_eol is not null

	MERGE INTO TUR_TURMA _target
	USING #TUR_Turma _source
	ON (_source.tur_codigo = _target.tur_codigo_eol and _target.tur_codigo_eol is not null)
	WHEN MATCHED THEN
		UPDATE SET			
				tur_descricao = NULL 
			, tur_tipo = _source.tur_tipo			
			, tur_situacao = 1
			, tur_dataAlteracao = getdate()	
	WHEN NOT MATCHED BY SOURCE AND _target.tur_situacao = 1 AND _target.tur_tipo = 1 
								AND (_target.cal_id = @cal_idEF or _target.cal_id = @cal_idEI or _target.cal_id = @cal_idEJA or _target.cal_id = @cal_idEJA2) THEN
			UPDATE SET tur_situacao = 7, tur_dataAlteracao = getdate();    	
	
	
	-- ===================================================================
	DECLARE @tur_codigo VARCHAR(20)
	DECLARE cursor_inserir CURSOR FOR 
	SELECT tur_codigo from #temp_inserir
	where tur_id is null

	OPEN cursor_inserir  
	FETCH NEXT FROM cursor_inserir INTO @tur_codigo

	WHILE @@FETCH_STATUS = 0  
	BEGIN

		  insert into TUR_TURMA (tur_id, esc_id, tur_codigo, cal_id, tur_situacao, tur_tipo, 
				tur_dataCriacao, tur_dataAlteracao, tur_codigo_eol)
		  select top 1 (select dbo.GERAR_TUR_ID()), esc_id, tur_descricao, cal_id, tur_situacao, tur_tipo, 
				getdate(), getdate(), tur_codigo
				from #TUR_Turma where tur_codigo = @tur_codigo

		  FETCH NEXT FROM cursor_inserir INTO @tur_codigo 
	END 

	CLOSE cursor_inserir  
	DEALLOCATE cursor_inserir	
	-- ===================================================================
			
	TRUNCATE TABLE #TUR_Turma
    
	-- Turmas de Recuperação Paralela
	insert into #TUR_Turma (tur_id, esc_id, tur_codigo, tur_descricao, tur_vagas,
			tur_minimoMatriculados, tur_duracao, cal_id, tur_situacao,
			tur_tipo, tur_docenteEspecialista, tur_dataAlteracao)
	SELECT isnull(_out.tur_id, -1) as tur_id, _out.esc_id, _out.tur_codigo, _out.tur_descricao,
			_out.tur_vagas, _out.tur_minimoMatriculados, _out.tur_duracao, _out.cal_id,
			_out.tur_situacao, _out.tur_tipo, _out.tur_docenteEspecialista, _out.tur_dataAlteracao
		FROM (SELECT dep_tur.tur_id, es.esc_id,
					cast(tur.cd_turma_escola as varchar (30)) AS tur_codigo,
					tur.dc_turma_escola AS tur_descricao, tur.qt_vaga_oferecida AS tur_vagas,
					tur.qt_vaga_oferecida/4 AS tur_minimoMatriculados, 1 AS tur_duracao, cal.cal_id AS cal_id,
					1 AS tur_situacao, 
					CASE WHEN tur.cd_tipo_programa in (94,95,96,97,362) THEN 5 -- AEE
						ELSE 2  -- RP
					END AS tur_tipo,
					0 AS tur_docenteEspecialista, turm.tur_dataAlteracao
				FROM TEMP_TURMAS_EOL tur with (nolock)
					INNER JOIN ACA_CalendarioAnual cal ON cal.cal_ano = tur.an_letivo and cal.cal_situacao = 1 and cal.cal_id = @cal_idEF
					INNER JOIN ESC_Escola es
					ON (es.esc_codigo COLLATE DATABASE_DEFAULT = tur.cd_escola COLLATE DATABASE_DEFAULT)
					AND es.esc_situacao <> 3
					LEFT JOIN #DEPARA_TURMA dep_tur
					ON CAST(tur.cd_turma_escola AS VARCHAR(30)) COLLATE DATABASE_DEFAULT = dep_tur.tur_codigo
					LEFT JOIN TUR_TURMA turm with (nolock)
					ON dep_tur.tur_codigo = turm.tur_codigo_eol and turm.tur_codigo_eol is not null
					AND turm.tur_situacao <> 3
				where tur.st_turma_escola <> 'E'
				and tur.cd_tipo_turma = 3
				and tur.cd_escola = @codigo_escola) AS _out
		GROUP BY isnull(tur_id, -1), esc_id, tur_codigo, tur_descricao, tur_vagas,
				tur_minimoMatriculados, tur_duracao, cal_id,
				tur_situacao, tur_tipo, tur_docenteEspecialista, tur_dataAlteracao   

	insert into @EscolasComEJA (esc_id)
	select tur.esc_id
		from TUR_Turma tur with (nolock)
			inner join ESC_Escola esc on tur.esc_id = esc.esc_id
			inner join TUR_TurmaCurriculo tcr on tcr.tur_id = tur.tur_id and tcr.tcr_situacao <> 3 and tcr.cur_id in (161,162)
			inner join ACA_CalendarioAnual cal on cal.cal_id = tur.cal_id and cal.cal_ano = @ano
		where tur.tur_tipo = 1 and tur.tur_situacao <> 3
		group by tur.esc_id
	  
	-- Turmas de Educação Física  
	insert into #TUR_Turma (tur_id, esc_id, tur_codigo, tur_descricao, tur_vagas,
			tur_minimoMatriculados, tur_duracao, cal_id, tur_situacao,
			tur_tipo, tur_docenteEspecialista, tur_dataAlteracao)
	SELECT isnull(_out.tur_id, -1) as tur_id, _out.esc_id, _out.tur_codigo, _out.tur_descricao,
			_out.tur_vagas, _out.tur_minimoMatriculados, _out.tur_duracao, _out.cal_id,
			_out.tur_situacao, _out.tur_tipo, _out.tur_docenteEspecialista, _out.tur_dataAlteracao
		FROM (SELECT dep_tur.tur_id, es.esc_id,
					cast(tur.cd_turma_escola as varchar (30)) AS tur_codigo,
					tur.dc_turma_escola AS tur_descricao, tur.qt_vaga_oferecida AS tur_vagas,
					tur.qt_vaga_oferecida/4 AS tur_minimoMatriculados, 1 AS tur_duracao, cal.cal_id AS cal_id,
					1 AS tur_situacao,
					3 AS tur_tipo,
					0 AS tur_docenteEspecialista, turm.tur_dataAlteracao
				FROM TEMP_TURMAS_EOL tur with (nolock)
					INNER JOIN ACA_CalendarioAnual cal ON cal.cal_ano = tur.an_letivo and cal.cal_situacao = 1 and cal.cal_id = @cal_idEF
					INNER JOIN ESC_Escola es
					ON (es.esc_codigo COLLATE DATABASE_DEFAULT = tur.cd_escola COLLATE DATABASE_DEFAULT)
					AND es.esc_situacao <> 3                   
					LEFT JOIN DEPARA_ESCOLAS_EDUCACAO_FISICA ped
					ON tur.cd_escola = ped.esc_codigo                   
					LEFT JOIN #DEPARA_TURMA dep_tur
					ON CAST(tur.cd_turma_escola AS VARCHAR(30)) COLLATE DATABASE_DEFAULT = dep_tur.tur_codigo
					LEFT JOIN TUR_Turma turm with (nolock)
					ON dep_tur.tur_codigo = turm.tur_codigo_eol and turm.tur_codigo_eol is not null
					AND turm.tur_situacao <> 3
					LEFT JOIN @EscolasComEJA eja on eja.esc_id = es.esc_id
				where tur.st_turma_escola <> 'E'
				and tur.cd_tipo_turma = 2
				and tur.cd_escola = @codigo_escola
				and (ped.esc_codigo is not null
					or es.esc_nome like 'EMEFM%' or eja.esc_id is not null) --add para pegar as turmas de ensino médio e turmas de EJA
				) AS _out
		GROUP BY isnull(tur_id, -1), esc_id, tur_codigo, tur_descricao, tur_vagas,
				tur_minimoMatriculados, tur_duracao, cal_id,
				tur_situacao, tur_tipo, tur_docenteEspecialista, tur_dataAlteracao
	
	UPDATE #TUR_Turma SET tur_dataAlteracao = GETDATE() where tur_dataAlteracao is null

	delete from #temp_inserir
	insert into #temp_inserir
	select t.tur_id, tmp.tur_codigo	
	from #TUR_Turma tmp
	left join TUR_TURMA t on t.tur_codigo_eol = tmp.tur_codigo and t.tur_codigo_eol is not null

	MERGE INTO TUR_TURMA _target
	USING #TUR_Turma _source
	ON (_source.tur_codigo = _target.tur_codigo_eol and _source.esc_id = _target.esc_id)		
	WHEN NOT MATCHED BY SOURCE AND _target.tur_tipo in (2,3,5) AND _target.tur_situacao = 1
			AND RIGHT(_target.tur_codigo ,4) <> '- EF' AND _target.cal_id = @cal_idEF  THEN
			UPDATE SET tur_situacao = 7, tur_dataAlteracao = getdate();
    
	-- ===================================================================
	DECLARE cursor_inserir CURSOR FOR 
	SELECT tur_codigo from #temp_inserir
	where tur_id is null

	OPEN cursor_inserir  
	FETCH NEXT FROM cursor_inserir INTO @tur_codigo

	WHILE @@FETCH_STATUS = 0  
	BEGIN

		  insert into TUR_TURMA (tur_id, esc_id, tur_codigo, cal_id, tur_situacao, tur_tipo, 
				tur_dataCriacao, tur_dataAlteracao, tur_codigo_eol)
		  select top 1 (select dbo.GERAR_TUR_ID()), esc_id, tur_descricao, cal_id, tur_situacao, tur_tipo, 
				getdate(), getdate(), tur_codigo
				from #TUR_Turma where tur_codigo = @tur_codigo

		  FETCH NEXT FROM cursor_inserir INTO @tur_codigo 
	END 

	CLOSE cursor_inserir  
	DEALLOCATE cursor_inserir
	-- ===================================================================
	
	delete from TEMP_TURMAS_EOL where cd_escola = @codigo_escola

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;

END
