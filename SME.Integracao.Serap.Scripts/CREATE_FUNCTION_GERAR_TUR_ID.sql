USE [GestaoAvaliacao_SGP]
GO
CREATE FUNCTION dbo.GERAR_TUR_ID ()
RETURNS bigint
AS
BEGIN

	DECLARE @max_id bigint = (select max(tur_id) from TUR_Turma);

	RETURN @max_id + 1;

END

