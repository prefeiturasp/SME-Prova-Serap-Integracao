using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Dominio
{
    public class SysUnidadeAdministrativa
    {
        public Guid  EntidadeId { get; set; }
        public Guid  Id { get; set; }
        public Guid  TuaId { get; set; }

        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public Guid SuperiorId { get; set; }
        public int Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int Integridade { get; set; }
        public string CodigoIntegracao { get; set; }
        public string CodigoInep { get; set; }
    }
}
