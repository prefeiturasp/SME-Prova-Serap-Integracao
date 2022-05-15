using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioEndEndereco : RepositorioCoreSSOBase, IRepositorioEndEndereco
    {

        public RepositorioEndEndereco(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<object> InserirEndereco(EndEndereco endereco)
        {
            using var conn = ObterConexao();
            try
            {
                var retorno = await conn.InsertAsync(endereco);
                return retorno;
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

        public async Task AtualizarEndereco(EndEndereco endereco)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update END_Endereco set end_dataAlteracao = GETDATE(), end_cep = @Cep
                                where right('00000000' + CONVERT(VARCHAR(8), end_cep),8) = @Cep
                                      and end_logradouro = @Logradouro
                                      and end_bairro = @Bairro
                                      and end_distrito = @Distrito;";

                var result = await conn.ExecuteAsync(query, 
                    new 
                    { 
                        endereco.Cep,
                        endereco.Logradouro,
                        endereco.Bairro,
                        endereco.Distrito
                    }, 
                    commandTimeout: 600);
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

        public async Task<IEnumerable<EndEndereco>> ObterEnderecosCoreSso()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
                                    end_id Id,
                                    right('00000000' + CONVERT(VARCHAR(8), end_cep),8) Cep,
                                    end_logradouro Logradouro,
                                    end_bairro Bairro,
                                    end_distrito Distrito,
                                    end_zona Zona,
                                    cid_id CidId,
                                    end_situacao Situacao,
                                    end_dataCriacao DataCriacao,
                                    end_dataAlteracao DataAlteracao,
                                    end_integridade Integridade
                                from END_Endereco";

                return await conn.QueryAsync<EndEndereco>(query, commandTimeout: 600);
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
