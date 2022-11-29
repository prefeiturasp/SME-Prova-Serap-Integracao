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
                var query = @"  ";

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
