USE [GestaoAvaliacao_SGP]


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


