using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

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
                return await conn.InsertAsync(endereco);
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

        public async Task AtualizarEndereco(EndEndereco endereco)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update END_Endereco set end_dataAlteracao = GETDATE()
                                where end_cep = @Cep
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
