using Dapper;
using SME.Integracao.Serap.Infra;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioSysUnidadeAdministrativa : RepositorioCoreSSOBase , IRepositorioSysUnidadeAdministrativa
    {
        public RepositorioSysUnidadeAdministrativa(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }


        public async Task LimpaTabelasTemporarias()
        {


            using var conn = ObterConexao();
            try
            {
                var query = "TRUNCATE TABLE tmp_escola; ";
                var result = await SqlMapper.QueryAsync<string>(conn, query, commandTimeout: 600);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }

        public async Task CriaTabelasTemporarias()
        {
            var queryTabelasTemporarias = @"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[end_esc]') AND type in (N'U'))
                                          DROP TABLE [dbo].[end_esc]
                                          GO
                                          SET ANSI_NULLS ON
                                          GO
                                          SET QUOTED_IDENTIFIER ON
                                          GO
                                          SET ANSI_PADDING ON
                                          GO
                                          CREATE TABLE [dbo].[end_esc](
                                          	[end_id] [uniqueidentifier] NULL,
                                          	[nm_logradouro] [varchar](60) NULL
                                          ) ON [PRIMARY]
                                          GO
                                          ";


            using var conn = ObterConexao();
            try
            {


                await SqlMapper.QueryAsync(conn, queryTabelasTemporarias);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task CriaTipoUnidadeAdministrativas()
        {
            using var conn = ObterConexao();
            try
            {
                var query =
                    @"  BEGIN
                             IF NOT EXISTS(SELECT * FROM sso_sys_tipounidadeadministrativa WHERE UPPER(tua_nome) = 'DIRETORIA REGIONAL DE EDUCAÇÃO')
                                 BEGIN
                                     INSERT INTO sso_sys_tipounidadeadministrativa (tua_nome, tua_situacao, tua_dataCriacao, tua_dataAlteracao, tua_integridade)
                                     VALUES ('Diretoria Regional de Educação', 1, GETDATE(), GETDATE(), 0)
                                    END
                             IF NOT EXISTS(SELECT * FROM sso_sys_tipounidadeadministrativa WHERE UPPER(tua_nome) = 'DISTRITO')
                                 BEGIN
                                     INSERT INTO sso_sys_tipounidadeadministrativa (tua_nome, tua_situacao, tua_dataCriacao, tua_dataAlteracao, tua_integridade)
                                     VALUES ('Distrito', 1, GETDATE(), GETDATE(), 0)
                                   END
                              IF NOT EXISTS(SELECT * FROM sso_sys_tipounidadeadministrativa WHERE UPPER(tua_nome) = 'SETOR')
                                 BEGIN
                                    INSERT INTO sso_sys_tipounidadeadministrativa (tua_nome, tua_situacao, tua_dataCriacao, tua_dataAlteracao, tua_integridade)
                                    VALUES ('Setor', 1, GETDATE(), GETDATE(), 0)
                                  END
                             IF NOT EXISTS(SELECT * FROM sso_sys_tipounidadeadministrativa WHERE UPPER(tua_nome) = 'ESCOLA')
                                 BEGIN
                                    INSERT INTO sso_sys_tipounidadeadministrativa (tua_nome, tua_situacao, tua_dataCriacao, tua_dataAlteracao, tua_integridade)
                                    VALUES ('Escola', 1, GETDATE(), GETDATE(), 0)
                                 END
                         END
                      ";
                await SqlMapper.QueryAsync(conn, query);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task CarregaDadosEscolas()
        {
            try
            {
                using var conn = ObterConexao();

                var queryCarregaDadosView =
              @"SELECT
        		cast(cd_unidade_educacao as varchar(6)) as cd_unidade_educacao
        		, cast(dc_tipo_unidade_educacao as varchar(25)) as dc_tipo_unidade_educacao
        		, cast(sg_tp_escola as varchar(12)) as sg_tp_escola
        		, cast(nm_unidade_educacao as varchar(60)) as nm_unidade_educacao
        		, tp_logradouro 
        		, cast(nm_logradouro as varchar(60)) as nm_logradouro
        		, cast(cd_nr_endereco as varchar(6)) as cd_nr_endereco
        		, cast(dc_complemento_endereco as varchar(30)) as dc_complemento_endereco
        		, cast(nm_bairro as varchar(40)) as nm_bairro
        		, cd_cep
        		, cast(nm_distrito_mec as varchar(100)) as nm_distrito_mec
        		, cast(nm_micro_regiao as varchar(40)) as nm_micro_regiao
        		, cd_setor_distrito
        		, cast(dc_sub_prefeitura as varchar(35)) as dc_sub_prefeitura
        		, setor.uad_id AS uad_idSuperior
        		, CASE sg_tipo_situacao_unidade
        			WHEN 'ATIVA' THEN 1
        		ELSE
        			3
        		END AS uad_situacao
        		, (SELECT top 1 tua_id FROM SSO_SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'escola') AS tua_id_escola
        		, (SELECT top 1 ent_id FROM SSO_SYS_Entidade WHERE LOWER(ent_sigla) = 'smesp') AS ent_id
        		, cast(cd_unidade_administrativa_referencia as varchar(6)) as cd_unidade_administrativa_referencia
        	FROM
        		[DB_EDUCACAO.REDE.SP].[se1426].[dbo].[v_unidade_educacao_dados_gerais] ueg WITH(READUNCOMMITTED)
        		INNER JOIN
        		(
        			SELECT
        				uad_id
        				, uad_codigo
        				, uad_nome
        			FROM
        				SSO_SYS_UnidadeAdministrativa WITH(READUNCOMMITTED)
        			WHERE
        				tua_id = (SELECT tua_id FROM SSO_SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'setor')
        		) AS setor
        			--ON (setor.uad_nome = ueg.nm_micro_regiao)
        			ON (setor.uad_codigo = ueg.cd_setor_distrito)
        	WHERE
        		dc_tipo_unidade_educacao = 'ESCOLA'
        		--AND sg_tipo_situacao_unidade = 'ATIVA'
        	GROUP BY
        		cd_unidade_educacao 
        		, dc_tipo_unidade_educacao
        		, sg_tp_escola
        		, nm_unidade_educacao
        		, tp_logradouro 
        		, nm_logradouro
        		, cd_nr_endereco
        		, dc_complemento_endereco
        		, nm_bairro
        		, cd_cep
        		, nm_distrito_mec 
        		, nm_micro_regiao 
        		, cd_setor_distrito
        		, dc_sub_prefeitura
        		, setor.uad_id 
        		,sg_tipo_situacao_unidade
        		, cd_unidade_administrativa_referencia";

                var result = await SqlMapper.QueryAsync<string>(conn, queryCarregaDadosView);

            }
            catch (System.Exception)
            {
                throw;
            }

        }

        public async Task ImportarEscolas()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"BEGIN
                DECLARE @ent_id UNIQUEIDENTIFIER, @tua_id_escola UNIQUEIDENTIFIER, @tua_id_setor UNIQUEIDENTIFIER,
                        @tua_id_biblioteca UNIQUEIDENTIFIER
                
                SET @ent_id = (SELECT ent_id FROM CoreSSO..SYS_Entidade WHERE LOWER(ent_sigla) = 'smesp')
                SET @tua_id_escola = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'escola')
                SET @tua_id_setor = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'setor')
                SET @tua_id_biblioteca = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'Biblioteca')
                
                MERGE CoreSSO..SYS_UnidadeAdministrativa _target
                USING (SELECT cd_unidade_educacao AS uad_codigo, dc_tipo_unidade_educacao, nm_unidade_educacao AS uad_nome,
                              nm_logradouro, cd_nr_endereco, nm_bairro, cd_setor_distrito, nm_micro_regiao AS nm_setor,
                              LEFT(cd_setor_distrito, 2) as cd_distrito, nm_distrito_mec AS nm_distrito,
                              setor.uad_id AS uad_idSuperior, cd_unidade_administrativa_referencia as cd_dre,
                              CASE sg_tipo_situacao_unidade WHEN 'ATIVA' THEN 1 ELSE 3 END AS uad_situacao,
                              @tua_id_escola AS tua_id_escola, @ent_id AS ent_id, cd_endereco_grh
                         FROM tmp_CoreSME_unidade_educacao_dados_gerais ueg WITH(READUNCOMMITTED)
                              INNER JOIN
                              (SELECT uad_id, uad_codigo, uad_nome,
                                      ROW_NUMBER() OVER (PARTITION BY uad_codigo ORDER BY uad_dataCriacao) AS rowNum
                                 FROM CoreSSO..SYS_UnidadeAdministrativa WITH(READUNCOMMITTED)
                                WHERE tua_id = @tua_id_setor) AS setor
                               ON RTRIM(LTRIM(setor.uad_codigo)) = ueg.cd_setor_distrito
                              AND setor.rowNum = 1
                        WHERE ((dc_tipo_unidade_educacao = 'ESCOLA')
                               -- filtro para pegar os CEUs 
                               or(cd_unidade_educacao like '200%'
                                   and dc_tipo_unidade_educacao = 'UNIDADE ADMINISTRATIVA'))
                        GROUP BY cd_unidade_educacao, dc_tipo_unidade_educacao, nm_unidade_educacao, nm_logradouro,
                                 cd_nr_endereco, nm_bairro, cd_setor_distrito, nm_micro_regiao, nm_distrito_mec,
                                 setor.uad_id, sg_tipo_situacao_unidade, cd_unidade_administrativa_referencia,
                                 cd_endereco_grh) AS _source
                 ON _source.uad_codigo = _target.uad_codigo
                AND _source.tua_id_escola = _target.tua_id
                AND _source.ent_id = _target.ent_id
                WHEN MATCHED THEN
                     UPDATE SET uad_nome = _source.uad_nome,
                                uad_dataAlteracao = GETDATE(),
                                uad_idSuperior = _source.uad_idSuperior,
                                uad_codigoIntegracao = _source.cd_endereco_grh,
                                uad_situacao = _source.uad_situacao
                WHEN NOT MATCHED THEN
                     INSERT(ent_id, tua_id, uad_codigo, uad_nome, uad_idSuperior, uad_situacao)
                     VALUES(@ent_id, @tua_id_escola, _source.uad_codigo, _source.uad_nome,
                             _source.uad_idSuperior, _source.uad_situacao)
                WHEN NOT MATCHED BY SOURCE AND((_target.tua_id = @tua_id_escola) AND(_target.uad_nome not like 'LAB DRE%'))
                     THEN
                     UPDATE SET uad_situacao = 3, uad_dataAlteracao = GETDATE();
            
                            --Insere a biblioteca da escola.
                MERGE CoreSSO..SYS_UnidadeAdministrativa _target
                USING(select isnull(bib.uad_id, newid()) as uad_id, esc.ent_id, @tua_id_biblioteca as tua_id,
                              esc.uad_nome, esc.uad_id as uad_idSuperior, esc.uad_situacao
                         from CoreSSO..SYS_UnidadeAdministrativa esc
                              left
                         join CoreSSO..SYS_UnidadeAdministrativa bib
            
                           on esc.uad_id = bib.uad_idSuperior
            
                          and bib.tua_id = @tua_id_biblioteca
                        where esc.tua_id = @tua_id_escola
                          ) as _source
                ON _source.uad_id = _target.uad_id
                WHEN MATCHED AND(_target.uad_nome <> _source.uad_nome) THEN
                    UPDATE SET uad_nome = _source.uad_nome,
                               uad_dataAlteracao = GETDATE()
                WHEN NOT MATCHED THEN
                     INSERT(uad_id, ent_id, tua_id, uad_codigo, uad_nome, uad_idSuperior, uad_situacao)
                     VALUES(_source.uad_id, _source.ent_id, _source.tua_id, '', _source.uad_nome,
                             _source.uad_idSuperior, _source.uad_situacao);
            
                            MERGE CoreSME..SYS_TipoEscola _target
                USING(select isnull(sg_tp_escola, 'CEU') sg_tp_escola
                         from tmp_CoreSME_unidade_educacao_dados_gerais
                        where ((dc_tipo_unidade_educacao = 'ESCOLA')
                               -- filtro para pegar os CEUs Puros
                               or(cd_unidade_educacao like '200%'
                                   and dc_tipo_unidade_educacao = 'UNIDADE ADMINISTRATIVA'))
                        group by isnull(sg_tp_escola, 'CEU')) AS _source
                ON _target.tes_desc = _source.sg_tp_escola
                WHEN NOT MATCHED THEN
                     INSERT(tes_id, tes_desc, tes_data, tes_excluido)
                     VALUES(NEWID(), _source.sg_tp_escola, GETDATE(), 0);
            
                            MERGE CoreSME..SYS_UnidadeAdministrativa_TipoEscola _target
                USING(SELECT uad_id, tes_id
                         FROM CoreSSO..SYS_UnidadeAdministrativa ua
                              INNER JOIN
                              (select cd_unidade_educacao, isnull(sg_tp_escola, 'CEU') sg_tp_escola
                                 from tmp_CoreSME_unidade_educacao_dados_gerais
                                where ((dc_tipo_unidade_educacao = 'ESCOLA')
                                      -- filtro para pegar os CEUs Puros
                                      or(cd_unidade_educacao like '200%'
                                          and dc_tipo_unidade_educacao = 'UNIDADE ADMINISTRATIVA'))) dg
                              ON ua.uad_codigo = dg.cd_unidade_educacao
                              INNER JOIN CoreSME..SYS_TipoEscola tp
                              ON tp.tes_desc = dg.sg_tp_escola
                        WHERE ua.tua_id = @tua_id_escola
                        GROUP BY uad_id, tes_id) AS _source
                ON _source.uad_id = _target.uad_id
                WHEN MATCHED THEN
                     UPDATE SET tes_id = _source.tes_id
                WHEN NOT MATCHED THEN
                     INSERT(uad_id, tes_id)
                     VALUES(_source.uad_id, _source.tes_id);
            
                            --if adicionado para rodar em ambientes internos sem este banco
            
                if db_id('PortalInstitutional') is not null
                BEGIN
                    MERGE PortalInstitutional..AdministrativeUnitType _target
                    USING(SELECT tua_id, tua_nome, tua_situacao
                             FROM CoreSSO..SYS_TipoUnidadeAdministrativa tua
                                  INNER JOIN  BD_PRODAM..v_unidade_educacao_dados_gerais esc
                                  on tua.tua_nome = esc.sg_tp_escola
                            GROUP BY tua_id, tua_nome, tua_situacao) AS _source
                    ON _target.Id = _source.tua_id
                    WHEN NOT MATCHED THEN
                         INSERT(Id, Code, Name, CreationDate, UpdateDate, State)
                         VALUES(_source.tua_id, _source.tua_nome, _source.tua_nome, getdate(), getdate(), _source.tua_situacao);
                            END
                            --update adicionado conforme solicitação em 14 / 05 / 2015, para resolvermos problemas de diferenças entre a base do portal e do core
                                --MERGE adicionado conforme solicitação em 18 / 06 / 2015
                                 -- cria tabela em memória para fazer recursividade até chegar na DRE
                                 DECLARE @ESCOLA TABLE
                                   (ent_id uniqueidentifier,
                                    uad_id uniqueidentifier,
                                    uad_codigo varchar(20),
                    uad_nome varchar(200),
                    tua_id uniqueidentifier,
                    uad_idSuperior uniqueidentifier,
                    Latitude decimal(9, 6),
                    Longitude decimal(9, 6),
                    uad_situacao tinyint)
                 INSERT INTO @ESCOLA
                 SELECT ent_id, uad_id, uad_codigo, uad_nome, tua.tua_id, uad_idSuperior, esc.cd_coordenada_geo_x Latitude,
                        esc.cd_coordenada_geo_y Longitude, uad_situacao
                   FROM CoreSSO..SYS_UnidadeAdministrativa uad
                        inner join BD_PRODAM..v_unidade_educacao_dados_gerais esc
                        on uad.uad_codigo = esc.cd_unidade_educacao
                        inner join CoreSSO..SYS_TipoUnidadeAdministrativa tua
                        on esc.sg_tp_escola = tua.tua_nome
                  WHERE uad.tua_id = @tua_id_escola
                    and uad_situacao = 1
                    and tua_situacao = 1
                  group by ent_id, uad_id, uad_codigo, uad_nome, tua.tua_id, uad_idSuperior, esc.cd_coordenada_geo_x,
            	        esc.cd_coordenada_geo_y, uad_situacao
                 WHILE EXISTS(SELECT uad.uad_id
                                 from CoreSSO..SYS_UnidadeAdministrativa uad
                                      inner
                                 join @ESCOLA esc
                           on uad.uad_id = esc.uad_idSuperior
                                where uad.uad_idSuperior is not null
                                  and uad.uad_situacao = 1)
                    update esc
                       set uad_idSuperior = uad.uad_idSuperior
                       from @ESCOLA esc
                           inner
                       join CoreSSO..SYS_UnidadeAdministrativa uad
                 on esc.uad_idSuperior = uad.uad_id
                     where uad.uad_situacao = 1
                --if adicionado para rodar em ambientes internos sem este banco
                if db_id('PortalInstitutional') is not null
                BEGIN
                    MERGE PortalInstitutional..School _target
                    USING @ESCOLA _source
                    ON _target.AdministrativeUnitId = _source.uad_id
                    WHEN MATCHED AND((_target.Name <> _source.uad_nome) or(_target.State <> _source.uad_situacao)
            
                                       or(_target.AdministrativeUnitSuperiorId <> _source.uad_idSuperior)) THEN
                        UPDATE SET Name = _source.uad_nome,
                                   State = _source.uad_situacao,
                                   AdministrativeUnitSuperiorId = _source.uad_idSuperior,
                                   UpdateDate = GETDATE()
            
                    WHEN NOT MATCHED THEN
            
                         INSERT(EntityId, AdministrativeUnitId, Code, Name, AdministrativeUnitTypeId,
                                 AdministrativeUnitSuperiorId, CreationDate, UpdateDate, State, Latitude, Longitude)
            
                         VALUES(_source.ent_id, _source.uad_id, _source.uad_codigo, _source.uad_nome, _source.tua_id,
                                 _source.uad_idSuperior, GETDATE(), GETDATE(), _source.uad_situacao, _source.Latitude, _source.Longitude)
            
                    WHEN NOT MATCHED BY SOURCE THEN
            
                         UPDATE SET State = 3,
            						UpdateDate = GETDATE();
                            END
                        END ";
                var result = await SqlMapper.QueryAsync<string>(conn, query, commandTimeout: 600);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task LimparTabelasTemporarias()
        {
            using var conn = ObterConexao();
            try
            {
                var query =
                    @"TRUNCATE TABLE tmp_escola;
                    TRUNCATE TABLE tmp_distrito_setor;";
                await SqlMapper.ExecuteAsync(conn, query, commandTimeout: 600);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task AtualizaSysUnidadeAdministativa()
        {
            using var conn = ObterConexao();
            {

                try
                {
                    //var query = @" DECLARE @ent_id UNIQUEIDENTIFIER, @tua_id_escola UNIQUEIDENTIFIER, @tua_id_setor UNIQUEIDENTIFIER,
                    //                       @tua_id_biblioteca UNIQUEIDENTIFIER

                    //                  SET @ent_id = (SELECT ent_id FROM CoreSSO..SYS_Entidade WHERE LOWER(ent_sigla) = 'smesp')
                    //                  SET @tua_id_escola = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'escola')
                    //                  SET @tua_id_setor = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'setor')
                    //                  SET @tua_id_biblioteca = (SELECT tua_id FROM CoreSSO..SYS_TipoUnidadeAdministrativa WHERE LOWER(tua_nome) = 'Biblioteca')

                    //                  MERGE CoreSSO..SYS_UnidadeAdministrativa _target
                    //                  USING (SELECT cd_unidade_educacao AS uad_codigo, dc_tipo_unidade_educacao, nm_unidade_educacao AS uad_nome,
                    //                                nm_logradouro, cd_nr_endereco, nm_bairro, cd_setor_distrito, nm_micro_regiao AS nm_setor,
                    //                                LEFT(cd_setor_distrito, 2) as cd_distrito, nm_distrito_mec AS nm_distrito,
                    //                                setor.uad_id AS uad_idSuperior, cd_unidade_administrativa_referencia as cd_dre,
                    //                                CASE sg_tipo_situacao_unidade WHEN 'ATIVA' THEN 1 ELSE 3 END AS uad_situacao,
                    //                                @tua_id_escola AS tua_id_escola, @ent_id AS ent_id, cd_endereco_grh
                    //                            FROM [10.49.16.23\SME_PRD].[Manutencao].[dbo].[tmp_CoreSME_unidade_educacao_dados_gerais] ueg WITH(READUNCOMMITTED)
                    //                                INNER JOIN
                    //                                (SELECT uad_id, uad_codigo, uad_nome,
                    //                                        ROW_NUMBER() OVER (PARTITION BY uad_codigo ORDER BY uad_dataCriacao) AS rowNum
                    //                                   FROM CoreSSO..SYS_UnidadeAdministrativa WITH(READUNCOMMITTED)
                    //                                  WHERE tua_id = @tua_id_setor) AS setor
                    //                                 ON RTRIM(LTRIM(setor.uad_codigo)) = ueg.cd_setor_distrito
                    //                                AND setor.rowNum = 1
                    //                          WHERE ((dc_tipo_unidade_educacao = 'ESCOLA')
                    //                                 -- filtro para pegar os CEUs Puros
                    //                                 or(cd_unidade_educacao like '200%'
                    //                                     and dc_tipo_unidade_educacao = 'UNIDADE ADMINISTRATIVA'))
                    //                          GROUP BY cd_unidade_educacao, dc_tipo_unidade_educacao, nm_unidade_educacao, nm_logradouro,
                    //                                   cd_nr_endereco, nm_bairro, cd_setor_distrito, nm_micro_regiao, nm_distrito_mec,
                    //                                   setor.uad_id, sg_tipo_situacao_unidade, cd_unidade_administrativa_referencia,
                    //                                   cd_endereco_grh) AS _source
                    //                   ON _source.uad_codigo = _target.uad_codigo
                    //                  AND _source.tua_id_escola = _target.tua_id
                    //                  AND _source.ent_id = _target.ent_id
                    //                  WHEN MATCHED THEN
                    //                       UPDATE SET uad_nome = _source.uad_nome,
                    //                                  uad_dataAlteracao = GETDATE(),
                    //                                  uad_idSuperior = _source.uad_idSuperior,
                    //                                  uad_codigoIntegracao = _source.cd_endereco_grh,
                    //                                  uad_situacao = _source.uad_situacao
                    //                  WHEN NOT MATCHED THEN
                    //                       INSERT(ent_id, tua_id, uad_codigo, uad_nome, uad_idSuperior, uad_situacao)
                    //                       VALUES(@ent_id, @tua_id_escola, _source.uad_codigo, _source.uad_nome,
                    //                               _source.uad_idSuperior, _source.uad_situacao)
                    //                  WHEN NOT MATCHED BY SOURCE AND((_target.tua_id = @tua_id_escola) AND(_target.uad_nome not like 'LAB DRE%'))
                    //                       THEN
                    //                       UPDATE SET uad_situacao = 3, uad_dataAlteracao = GETDATE(); ";

                    var query = @"SELECT top(10) uad_id, uad_codigo, uad_nome
                                                           
                                                       FROM CoreSSO..SYS_UnidadeAdministrativa";

                    var result = await conn.QueryAsync(query);
                }
                catch (System.Exception ex)
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
}
