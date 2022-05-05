using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace SME.Integracao.Serap.Dados
{
   public static class DapperInterceptor
    {
        public static async Task<object> InsertAsync<TEntity>(this IDbConnection connection, TEntity entity, IDbTransaction transaction = null) where TEntity : class
        {
            try
            {
                var result = await Dommel.DommelMapper.InsertAsync<TEntity>(connection, entity, transaction);
               // insightsClient?.TrackDependency("SQLServer", "InsertAsync", nameof(entity), inicioOperacao, timer.Elapsed, true);

                return result;
            }
            catch (Exception ex)
            {
                //   insightsClient?.TrackDependency("SQLServer", "InsertAsync", $"{nameof(entity)} -> erro: {ex.Message}", inicioOperacao, timer.Elapsed, false);
                throw;
            }
        }

    }
}
