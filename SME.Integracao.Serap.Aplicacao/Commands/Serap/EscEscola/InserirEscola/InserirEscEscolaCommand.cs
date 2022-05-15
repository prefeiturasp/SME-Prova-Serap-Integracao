using MediatR;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
   public class InserirEscEscolaCommand : IRequest<EscEscola>
    {
        public InserirEscEscolaCommand(EscEscola escola)
        {
            Escola = escola;
        }

        public EscEscola Escola { get; set; }
    }
 
}
