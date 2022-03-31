using MediatR;
using SME.Integracao.Serap.Dados;
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
        private readonly IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa;
        public TestCommandCommandHandler(IRepositorioSysUnidadeAdministrativa repositorio)

        {
            this.repositorioSysUnidadeAdministrativa = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }


        public async Task<bool> Handle(TestCommand request, CancellationToken cancellationToken)
        {

            try
            {
                await repositorioSysUnidadeAdministrativa.AtualizaSysUnidadeAdministativa();
                return true;
            }
            catch (Exception ex)
            {

                throw new Exception ("Teste de erro na fila" + ex.Message);
            }

        }

    }
}
