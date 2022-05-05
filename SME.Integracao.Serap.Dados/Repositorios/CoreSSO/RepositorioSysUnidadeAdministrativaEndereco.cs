using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioSysUnidadeAdministrativaEndereco : RepositorioCoreSSOBase, IRepositorioSysUnidadeAdministrativaEndereco
    {
        public RepositorioSysUnidadeAdministrativaEndereco(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<object> InserirUnidadeAdministrativaEndereco(SysUnidadeAdministrativaEndereco unidadeAdministrativaEndereco)
        {
            using var conn = ObterConexao();
            try
            {
                return await conn.InsertAsync(unidadeAdministrativaEndereco);
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

        public async Task AtualizarUnidadeAdministrativaEndereco(SysUnidadeAdministrativaEndereco uae)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"UPDATE SYS_UnidadeAdministrativaEndereco 
	                                SET uae_dataAlteracao = GETDATE(),
		                                uae_numero = @Numero,
		                                uae_complemento = @Complemento
                                where ent_id = @EntId
                                  and uad_id = @UadId";

                var result = await conn.ExecuteAsync(query,
                    new
                    {
                        uae.EntId,
                        uae.UadId,
                        uae.Numero,
                        uae.Complemento
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

        public async Task<IEnumerable<SysUnidadeAdministrativaEndereco>> ObterSysUnidadesAdministrativasEnderecos()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
									ent_id EntId,
									uad_id UadId,
									uae_id UaeId,
									end_id EndId,
									uae_numero Numero,
									uae_complemento Complemento,
									uae_situacao Situacao,
									uae_dataCriacao DataCriacao,
									uae_dataAlteracao dataAlteracao,
									uae_enderecoPrincipal EnderecoPrincipal,
									uae_latitude Latitude,
									uae_longitude Longitude
									from SYS_UnidadeAdministrativaEndereco";

                return await conn.QueryAsync<SysUnidadeAdministrativaEndereco>(query);

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
