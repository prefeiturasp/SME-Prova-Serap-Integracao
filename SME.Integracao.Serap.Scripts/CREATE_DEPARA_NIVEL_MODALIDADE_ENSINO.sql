USE [GestaoAvaliacao_SGP]

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DEPARA_NIVEL_MODALIDADE_ENSINO')
BEGIN

CREATE TABLE [dbo].[DEPARA_NIVEL_MODALIDADE_ENSINO](
	[cd_etapa_ensino] [int] NULL,
	[dc_etapa_ensino] [varchar](40) NULL,
	[tne_id] [int] NULL,
	[tne_nome] [varchar](100) NULL,
	[tme_id] [int] NULL,
	[tme_nome] [varchar](100) NULL
) ON [PRIMARY]

insert into DEPARA_NIVEL_MODALIDADE_ENSINO 
(cd_etapa_ensino,dc_etapa_ensino,tne_id,tne_nome,tme_id,tme_nome)
values
(1,	'EDUCACAO INFANTIL'				,1,	'Educação Infantil'	,1,	'Regular'),
(2,	'EJA CIEJA'						,2,	'Ensino Fundamental',8,	'CIEJA'),
(3,	'EJA ESCOLAS ENSINO FUNDAMENTAL',2,	'Ensino Fundamental',6,	'EJA Regular'),
(4,	'ENSINO FUNDAMENTAL DE 8 ANOS'	,2,	'Ensino Fundamental',1,	'Regular'),
(5,	'ENSINO FUNDAMENTAL DE 9 ANOS'	,2,	'Ensino Fundamental',1,	'Regular'),
(6,	'ENSINO MEDIO'					,3,	'Ensino Médio'		,1,	'Regular'),
(9,	'ENSINO MEDIO NORMAL/MAGISTERIO',3,	'Ensino Médio'		,1,	'Regular'),
(10,'EDUCACAO INFANTIL'				,1,	'Educação Infantil'	,2,	'Especial'),
(11,'EJA ESCOLAS EDUCACAO ESPECIAL'	,2,	'Ensino Fundamental',4,	'EJA Especial'),
(12,'ENSINO FUNDAMENTAL 8 ANOS'		,2,	'Ensino Fundamental',2,	'Especial'),
(13,'ENSINO FUNDAMENTAL 9 ANOS'		,2,	'Ensino Fundamental',2,	'Especial'),
(14,'TECNICO MEDIO'					,3,	'Ensino Médio'		,1,	'Regular'),
(17,'ESPEC ENS MEDIO'				,3,	'Ensino Médio'		,2,	'Especial'),
(18,'ESPEC ENS INFANTIL'			,1,	'Ensino Infantil'	,2,	'Especial')

END
