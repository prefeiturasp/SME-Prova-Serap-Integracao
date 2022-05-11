using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Dominio;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TrataSysUnidadeAdministrativaUseCase : ITrataSysUnidadeAdministrativaUseCase
    {
        private readonly IMediator mediator;

        public TrataSysUnidadeAdministrativaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var mensagem = $"WORKER INTEGRAÇÃO SUCESSO - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";

                var unidadesAdministrativasEOL = await mediator.Send(new BuscaUnidadesAdministrativasEolQuery());
                var unidadesAdministrativasCoreSSO = await mediator.Send(new BuscaUnidadesAdministrativasCoreSSOQuery());
                var parametrosCoreSso = await mediator.Send(new ObterParametrosCoreSsoQuery());

                var listaCodigosEol = unidadesAdministrativasEOL.Select(a => a.CodigoUnidadeEducacao).Distinct().ToList();
                var listaCodigosCoreSSO = unidadesAdministrativasCoreSSO.Select(a => a.Codigo).Distinct().ToList();

                var listaCodigosUnidadesNovas = listaCodigosEol.Where(x => !listaCodigosCoreSSO.Contains(x)).ToList();

                if (listaCodigosUnidadesNovas != null && listaCodigosUnidadesNovas.Any())
                {
                    var uasNovasParaIncluir = unidadesAdministrativasEOL.Where(a => listaCodigosUnidadesNovas.Contains(a.CodigoUnidadeEducacao)).ToList();

                    var uasNovasParaIncluirEntidade = uasNovasParaIncluir.Select(a => new SysUnidadeAdministrativa()
                    {
                        Codigo = a.CodigoUnidadeEducacao,
                        CodigoIntegracao = a.CodigoNrEndereco,
                        EntidadeId = parametrosCoreSso.EntIdSmeSp,
                        Nome = a.NomeUnidadeEducacao,
                        Sigla = a.SiglaTipoEscola,
                        Situacao = a.SituacaoUnidadeEducacao,
                        SuperiorId = a.UadIdSuperior,
                        TuaId = a.TuaIdEscola

                    }).ToList();

                    await mediator.Send(new InserirUnidadeAdministrativaEmCascataCommand(uasNovasParaIncluirEntidade));
                }

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR UNIDADES ADMINISTRATIVAS] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }        
    }
}
