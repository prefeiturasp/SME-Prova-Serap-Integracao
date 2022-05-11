using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarUnidadeAdministrativaContatoUseCase : ITratarUnidadeAdministrativaContatoUseCase
    {
        private readonly IMediator mediator;

        public TratarUnidadeAdministrativaContatoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                await TratarUnidadesAdministrativasContatos();
                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR CONTATOS] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, $"Erros: {ex.Message}", rastreamento: ex?.StackTrace, excecaoInterna: ex.InnerException?.Message));
                return false;
            }
        }

        private async Task TratarUnidadesAdministrativasContatos()
        {
            var dadosTempDispContato = await mediator.Send(new ObterDadosContatosQuery());
            var listaUac = await mediator.Send(new ObterUnidadesAdministrativasContatosQuery());

            var atualizar = ObterListaAtualizar(dadosTempDispContato.ToList(), listaUac.ToList());
            foreach (SysUnidadeAdministrativaContato uac in atualizar)
            {
                await mediator.Send(new AtualizarUnidadeAdministrativaContatoCommand(uac));
            }

            var inserir = ObterListaInserir(dadosTempDispContato.ToList(), listaUac.ToList());
            foreach (SysUnidadeAdministrativaContato uac in inserir)
            {
                await mediator.Send(new InserirUnidadeAdministrativaContatoCommand(uac));
            }
        }

        private List<SysUnidadeAdministrativaContato> ObterListaAtualizar(List<TempDispContatoDto> dadosTempDispContato, List<SysUnidadeAdministrativaContato> listaUac)
        {
            var query = from disp in dadosTempDispContato
                        join uac in listaUac on
                        new
                        {
                            EntId = disp.EntId,
                            UadId = disp.UadId,
                            UacId = disp.UacId
                        }
                        equals
                        new
                        {
                            EntId = uac.EntId,
                            UadId = uac.UadId,
                            UacId = uac.UacId
                        }
                        where uac.Situacao == 3 || uac.Contato != disp.UacContato
                        select new { Uac = uac, Contato = disp.UacContato };

            return (List<SysUnidadeAdministrativaContato>)query.Select(uac => new SysUnidadeAdministrativaContato
            {
                EntId = uac.Uac.EntId,
                UadId = uac.Uac.UadId,
                UacId = uac.Uac.UacId,
                TmcId = uac.Uac.TmcId,
                Contato = uac.Contato,
            });
        }

        private List<SysUnidadeAdministrativaContato> ObterListaInserir(List<TempDispContatoDto> dadosTempDispContato, List<SysUnidadeAdministrativaContato> listaUac)
        {
            return dadosTempDispContato
                .Where(disp => !listaUac.Any(uac => uac.EntId == disp.EntId
                                                 && uac.UadId == disp.UadId
                                                 && uac.UacId == disp.UacId))
                .Select(disp =>
                    new SysUnidadeAdministrativaContato(disp.EntId, disp.UadId, disp.TmcId, disp.UacContato)).ToList();
        }
    }
}
