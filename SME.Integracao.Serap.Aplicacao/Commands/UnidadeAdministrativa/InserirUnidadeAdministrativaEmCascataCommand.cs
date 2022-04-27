using MediatR;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao
{
   public class InserirUnidadeAdministrativaEmCascataCommand : IRequest<bool>
    {
        public InserirUnidadeAdministrativaEmCascataCommand(List<SysUnidadeAdministrativa> novasUnidadesAdministrativas)
        {
            NovasUnidadesAdministrativas = novasUnidadesAdministrativas;
        }

        public List<SysUnidadeAdministrativa> NovasUnidadesAdministrativas { get; set; }
    }
}
