using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarUnidadeAdministrativaContatoCommandHandler : IRequestHandler<AtualizarUnidadeAdministrativaContatoCommand, bool>
    {

        private readonly IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato;

        public AtualizarUnidadeAdministrativaContatoCommandHandler(IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato)
        {
            this.repositorioUnidadeAdministrativaContato = repositorioUnidadeAdministrativaContato ?? throw new ArgumentNullException(nameof(repositorioUnidadeAdministrativaContato));
        }
        public async Task<bool> Handle(AtualizarUnidadeAdministrativaContatoCommand request, CancellationToken cancellationToken)
        {
            await repositorioUnidadeAdministrativaContato.AtualizarUnidadeAdministrativaContato(request.UnidadeAdministrativaContato);
            return true;
        }
    }
}
