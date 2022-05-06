using MediatR;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirUnidadeAdministrativaContatoCommandHandler : IRequestHandler<InserirUnidadeAdministrativaContatoCommand, SysUnidadeAdministrativaContato>
    {
        
        private readonly IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato;

        public InserirUnidadeAdministrativaContatoCommandHandler(IRepositorioSysUnidadeAdministrativaContato repositorioUnidadeAdministrativaContato)
        {
            this.repositorioUnidadeAdministrativaContato = repositorioUnidadeAdministrativaContato ?? throw new ArgumentNullException(nameof(repositorioUnidadeAdministrativaContato));
        }

        public async Task<SysUnidadeAdministrativaContato> Handle(InserirUnidadeAdministrativaContatoCommand request, CancellationToken cancellationToken)
        {
            return (SysUnidadeAdministrativaContato)await repositorioUnidadeAdministrativaContato.InserirUnidadeAdministrativaContato(request.UnidadeAdministrativaContato);
        }
    }
}
