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
                var query = @"SELECT esc_id  as                EscolaId                  
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
                return await conn.InsertAsync(escola);
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

