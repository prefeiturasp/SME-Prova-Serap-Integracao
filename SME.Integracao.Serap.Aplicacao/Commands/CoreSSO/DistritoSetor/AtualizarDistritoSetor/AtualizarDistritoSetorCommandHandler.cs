using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarDistritoSetorCommandHandler : IRequestHandler<AtualizarDistritoSetorCommand, bool>
    {

        private readonly IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa;

        public AtualizarDistritoSetorCommandHandler(IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa)
        {
            this.repositorioSysUnidadeAdministrativa = repositorioSysUnidadeAdministrativa ?? throw new ArgumentNullException(nameof(repositorioSysUnidadeAdministrativa));
        }

        public async Task<bool> Handle(AtualizarDistritoSetorCommand request, CancellationToken cancellationToken)
        {
            await repositorioSysUnidadeAdministrativa.AtualizarDistritoSetor(request.DistritoSetor);
            return true;
        }
    }
}
