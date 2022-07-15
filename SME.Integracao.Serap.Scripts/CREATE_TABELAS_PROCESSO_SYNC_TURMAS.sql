USE [GestaoAvaliacao_SGP]
GO

CREATE TABLE [dbo].[PROCESSO_SYNC_TURMAS](
	[pro_id] [UNIQUEIDENTIFIER] NOT NULL,
	[pro_situacao] [int] NOT NULL,
	[pro_dataCriacao] [datetime] NOT NULL,
	[pro_dataAlteracao] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.PROCESSO_SYNC_TURMAS] PRIMARY KEY CLUSTERED 
(
	[pro_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ESCOLA_SYNC_TURMAS](
	[pro_id] [UNIQUEIDENTIFIER] NOT NULL,
	[codigo_escola] [varchar](20) NOT NULL,
	[dataCriacao] [datetime] NOT NULL,
	[dataAlteracao] [datetime] NOT NULL
)
GO
