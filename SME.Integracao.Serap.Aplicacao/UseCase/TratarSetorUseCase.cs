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
    public class TratarSetorUseCase : ITratarSetorUseCase
    {
        private readonly IMediator mediator;

        public TratarSetorUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var unidadesAdministrativasCoreSSO = await mediator.Send(new BuscaUnidadesAdministrativasCoreSSOQuery());
                var dadosSetores = await mediator.Send(new ObterDadosSetoresParaInserirAlterarQuery());

                var listaAtualizar = ObterListaAtualizar(dadosSetores, unidadesAdministrativasCoreSSO);
                var listaInserir = ObterListaInserir(dadosSetores, unidadesAdministrativasCoreSSO);

                foreach (SysUnidadeAdministrativa setor in listaAtualizar)
                {
                    await mediator.Send(new AtualizarDistritoSetorCommand(setor));
                }

                await mediator.Send(new InserirUnidadeAdministrativaEmCascataCommand(listaInserir));

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SysUnidadeAdministrativa));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR SETORES] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }

        public List<SysUnidadeAdministrativa> ObterListaAtualizar(IEnumerable<DadosSetorDto> dadosSetores, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativas)
        {

            var query = from setor in dadosSetores
                        join uad in unidadesAdministrativas on
                        new
                        {
                            EntId = setor.EntId,
                            TuaId = setor.TuaIdSetor,
                            UadIdSuperior = setor.UadIdDistrito,
                            CodigoSetor = setor.CodigoSetor
                        }
                        equals
                        new
                        {
                            EntId = uad.EntidadeId,
                            TuaId = uad.TuaId,
                            UadIdSuperior = uad.SuperiorId,
                            CodigoSetor = uad.Codigo
                        }
                        select
                        new
                        {
                            EntidadeId = uad.EntidadeId,
                            TuaId = uad.TuaId,
                            Codigo = uad.Codigo,
                            Nome = setor.NomeSetor,
                            Sigla = uad.Sigla,
                            SuperiorId = setor.UadIdDistrito,
                            Situacao = uad.Situacao,
                            DataCriacao = uad.DataCriacao,
                            DataAlteracao = DateTime.Now,
                            Integridade = uad.Integridade,
                            CodigoIntegracao = setor.CodigoEnderecoGrh,
                            CodigoInep = uad.CodigoInep,
                        };

            return (List<SysUnidadeAdministrativa>)query;
        }

        public List<SysUnidadeAdministrativa> ObterListaInserir(IEnumerable<DadosSetorDto> dadosSetores, IEnumerable<SysUnidadeAdministrativa> unidadesAdministrativas)
        {

            var filtro = dadosSetores.Where(x => !unidadesAdministrativas.Any(uad => x.EntId == uad.EntidadeId
                                                                                    && x.TuaIdSetor == uad.TuaId
                                                                                    && x.UadIdDistrito == uad.SuperiorId
                                                                                    && x.CodigoSetor == uad.Codigo));

            return filtro.Select(x =>
                            new SysUnidadeAdministrativa
                            {
                                EntidadeId = x.EntId,
                                TuaId = x.TuaIdSetor,
                                Codigo = x.CodigoSetor,
                                Nome = x.NomeSetor,
                                SuperiorId = x.UadIdDistrito,
                                Situacao = 1,
                                DataCriacao = DateTime.Now,
                                DataAlteracao = DateTime.Now,
                                CodigoIntegracao = x.CodigoEnderecoGrh
                            }).ToList();
        }
    }
}
