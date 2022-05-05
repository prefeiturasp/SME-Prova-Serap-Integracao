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
        private ParametrosCoreSsoDto parametrosCoreSso { get; set; }

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
                await PopularParametrosCoreSso();

                var novosEnderecos = await TratarEnderecos(unidadesAdministrativasEOL, unidadesAdministrativasCoreSSO);

                var tempEnderecos = GerarListaTempEnderecos(unidadesAdministrativasEOL, unidadesAdministrativasCoreSSO, novosEnderecos);

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

        private async Task PopularParametrosCoreSso()
        {
            parametrosCoreSso = await mediator.Send(new ObterParametrosCoreSsoQuery());
        }

        private async Task<List<EndEndereco>> TratarEnderecos(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesAdministrativasEOL, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativasCoreSSO)
        {
            var enderecosUnidadesParaAlterar = unidadesAdministrativasEOL
                                .Where(eol => unidadesAdministrativasCoreSSO.Any(core => core.EntidadeId == eol.EntId
                                                                                      && core.TuaId == eol.TuaIdEscola
                                                                                      && core.Codigo == eol.CodigoUnidadeEducacao));
            var enderecosParaAlterar = MapearParaEndereco(enderecosUnidadesParaAlterar);
            foreach (EndEndereco endereco in enderecosParaAlterar)
            {
                await mediator.Send(new AtualizarEnderecoCommand(endereco));
            }

            var enderecosUnidadesParaInserir = unidadesAdministrativasEOL
                .Where(eol => !unidadesAdministrativasCoreSSO.Any(core => core.EntidadeId == eol.EntId
                                                                      && core.TuaId == eol.TuaIdEscola
                                                                      && core.Codigo == eol.CodigoUnidadeEducacao));
            var enderecosParaInserir = MapearParaEndereco(enderecosUnidadesParaInserir);
            foreach (EndEndereco endereco in enderecosParaInserir)
            {
                int integridade = enderecosParaInserir.Where(end => end.Cep == endereco.Cep
                                                                 && end.Logradouro == endereco.Logradouro
                                                                 && end.Bairro == endereco.Bairro
                                                                 && end.Distrito == endereco.Distrito).Count();
                endereco.Integridade = integridade;
                var novoEndereco = await mediator.Send(new InserirEnderecoCommand(endereco));
                endereco.Id = novoEndereco.Id;
            }

            return enderecosParaInserir;
        }

        private List<object> GerarListaTempEnderecos(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesAdministrativasEOL,
                                                   IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativasCoreSSO,
                                                   List<EndEndereco> novosEnderecos)
        {
            var query = from core in unidadesAdministrativasCoreSSO
                        join eol in unidadesAdministrativasEOL on
                        new
                        {
                            Codigo = core.Codigo
                        }
                        equals
                        new
                        {
                            Codigo = eol.CodigoUnidadeEducacao
                        }
                        where core.EntidadeId == parametrosCoreSso.EntIdSmeSp
                           && core.TuaId == parametrosCoreSso.TuaIdDre
                        select new { Core = core, Eol = eol };

            var query_final = from a in query
                              join end in novosEnderecos on
                              new
                              {
                                  Logradouro = a.Eol.NomeLogradouro,
                                  Cep = a.Eol.CodigoCep,
                                  Bairro = a.Eol.NomeBairro,
                                  Distrito = a.Eol.NomeDistritoMec
                              }
                              equals
                              new
                              {
                                  Logradouro = end.Logradouro,
                                  Cep = end.Cep,
                                  Bairro = end.Bairro,
                                  Distrito = end.Distrito
                              }
                              select new
                              {
                                  EntId = parametrosCoreSso.EntIdSmeSp,
                                  UadId = a.Core.Id,
                                  EndId = end.Id,
                                  CdNrEndereco = a.Eol.CodigoNrEndereco,
                                  ComplementoEndereco = a.Eol.DescricaoComplementoEndereco,
                                  Logradouro = end.Logradouro
                              };

            return (List<object>)query_final.ToList().Distinct();
        }

        private List<EndEndereco> MapearParaEndereco(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesEducacao)
        {
            return unidadesEducacao.Select(x =>
                new EndEndereco(
                                    x.CodigoCep,
                                    x.NomeLogradouro,
                                    x.NomeBairro,
                                    x.NomeDistritoMec,
                                    null,
                                    parametrosCoreSso.CidIdSaoPaulo,
                                    1,
                                    DateTime.Now,
                                    DateTime.Now,
                                    1
                               )
            ).ToList();
        }
    }
}
