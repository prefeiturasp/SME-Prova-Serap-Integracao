using System;

namespace SME.Integracao.Serap.Infra
{
    public class TempDispContatoDto
    {        
        public Guid EntId { get; set; }
        public Guid UadId { get; set; }
        public Guid UacId { get; set; }
        public Guid TmcId { get; set; }
        public string UacContato { get; set; }
    }
}
