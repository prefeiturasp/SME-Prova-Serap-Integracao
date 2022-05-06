using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class MergeEolCoreSsoUnidadeAdministrativaContatoCommandHandler : IRequestHandler<MergeEolCoreSsoUnidadeAdministrativaContatoCommand, bool>
    {
        
        private readonly IRepositorioUnidadeAdministrativaContatoEol repositorioUnidadeAdministrativaContatoEol;

        public MergeEolCoreSsoUnidadeAdministrativaContatoCommandHandler(IRepositorioUnidadeAdministrativaContatoEol repositorioUnidadeAdministrativaContatoEol)
        {
            this.repositorioUnidadeAdministrativaContatoEol = repositorioUnidadeAdministrativaContatoEol ?? throw new ArgumentNullException(nameof(repositorioUnidadeAdministrativaContatoEol));
        }

        public async Task<bool> Handle(MergeEolCoreSsoUnidadeAdministrativaContatoCommand request, CancellationToken cancellationToken)
        {
            await repositorioUnidadeAdministrativaContatoEol.MergeEolCoreSsoUnidadeAdministrativaContato();
            return true;
        }
    }
}
