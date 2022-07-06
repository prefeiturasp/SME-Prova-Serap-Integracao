
namespace SME.Integracao.Serap.Dados
{
    public static class QueriesTurma
    {
		internal static string CriarTempTurmasEol()
		{
			return @"
					IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'TEMP_TURMAS_EOL')
						BEGIN

							CREATE TABLE [dbo].[TEMP_TURMAS_EOL](
								[cd_turma_escola] [int] NOT NULL,
								[dc_tipo_periodicidade] [varchar](40) NULL,
								[an_letivo] [smallint] NOT NULL,
								[cd_escola] [char](6) NOT NULL,
								[cd_serie_ensino] [int] NULL,
								[dc_serie_ensino] [varchar](40) NULL,
								[cd_modalidade_ensino] [int] NULL,
								[dc_modalidade_ensino] [varchar](40) NULL,
								[cd_etapa_ensino] [int] NULL,
								[dc_etapa_ensino] [varchar](60) NULL,
								[cd_ciclo_ensino] [int] NULL,
								[dc_ciclo_ensino] [varchar](20) NULL,
								[cd_tipo_projeto] [int] NULL,
								[dc_tipo_projeto] [varchar](40) NULL,
								[cd_tipo_habilitacao_profissional] [int] NULL,
								[dc_tipo_habilitacao_profissional] [varchar](30) NULL,
								[dc_turma_escola] [varchar](100) NOT NULL,
								[cd_ambiente_escola] [int] NOT NULL,
								[cd_tipo_atendimento] [int] NOT NULL,
								[dc_tipo_atendimento] [varchar](20) NULL,
								[cd_tipo_turma] [int] NOT NULL,
								[dc_tipo_turma] [varchar](70) NULL,
								[cd_tipo_programa] [int] NULL,
								[dc_tipo_programa] [varchar](40) NULL,
								[qt_vaga_oferecida] [tinyint] NOT NULL,
								[st_turma_escola] [char](1) NOT NULL,
								[cd_tipo_turno] [int] NOT NULL,
								[dc_tipo_turno] [varchar](20) NULL,
								[cd_duracao] [int] NOT NULL,
								[qt_hora_duracao] [smallint] NULL,
								[qt_minuto_duracao] [smallint] NULL,
								[ho_entrada] [char](4) NOT NULL,
								[ho_saida] [char](4) NOT NULL,
								[dt_inicio_turma] [datetime] NOT NULL,
								[dt_fim_turma] [datetime] NOT NULL,
								[dt_inicio] [datetime] NOT NULL,
								[dt_fim] [datetime] NULL,
								[cd_serie_eol98] [int] NULL,
								[cd_serie_eol2007] [int] NULL,
								[sg_serie_eol98] [char](20) NULL,
								[cd_serie_eol98_texto] [char](5) NULL,
								[hr_entrada] [time](0) NULL,
								[hr_saida] [time](0) NULL,
								[tur_descricao] [varchar](200) NULL,
								[tne_nome] [varchar](100) NULL,
								[tme_nome] [varchar](100) NULL,
								[crp_ciclo] [varchar](100) NULL
							) ON [PRIMARY]

						END
				";
		}

		internal static string CarregaTempTurmasEolFiltroCursosIntegracao()
		{
			return @$"
						insert into [@linkedServerSME].[GestaoAvaliacao_SGP].[dbo].[TEMP_TURMAS_EOL]
							({ColunasTempTurmasEol()})
						select
							{ColunasTurmasEol()}
						from {TabelasEol()}
						where e.cd_escola = @codigoEscola
							and (se.cd_serie_ensino IS NOT NULL)
							and (se.cd_modalidade_ensino IS NOT NULL)
							and (ee.cd_etapa_ensino IS NOT NULL)
							and (ee.cd_etapa_ensino IN (1,2,3,4,5,6,9,10,11,13,14,17))
							and (ce.cd_ciclo_ensino IS NOT NULL)
							and (te.an_letivo >= @anoLetivo)
							and (EXISTS (select 1 from alunos_matriculas_norm where CodigoTurma = te.cd_turma_escola))
				";
		}

		internal static string CarregaTempTurmasEolFiltroTipoPrograma()
		{
			return @$"
						insert into [@linkedServerSME].[GestaoAvaliacao_SGP].[dbo].[TEMP_TURMAS_EOL]
							({ColunasTempTurmasEol()})
						select
							{ColunasTurmasEol()}
						from {TabelasEol()}
						where e.cd_escola = @codigoEscola
							and te.an_letivo >= @anoLetivo
							and cd_tipo_turma = 3
							and cd_tipo_programa in (301, 302, 303, 99, 100, 96)
							and (EXISTS (select 1 from alunos_matriculas_norm where CodigoTurma = te.cd_turma_escola))
				";
		}

