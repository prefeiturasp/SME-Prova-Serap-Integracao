using MediatR;
using SME.Integracao.Serap.Dados;
using System;
using System.Collections.Generic;
using SME.Integracao.Serap.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
   public class InserirUnidadeAdministrativaCommandHandler : IRequestHandler<InserirUnidadeAdministrativaEmCascataCommand, bool>
    {
        private readonly IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa;

        public InserirUnidadeAdministrativaCommandHandler(IRepositorioSysUnidadeAdministrativa repositorioSysUnidadeAdministrativa)
        {
            this.repositorioSysUnidadeAdministrativa = repositorioSysUnidadeAdministrativa ?? throw new System.ArgumentNullException(nameof(repositorioSysUnidadeAdministrativa));
        }
        public async Task<bool> Handle(InserirUnidadeAdministrativaEmCascataCommand request, CancellationToken cancellationToken)
        {
            foreach(var novaUnidadeAdmnistrativa in request.NovasUnidadesAdministrativas)
            {
               var retornoUnidade =  await repositorioSysUnidadeAdministrativa.InserirUnidadeAdministrativa(novaUnidadeAdmnistrativa);
                return true;
            }

          var retorno = await  repositorioSysUnidadeAdministrativa.IntegraEnderecoEhContato();


            return true;
      

        }
           
    }
   
}
