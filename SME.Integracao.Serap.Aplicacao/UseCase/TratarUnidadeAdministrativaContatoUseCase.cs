using MediatR;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarUnidadeAdministrativaContatoUseCase : AbstractUseCase, ITratarUnidadeAdministrativaContatoUseCase
    {

        public TratarUnidadeAdministrativaContatoUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                await TratarUnidadesAdministrativasContatos();
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.Distrito));
                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR CONTATOS] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
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
            var lista = new List<SysUnidadeAdministrativaContato>();
            foreach (var uac in listaUac)
            {
                var contato = dadosTempDispContato.FirstOrDefault(x => x.UadId == uac.UadId &&
                                                                       x.UacId == uac.UacId && 
                                                                       x.EntId  == uac.EntId);
                if (contato != null) 
                {
                    if (contato.UacContato.Trim() != uac.Contato.Trim() || uac.Situacao == 3)
                    {
                        var uacAlterada = new SysUnidadeAdministrativaContato()
                        {
                            EntId = contato.EntId,
                            UadId = contato.UadId,
                            UacId = contato.UacId,
                            TmcId = contato.TmcId,
                            Contato = contato.UacContato,
                        };

                        lista.Add(uacAlterada);
                    }
                }
            }
            return lista;
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
