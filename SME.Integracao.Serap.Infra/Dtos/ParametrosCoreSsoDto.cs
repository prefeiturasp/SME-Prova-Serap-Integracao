using System;

namespace SME.Integracao.Serap.Infra
{
    public class ParametrosCoreSsoDto
    {
        public ParametrosCoreSsoDto()
        {

        }

        public Guid EntIdSmeSp { get; set; }
        public Guid TuaIdDre { get; set; }
        public Guid CidIdSaoPaulo { get; set; }        
    }
}
