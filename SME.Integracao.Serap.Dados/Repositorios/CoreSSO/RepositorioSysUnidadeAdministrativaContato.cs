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
    public class RepositorioSysUnidadeAdministrativaContato : RepositorioCoreSSOBase, IRepositorioSysUnidadeAdministrativaContato
    {
        public RepositorioSysUnidadeAdministrativaContato(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<object> InserirUnidadeAdministrativaContato(SysUnidadeAdministrativaContato uac)
        {
            using var conn = ObterConexao();
            try
            {
                return await conn.InsertAsync(uac);
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

        public async Task AtualizarUnidadeAdministrativaContato(SysUnidadeAdministrativaContato uac)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update SYS_UnidadeAdministrativaContato
					            set uac_contato = @Contato,
					            uac_situacao = 1,
					            uac_dataAlteracao = GETDATE()
					            where 
					                ent_id = @EntId
					           and     uad_id = @UadId
					           and     uac_id = @UacId";

                var result = await conn.ExecuteAsync(query,
                    new
                    {
                        uac.EntId,
                        uac.UadId,
                        uac.UacId,
                        uac.Contato
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

        public async Task<IEnumerable<SysUnidadeAdministrativaContato>> ObterUnidadesAdministrativasContatos()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
								ent_id EntId,
								uad_id UadId,
								uac_id UacId,
								tmc_id TmcId,
								uac_contato Contato,
								uac_situacao Situacao,
								uac_dataCriacao DataCriacao,
								uac_dataAlteracao DataAlteracao
								from SYS_UnidadeAdministrativaContato";

                return await conn.QueryAsync<SysUnidadeAdministrativaContato>(query);
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
