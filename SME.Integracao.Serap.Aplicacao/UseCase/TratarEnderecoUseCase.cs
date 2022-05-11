using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarEnderecoUseCase : ITratarEnderecoUseCase
    {
        private readonly IMediator mediator;
        private ParametrosCoreSsoDto parametrosCoreSso { get; set; }

        public TratarEnderecoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                parametrosCoreSso = await mediator.Send(new ObterParametrosCoreSsoQuery());
                var unidadesAdministrativasEOL = await mediator.Send(new BuscaUnidadesAdministrativasEolQuery());
                var unidadesAdministrativasCoreSSO = await mediator.Send(new BuscaUnidadesAdministrativasCoreSSOQuery());

                var novosEnderecos = await TratarEnderecos(unidadesAdministrativasEOL, unidadesAdministrativasCoreSSO);
                var tempEnderecos = GerarListaTempEnderecos(unidadesAdministrativasEOL, unidadesAdministrativasCoreSSO, novosEnderecos);
                await TratarUnidadesAdministrativasEnderecos(tempEnderecos);

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR ENDEREÇOS] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
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

        private List<TempEnderecoDto> GerarListaTempEnderecos(IEnumerable<UnidadeEducacaoDadosGeraisDto> unidadesAdministrativasEOL,
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

            return (List<TempEnderecoDto>)query_final.ToList().Distinct();
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

        private async Task TratarUnidadesAdministrativasEnderecos(List<TempEnderecoDto> tempEnderecos)
        {
            var uaes = await mediator.Send(new ObterUnidadesAdministrativasEnderecosQuery());

            var atualizar = uaes.Where(uae => tempEnderecos.Any(end => end.EntId == uae.EntId && end.UadId == uae.UadId))
                .Select(uae => new SysUnidadeAdministrativaEndereco
                {
                    Numero = tempEnderecos.Where(end => end.EntId == uae.EntId && end.UadId == uae.UadId).FirstOrDefault().CdNrEndereco,
                    Complemento = tempEnderecos.Where(end => end.EntId == uae.EntId && end.UadId == uae.UadId).FirstOrDefault().ComplementoEndereco
                });

            var inserir = tempEnderecos.Where(end => !uaes.Any(uae => end.EntId == uae.EntId && end.UadId == uae.UadId));

            foreach (SysUnidadeAdministrativaEndereco uae in atualizar)
            {
                await mediator.Send(new AtualizarUnidadeAdministrativaEnderecoCommand(uae));
            }

            foreach (TempEnderecoDto end in inserir)
            {
                var uae = new SysUnidadeAdministrativaEndereco(end.EntId, end.UadId, end.EndId, end.CdNrEndereco, end.ComplementoEndereco);
                await mediator.Send(new InserirUnidadeAdministrativaEnderecoCommand(uae));
            }
        }
    }
}
