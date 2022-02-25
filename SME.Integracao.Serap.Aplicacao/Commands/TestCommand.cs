using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Commands
{
   public class TestCommand : IRequest<bool>
    {

    }

    public class TestCommandCommandHandler : IRequestHandler<TestCommand, bool>
    {
       
        public async Task<bool> Handle(TestCommand request, CancellationToken cancellationToken)
        {
            throw new Exception("Teste de erro na fila");
        }

    }
}
