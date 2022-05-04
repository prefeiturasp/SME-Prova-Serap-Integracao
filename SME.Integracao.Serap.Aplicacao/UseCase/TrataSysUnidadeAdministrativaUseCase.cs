using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.UseCase
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
                var cidId = await mediator.Send(new ObterCidIdQuery());

                await TratarEnderecoUnidades(unidadesAdministrativasEOL, unidadesAdministrativasCoreSSO, cidId);

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
                        EntidadeId = Guid.Parse("6CF424DC-8EC3-E011-9B36-00155D033206"),
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
                var mensagem = $"ERRO WORKER INTEGRACAO - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";

                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }

        private async Task TratarEnderecoUnidades(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesAdministrativasEOL, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativasCoreSSO, Guid cidId)
        {
            var enderecosUnidadesParaAlterar = unidadesAdministrativasEOL
                                .Where(eol => unidadesAdministrativasCoreSSO.Any(core => core.EntidadeId == eol.EntId
                                                                                      && core.TuaId == eol.TuaIdEscola
                                                                                      && core.Codigo == eol.CodigoUnidadeEducacao));
            var enderecosParaAlterar = MapearParaEndereco(enderecosUnidadesParaAlterar, cidId);
            foreach (EndEndereco endereco in enderecosParaAlterar)
            {
                await mediator.Send(new AtualizarEnderecoCommand(endereco));
            }

            var enderecosUnidadesParaInserir = unidadesAdministrativasEOL
                .Where(eol => !unidadesAdministrativasCoreSSO.Any(core => core.EntidadeId == eol.EntId
                                                                      && core.TuaId == eol.TuaIdEscola
                                                                      && core.Codigo == eol.CodigoUnidadeEducacao));
            var enderecosParaInserir = MapearParaEndereco(enderecosUnidadesParaInserir, cidId);
            foreach (EndEndereco endereco in enderecosParaInserir)
            {
                int integridade = enderecosParaInserir.Where(end => end.Cep == endereco.Cep
                                                                 && end.Logradouro == endereco.Logradouro
                                                                 && end.Bairro == endereco.Bairro
                                                                 && end.Distrito == endereco.Distrito).Count();
                endereco.Integridade = integridade;
                await mediator.Send(new InserirEnderecoCommand(endereco));
            }
        }

        private List<EndEndereco> MapearParaEndereco(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesEducacao, Guid cidId)
        {
            return unidadesEducacao.Select(x => 
                new EndEndereco(
                                    x.CodigoCep, 
                                    x.NomeLogradouro, 
                                    x.NomeBairro, 
                                    x.NomeDistritoMec,
                                    null,
                                    cidId,
                                    1,
                                    DateTime.Now,
                                    DateTime.Now,
                                    1
                               )
            ).ToList();
        }
    }
}
