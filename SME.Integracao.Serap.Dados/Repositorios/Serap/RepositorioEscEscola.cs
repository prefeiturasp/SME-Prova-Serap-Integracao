using Dapper;
using SME.Integracao.Serap.Dados.Repositorios;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioEscEscola : RepositorioSerap, IRepositorioEscEscola
    {
        public RepositorioEscEscola(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }


        public async Task<IEnumerable<EscEscola>> BuscaEscolas()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"SELECT esc_id  as                EscId                  
                                   , ent_id  as                EntidadeId
                                   , uad_id  as                UadId
                                   , esc_codigo                EscCodigo
                                   , esc_nome  as              EscNome
                                   , esc_situacao  as          EscSituacao    
                                   , esc_dataCriacao as        DataCriacao
                                   , esc_dataAlteracao as      DataAlteracao
                                   , uad_idSuperiorGestao as   UadIdSuperiorGestao
                                   , tua_id as                 TuaId
                               FROM[GestaoAvaliacao_SGP].[dbo].[ESC_Escola]";

                return await conn.QueryAsync<EscEscola>(query, commandTimeout: 600);
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

        public async Task<object> InserirEscola(EscEscola escola)
        {
            using var conn = ObterConexao();
            try
            {

                var query = @"INSERT INTO [dbo] .[ESC_Escola]
                                                  ([esc_id]
                                                  ,[ent_id]
                                                  ,[uad_id]
                                                  ,[esc_codigo]
                                                  ,[esc_nome]
                                                  ,[esc_situacao]
                                                  ,[esc_dataCriacao]
                                                  ,[esc_dataAlteracao]
                                                  ,[uad_idSuperiorGestao]
                                                   ,[tua_id])
                                                     VALUES (@EscId,
                                                               @EntId,
                                                               @UadId,
                                                               @EscCodigo,
                                                               @EscNome,
                                                               @EscSituacao,
                                                               @DataCriacao,
                                                               @DataAlteracao,
                                                               @UadIdSuperiorGestao,
                                                               @TuaId)";

                return await conn.ExecuteAsync(query,
                    new
                    {
                        escola.EscId,
                        escola.EntId,
                        escola.UadId,
                        escola.EscCodigo,
                        escola.EscNome,
                        escola.EscSituacao,
                        escola.DataCriacao,
                        escola.DataAlteracao,
                        escola.UadIdSuperiorGestao,
                        escola.TuaId
                    },
                    commandTimeout: 600);;
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

        public async Task<object> AtualizarEscola(EscEscola escola)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"UPDATE [ESC_Escola] SET [esc_nome] = @EscNome, 
                                                      [esc_situacao] =  @EscSituacao,
                                                      [esc_dataAlteracao] = @DataAlteracao
                                                WHERE esc_id = @EscId";

                return await conn.ExecuteAsync(query,
                    new
                    {
                        escola.EscId,
                        escola.EscNome,
                        escola.EscSituacao,
                        escola.DataAlteracao
                    },
                    commandTimeout: 600); ;
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



        public async Task<int> BuscaMaiorIdEscolas()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"SELECT esc_id  as  EscolaId                  
                               FROM[GestaoAvaliacao_SGP].[dbo].[ESC_Escola]
                               ORDER BY esc_id desc";

                return await conn.QueryFirstAsync<int>(query, commandTimeout: 600);
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