		internal static string CarregaTempTurmasEolFiltroTipoEdFisica()
		{
			return @$"
						insert into [@linkedServerSME].[GestaoAvaliacao_SGP].[dbo].[TEMP_TURMAS_EOL]
							({ColunasTempTurmasEol()})
						select
							{ColunasTurmasEol()}
						from {TabelasEol()}
						where e.cd_escola = @codigoEscola
							and te.an_letivo >= @anoLetivo
							and cd_tipo_turma = 2
							and (EXISTS (select 1 from alunos_matriculas_norm where CodigoTurma = te.cd_turma_escola))
				";
		}

		internal static string CarregaTempTurmasEolColunaEtapaEnsinoMedio()
		{
			return @$"
						insert into [@linkedServerSME].[GestaoAvaliacao_SGP].[dbo].[TEMP_TURMAS_EOL]
							({ColunasTempTurmasEol()})
						select
							{ColunasTurmasEolEnsinoMedio()}
						from {TabelasEol()}
						where e.cd_escola = @codigoEscola
							and te.an_letivo >= @anoLetivo
							and cd_tipo_turma in (09,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51)
							and (EXISTS (select 1 from alunos_matriculas_norm where CodigoTurma = te.cd_turma_escola))
				";
		}

		internal static string ColunasTempTurmasEol()
		{
			return @"
						cd_turma_escola,dc_tipo_periodicidade,an_letivo,cd_escola,cd_serie_ensino,dc_serie_ensino,cd_modalidade_ensino,
						cd_etapa_ensino,dc_etapa_ensino,cd_ciclo_ensino,dc_ciclo_ensino,dc_turma_escola,cd_ambiente_escola,cd_tipo_atendimento,
						cd_tipo_turma,cd_tipo_programa,qt_vaga_oferecida,st_turma_escola,cd_tipo_turno,cd_duracao,ho_entrada,ho_saida,
						dt_inicio_turma,dt_fim_turma,dt_inicio,dt_fim
				";
		}

		internal static string ColunasTurmasEol()
		{
			return @"
						te.cd_turma_escola,tp.dc_tipo_periodicidade,te.an_letivo,te.cd_escola,se.cd_serie_ensino,se.dc_serie_ensino,
							se.cd_modalidade_ensino,ee.cd_etapa_ensino,ee.dc_etapa_ensino,ce.cd_ciclo_ensino,ce.dc_ciclo_ensino,
						left( ( CASE WHEN ce.cd_ciclo_ensino IN ( 20, 13 )
								THEN te.dc_turma_escola + ' - ' + se.dc_serie_ensino
								ELSE te.dc_turma_escola
							END ),15) AS dc_turma_escola,
							cd_ambiente_escola,cd_tipo_atendimento,cd_tipo_turma,cd_tipo_programa,qt_vaga_oferecida,st_turma_escola,
							cd_tipo_turno,cd_duracao,te.ho_entrada,te.ho_saida,te.dt_inicio_turma,te.dt_fim_turma,te.dt_inicio,te.dt_fim
				";
		}

		internal static string ColunasTurmasEolEnsinoMedio()
		{
			return @"
						te.cd_turma_escola,tp.dc_tipo_periodicidade,te.an_letivo,te.cd_escola,se.cd_serie_ensino,se.dc_serie_ensino,
							se.cd_modalidade_ensino,
						CASE
						WHEN ee.cd_etapa_ensino IS NULL 
							THEN 6 
						ELSE ee.cd_etapa_ensino END as cd_etapa_ensino,
						CASE 
						WHEN ee.dc_etapa_ensino IS NULL 
						THEN 'ENSINO MEDIO' 
						ELSE ee.dc_etapa_ensino END as dc_etapa_ensino,
							ce.cd_ciclo_ensino,ce.dc_ciclo_ensino,
						left( ( CASE WHEN ce.cd_ciclo_ensino IN ( 20, 13 )
								THEN te.dc_turma_escola + ' - ' + se.dc_serie_ensino
								ELSE te.dc_turma_escola
							END ),15) AS dc_turma_escola,
							cd_ambiente_escola,cd_tipo_atendimento,cd_tipo_turma,cd_tipo_programa,qt_vaga_oferecida,st_turma_escola,
							cd_tipo_turno,cd_duracao,te.ho_entrada,te.ho_saida,te.dt_inicio_turma,te.dt_fim_turma,te.dt_inicio,te.dt_fim
				";
		}

		internal static string TabelasEol()
		{
			return @"
						turma_escola te
							inner join tipo_periodicidade tp on te.cd_tipo_periodicidade = tp.cd_tipo_periodicidade
							inner join serie_turma_escola ste on ste.cd_turma_escola = te.cd_turma_escola
							inner join escola e on te.cd_escola = e.cd_escola
							inner join serie_ensino se on se.cd_serie_ensino = ste.cd_serie_ensino
							inner join etapa_ensino ee on ee.cd_etapa_ensino = se.cd_etapa_ensino
							inner join ciclo_ensino ce on ce.cd_ciclo_ensino = se.cd_ciclo_ensino
				";
		}
	}
}
