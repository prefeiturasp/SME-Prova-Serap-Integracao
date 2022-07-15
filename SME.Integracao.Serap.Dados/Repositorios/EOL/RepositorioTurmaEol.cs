using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioTurmaEol : RepositorioEOL, IRepositorioTurmaEol
    {
        public RepositorioTurmaEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

		public async Task CarregaTempTurmasEolIntegracao(string codigoEscola, int anoLetivo)
		{

			using var conn = ObterConexao();
			try
			{
				string linkedServerSME = ObterLinkedServerSME();
				var queries = new List<string>();
				queries.Add(QueriesTurma.CarregaTempTurmasEolFiltroCursosIntegracao());
				queries.Add(QueriesTurma.CarregaTempTurmasEolFiltroTipoPrograma());
				queries.Add(QueriesTurma.CarregaTempTurmasEolFiltroTipoEdFisica());
				queries.Add(QueriesTurma.CarregaTempTurmasEolColunaEtapaEnsinoMedio());

				foreach (string queryEol in queries)
                {
					var query = queryEol.Replace("@linkedServerSME", linkedServerSME);
					await conn.ExecuteAsync(query.ToString(), new { codigoEscola, anoLetivo }, commandTimeout: 6000);
				}
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
