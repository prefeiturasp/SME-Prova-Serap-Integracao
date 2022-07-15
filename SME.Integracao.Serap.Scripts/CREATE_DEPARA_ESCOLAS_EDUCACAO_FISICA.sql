USE [GestaoAvaliacao_SGP]

IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DEPARA_ESCOLAS_EDUCACAO_FISICA]') AND type in (N'U'))
BEGIN

CREATE TABLE [dbo].[DEPARA_ESCOLAS_EDUCACAO_FISICA](
	[esc_codigo] [varchar](7) NOT NULL,
	[esc_id] [int] NULL
) ON [PRIMARY]

insert into DEPARA_ESCOLAS_EDUCACAO_FISICA
(esc_codigo, esc_id)
values
('094218',	118),
('093912',	163),
('097721',	248),
('093378',	250),
('018546',	347),
('094382',	368),
('010154',	529),
('099180',	558),
('091812',	560),
('000329',	561)

END

