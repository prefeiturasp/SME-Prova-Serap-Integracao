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

        public async Task<ParametrosCoreSsoDto> ObterParametrosCoreSso()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"DECLARE
	                              @ent_id_smesp		UNIQUEIDENTIFIER
	                            , @tua_id_dre		UNIQUEIDENTIFIER
	                            , @cid_id_sao_paulo UNIQUEIDENTIFIER;

	                          SET @ent_id_smesp = (SELECT ent_id FROM sys_entidade WHERE UPPER(ent_sigla) = 'SMESP');
	                          SET @tua_id_dre = (SELECT tua_id FROM sys_tipounidadeadministrativa WHERE UPPER(tua_nome) = 'ESCOLA');
	                          SET @cid_id_sao_paulo = (SELECT TOP 1 cid_id FROM end_cidade WHERE cid_nome = 'SÃO PAULO' ORDER BY cid_integridade);
	                          
                              select @ent_id_smesp EntIdSmeSp, @tua_id_dre TuaIdDre, @cid_id_sao_paulo CidIdSaoPaulo;";

                return await conn.QueryFirstOrDefaultAsync<ParametrosCoreSsoDto>(query);
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

        public async Task<ParametrosTipoMeioContatoCoreSsoDto> ObterParametrosTipoMeioContatoCoreSso()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"DECLARE @tmc_fone UNIQUEIDENTIFIER, @tmc_mail UNIQUEIDENTIFIER, @tua_id UNIQUEIDENTIFIER
                                select @tmc_fone = tmc_id from SYS_TipoMeioContato where tmc_nome = 'TELEFONE' and tmc_situacao = 1
                                select @tmc_mail = tmc_id from SYS_TipoMeioContato where tmc_nome = 'E-MAIL' and tmc_situacao = 1
                                select @tua_id = tua_id from SYS_TipoUnidadeAdministrativa where tua_nome = 'Escola' and tua_situacao = 1
                                select @tmc_fone TmcFone, @tmc_mail TmcMail, @tua_id TuaId";

                return await conn.QueryFirstOrDefaultAsync<ParametrosTipoMeioContatoCoreSsoDto>(query);
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
