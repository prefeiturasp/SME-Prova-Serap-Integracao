﻿using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarDistritoUseCase : ITratarDistritoUseCase
    {

        private readonly IMediator mediator;

        public TratarDistritoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var unidadesAdministrativasCoreSSO = await mediator.Send(new BuscaUnidadesAdministrativasCoreSSOQuery());
                var dadosDistritos = await mediator.Send(new ObterDadosDistritoParaInserirAlterarQuery());

                var listaAtualizar = ObterListaAtualizar(dadosDistritos, unidadesAdministrativasCoreSSO);
                var listaInserir = ObterListaInserir(dadosDistritos, unidadesAdministrativasCoreSSO);

                foreach (SysUnidadeAdministrativa distrito in listaAtualizar)
                {
                    await mediator.Send(new AtualizarDistritoSetorCommand(distrito));
                }

                await mediator.Send(new InserirUnidadeAdministrativaEmCascataCommand(listaInserir));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR DISTRITOS] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }

        public List<SysUnidadeAdministrativa> ObterListaAtualizar(IEnumerable<DadosDistritoDto> dadosDistritos, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativas)
        {            

            var query = from distrito in dadosDistritos
                        join uad in unidadesAdministrativas on
                        new
                        {
                            EntId = distrito.EntId,
                            TuaId = distrito.TuaIdDistrito,
                            UadIdSuperior = distrito.UadIdDre,
                            CodigoDistrito = distrito.CodigoDistrito
                        }
                        equals
                        new
                        {
                            EntId = uad.EntidadeId,
                            TuaId = uad.TuaId,
                            UadIdSuperior = uad.SuperiorId,
                            CodigoDistrito = uad.Codigo
                        }
                        select
                        new
                        {
                            EntidadeId = uad.EntidadeId,
                            TuaId = uad.TuaId,
                            Codigo = uad.Codigo,
                            Nome = distrito.NomeDistrito,
                            Sigla = uad.Sigla,
                            SuperiorId = distrito.UadIdDre,
                            Situacao = uad.Situacao,
                            DataCriacao = uad.DataCriacao,
                            DataAlteracao = DateTime.Now,
                            Integridade = uad.Integridade,
                            CodigoIntegracao = distrito.CodigoEnderecoGrh,
                            CodigoInep = uad.CodigoInep,
                        };

            return (List<SysUnidadeAdministrativa>)query;
        }

        public List<SysUnidadeAdministrativa> ObterListaInserir(IEnumerable<DadosDistritoDto> dadosDistritos, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativas)
        {

            var filtro = dadosDistritos.Where(x => !unidadesAdministrativas.Any(uad => x.EntId == uad.EntidadeId
                                                                                    && x.TuaIdDistrito == uad.TuaId
                                                                                    && x.UadIdDre == uad.SuperiorId
                                                                                    && x.CodigoDistrito == uad.Codigo));

            return filtro.Select(x =>
                            new SysUnidadeAdministrativa
                            {
                                EntidadeId = x.EntId,
                                TuaId = x.TuaIdDistrito,
                                Codigo = x.CodigoDistrito,
                                Nome = x.NomeDistrito,
                                SuperiorId = x.UadIdDre,
                                Situacao = 1,
                                DataCriacao = DateTime.Now,
                                DataAlteracao = DateTime.Now,
                                CodigoIntegracao = x.CodigoEnderecoGrh
                            }).ToList();
        }
    }
}
