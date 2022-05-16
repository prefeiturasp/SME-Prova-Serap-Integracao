using Dapper.FluentMap.Dommel.Mapping;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Dados.Mapeamentos
{
    public class EscEscolaMap : DommelEntityMap<EscEscola>
    {
        public EscEscolaMap()
        {
            ToTable("ESC_ESCOLA");
            Map(c => c.EscId).ToColumn("esc_id").IsKey().IsIdentity();

            Map(c => c.EntId).ToColumn("ent_id");
            Map(c => c.UadId).ToColumn("uad_id");
            Map(c => c.EscCodigo).ToColumn("esc_codigo");
            Map(c => c.EscNome).ToColumn("esc_nome");
            Map(c => c.EscSituacao).ToColumn("esc_situacao");
            Map(c => c.DataCriacao).ToColumn("esc_dataCriacao");
            Map(c => c.DataAlteracao).ToColumn("esc_dataAlteracao");
            Map(c => c.UadIdSuperiorGestao).ToColumn("uad_idSuperiorGestao");
            Map(c => c.TuaId).ToColumn("tua_id");
        }
    }
}