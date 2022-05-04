using Dapper;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioGeralCoreSso : RepositorioCoreSSOBase, IRepositorioGeralCoreSso
    {
        public RepositorioGeralCoreSso(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<Guid> ObterCidId()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"SELECT TOP 1 cid_id FROM END_Cidade WHERE cid_nome = 'SÃO PAULO' ORDER BY cid_integridade";
                return await conn.QueryFirstOrDefaultAsync<Guid>(query);
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
