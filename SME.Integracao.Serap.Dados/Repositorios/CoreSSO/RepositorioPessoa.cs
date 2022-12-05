using Dapper;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioPessoa : RepositorioCoreSSOBase, IRepositorioPessoa
    {
        public RepositorioPessoa(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> CriarOuLimparTempDadosPessoa()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"IF OBJECT_ID(N'dbo.TEMP_DADOS_PESSOA', N'U') IS NULL
								BEGIN

									CREATE TABLE [dbo].[TEMP_DADOS_PESSOA](
                                    [pes_id] [UNIQUEIDENTIFIER] null,
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
									[dt_inicio] [datetime] NOT NULL
								) ON [PRIMARY]

								END
								ELSE
								BEGIN
									delete from [dbo].[TEMP_DADOS_PESSOA]
								END";

                await conn.ExecuteAsync(query);
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
        public async Task<long> ObterTotalPessoasTratar()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select COUNT(cd_registro_funcional) total from TEMP_DADOS_PESSOA";
                return await conn.QueryFirstOrDefaultAsync<long>(query);
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

        public async Task<bool> InserirAtualizarDadosPessoa(int numeroPagina, long numeroRegistros)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"  DECLARE
									@ent_id_smesp UNIQUEIDENTIFIER
									, @tdo_id_cpf UNIQUEIDENTIFIER

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
		
								SET	@ent_id_smesp = (SELECT ent_id FROM SYS_Entidade WHERE ent_sigla = 'smesp')
								SET @tdo_id_cpf = (SELECT tdo_id FROM SYS_TipoDocumentacao WHERE tdo_sigla = 'cpf')

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
									@DADOS_PESSOA car
									INNER JOIN
									(
										SELECT
											u.pes_id
											, pes.pes_nome
											, u.usu_login
										FROM
											SYS_Usuario u WITH(READUNCOMMITTED)
											INNER JOIN PES_Pessoa pes
												ON (pes.pes_id = u.pes_id)
										WHERE
											u.ent_id = @ent_id_smesp
											AND u.usu_situacao <> 3
											AND pes.pes_situacao = 1
									) AS usu
										ON (usu.usu_login = car.cd_registro_funcional
											AND usu.pes_nome = car.nm_pessoa)
	
								DECLARE @pessoa_inserir table(pes_id UNIQUEIDENTIFIER, psd_numero VARCHAR(11), pes_nome VARCHAR(200), pes_dataNascimento date, pes_sexo tinyint)
								INSERT INTO @pessoa_inserir (pes_id, psd_numero, pes_nome, pes_dataNascimento, pes_sexo)
								SELECT crg.pes_id, crg.cd_cpf_pessoa AS cpf, crg.nm_pessoa AS pes_nome, crg.dt_nascimento_pessoa AS pes_dataNascimento,
										CASE crg.cd_sexo_pessoa
											WHEN 'M' THEN 1
											WHEN 'F' THEN 2
											ELSE NULL
										END AS pes_sexo
									FROM @DADOS_PESSOA crg
									WHERE NOT EXISTS (SELECT pes.pes_id FROM PES_Pessoa pes
													WHERE pes.pes_id = crg.pes_id)
									AND NOT EXISTS (SELECT pes.pes_id FROM PES_Pessoa pes 
													WHERE pes.pes_nome = crg.nm_pessoa and pes.pes_situacao = 1
														and pes.pes_dataNascimento = crg.dt_nascimento_pessoa
														and pes.pes_sexo = CASE crg.cd_sexo_pessoa WHEN 'M' THEN 1 WHEN 'F' THEN 2 ELSE NULL END)
									GROUP BY crg.nm_pessoa, crg.cd_cpf_pessoa, crg.pes_id, crg.dt_nascimento_pessoa, crg.cd_sexo_pessoa

								UPDATE @pessoa_inserir SET pes_id = NEWID() WHERE pes_id IS NULL
	
								INSERT INTO PES_Pessoa (pes_nome, pes_dataNascimento, pes_sexo, pes_situacao, pes_integridade)
								SELECT pes_nome, pes_dataNascimento, pes_sexo, 1, 1
									FROM @pessoa_inserir _source
									WHERE NOT EXISTS (SELECT pes.pes_id FROM PES_Pessoa pes 
													WHERE pes.pes_id = _source.pes_id)";

                await conn.ExecuteAsync(query, new { numeroPagina, numeroRegistros },commandTimeout: 60000);
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
