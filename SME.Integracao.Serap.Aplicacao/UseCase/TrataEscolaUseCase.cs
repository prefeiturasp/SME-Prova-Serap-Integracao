using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao.Queries;
using SME.Integracao.Serap.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.UseCase
{
    public class TrataEscolaUseCase : ITrataEscolaUseCase
    {
        private readonly IMediator mediator;

        public TrataEscolaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var escolasSerap = await mediator.Send(new BuscaEscolasQuery());
            var escolasEolCore = await mediator.Send(new BuscaEscolasEolCoreQuery());
            var unidadesAdministrativasCoreSSO = await mediator.Send(new BuscaUnidadesAdministrativasCoreSSOQuery());

            var listaCodigoEscolasCoresso = escolasEolCore.Select(a => a.UadCodigo).Distinct().ToList();

            var listaCodigosEscolasSerap = escolasSerap.Select(a => a.EscCodigo).Distinct().ToList();

            var listaCodigoNovasEscolas = listaCodigoEscolasCoresso.Where(x => !listaCodigosEscolasSerap.Contains(x)).ToList();


            if (listaCodigoNovasEscolas != null && listaCodigoNovasEscolas.Any())
            {
                var uasNovasParaIncluir = escolasEolCore.Where(a => listaCodigoNovasEscolas.Contains(a.UadCodigo)).ToList();

                var uasNovasParaIncluirEntidade = uasNovasParaIncluir.Select(a => new EscEscola()
                {
                    EscNome = a.UadNome,
                    UadId = a.UadId,
                    EscCodigo = a.UadCodigo,
                    DataCriacao = DateTime.Now,
                    EscSituacao = a.UadSituacao,
                    UadIdSuperiorGestao = a.UadIdSuperior
                }).ToList();

            }

            return true;



            // Busca V_unidadeEducacaodadosGerais
            // Busca sysUnidade administratica com tipo escola e situcao = 1
            // Pega tipos escolas 
            // Acha as dres da escola nova 
            // Insere registro novo 
            // atualiza registro antigo
            // exclui se necessário
        }
    }
}
