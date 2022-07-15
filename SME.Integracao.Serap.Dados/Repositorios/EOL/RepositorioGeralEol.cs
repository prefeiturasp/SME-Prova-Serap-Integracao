using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioGeralEol : RepositorioEOL, IRepositorioGeralEol
    {
        public RepositorioGeralEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task<IEnumerable<TipoTurno>> ObterTipoTurnoEol()
		{

			using var conn = ObterConexao();
			try
			{
				var query = "select cd_tipo_turno Id, dc_tipo_turno Nome from tipo_turno";

				return await conn.QueryAsync<TipoTurno>(query.ToString(), commandTimeout: 600);
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
