using Dapper;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioPessoaDocumento : RepositorioCoreSSOBase, IRepositorioPessoaDocumento
    {
        public RepositorioPessoaDocumento(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> InserirAtualizarPessoaDocumento(int numeroPagina, long numeroRegistros)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"  DECLARE
									@ent_id_smesp UNIQUEIDENTIFIER
									, @tdo_id_cpf UNIQUEIDENTIFIER
		
								SET	@ent_id_smesp = (SELECT ent_id FROM SYS_Entidade WHERE ent_sigla = 'smesp')
								SET @tdo_id_cpf = (SELECT tdo_id FROM SYS_TipoDocumentacao WHERE tdo_sigla = 'cpf')

								declare @DADOS_PESSOA table([pes_id] [uniqueidentifier] NULL,
																						[nm_pessoa] [varchar](70) NULL,
																						[dt_nascimento_pessoa] [datetime] NULL,
																						[cd_sexo_pessoa] [char](1) NULL,
																						[cd_cpf_pessoa] [varchar](11) NULL,
																						[cd_registro_funcional] [varchar](7) NULL,
																						[cd_cargo_base_servidor] [int] NOT NULL,
																						[lotacao] [varchar](6) NULL,
																						[origem] [int] NOT NULL,
																						[cd_cargo] [int] NOT NULL,
																						[dc_cargo] [varchar](50) NULL,
																						[cd_situacao_funcional] [int] NULL,
																						[dc_situacao_funcional] [varchar](40) NULL,
																						[pass] [varchar](256) NULL,
																						[dt_inicio] [datetime] NOT NULL);

															insert into @DADOS_PESSOA
															SELECT  a.pes_id
																	,a.nm_pessoa
																	,a.dt_nascimento_pessoa
																	,a.cd_sexo_pessoa
																	,a.cd_cpf_pessoa
																	,a.cd_registro_funcional
																	,a.cd_cargo_base_servidor
																	,a.lotacao
																	,a.origem
																	,a.cd_cargo
																	,a.dc_cargo
																	,a.cd_situacao_funcional
																	,a.dc_situacao_funcional
																	,a.pass
																	,a.dt_inicio								
															FROM (
															SELECT *,
															ROW_NUMBER() OVER (ORDER BY cd_registro_funcional) AS NumLinha
															FROM TEMP_DADOS_PESSOA
															 ) AS A
															WHERE A.NumLinha BETWEEN ((@NumeroPagina-1)*@NumeroRegistros)+1
															AND @NumeroRegistros*(@NumeroPagina)
		
								UPDATE @DADOS_PESSOA SET
									pes_id = usu.pes_id
								FROM
									@DADOS_PESSOA tmp
									INNER JOIN
									(
										SELECT
											_u.usu_id
											, _u.pes_id
											, _u.usu_login
										FROM
											(
												SELECT
													t.usu_id
													, t.usu_login
													, t.pes_id
													, ROW_NUMBER() OVER (PARTITION BY t.usu_id ORDER BY t.qtd DESC) AS rowNum
												FROM
													(
														SELECT
															ut.usu_id
															, ut.usu_login
															, ut.pes_id
															, CASE
																WHEN EXISTS(SELECT us.usu_id FROM SYS_UsuarioGrupo us 
																INNER JOIN SYS_Grupo gru ON (gru.gru_id = us.gru_id) 
																WHERE gru.sis_id = 55 AND us.usu_id = ut.usu_id) THEN 1000
																ELSE ut.qtd
															END AS qtd
														FROM
															(
																SELECT
																	u.usu_id
																	, u.usu_login
																	, u.pes_id
																	, COUNT(u.usu_id) AS qtd
																FROM
																	SYS_Usuario u
																	LEFT JOIN SYS_UsuarioGrupo ug
																		ON (ug.usu_id = u.usu_id)
																	INNER JOIN @DADOS_PESSOA c
																		ON (c.cd_registro_funcional = u.usu_login)
																WHERE
																	ent_id = @ent_id_smesp
																GROUP BY
																	u.usu_id
																	, u.usu_login
																	, u.pes_id
															) AS ut
													) AS t
											) AS _u	
									) AS usu
										ON (usu.usu_login = tmp.cd_registro_funcional)		

								MERGE INTO PES_PessoaDocumento _target
								USING
								(
									SELECT
										crg.cd_cpf_pessoa AS psd_numero
										, @tdo_id_cpf AS tdo_id
										, crg.pes_id
										, 1 AS psd_situacao
									FROM
										@DADOS_PESSOA crg
										LEFT JOIN PES_PessoaDocumento psd
											ON (psd.tdo_id = @tdo_id_cpf
												AND psd.psd_numero = crg.cd_cpf_pessoa)
									WHERE crg.pes_id is not null
									GROUP BY
										cd_cpf_pessoa
										, crg.pes_id
								) AS _source
								ON (_source.pes_id = _target.pes_id
									AND _source.tdo_id = _target.tdo_id)
								WHEN MATCHED THEN
									UPDATE SET
										psd_dataAlteracao = GETDATE()
										, psd_numero = _source.psd_numero
								WHEN NOT MATCHED THEN
									INSERT
									(
										pes_id
										, tdo_id
										, psd_numero
										, psd_situacao			
									)
									VALUES
									(
										_source.pes_id
										, _source.tdo_id
										, _source.psd_numero
										, _source.psd_situacao
									);";

                await conn.ExecuteAsync(query, new { numeroPagina, numeroRegistros }, commandTimeout: 60000);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
