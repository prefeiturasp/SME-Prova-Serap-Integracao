using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarTipoTurnoUseCase : AbstractUseCase, ITratarTipoTurnoUseCase
    {
        public TratarTipoTurnoUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var listasParaTratar = await mediator.Send(new ObterListasTipoTurnoTratarQuery());

                foreach (TipoTurno tipoTurnoInserir in listasParaTratar.Inserir)
                    await mediator.Send(new InserirTipoTurnoCommand(tipoTurnoInserir));

                foreach (TipoTurno tipoTurnoExcluir in listasParaTratar.Excluir)
                    await mediator.Send(new ExcluirTipoTurnoPorIdCommand(tipoTurnoExcluir.Id));

                if (mensagemRabbit.Continuar)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TurmaEscolaSync));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR TIPO TURNO] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                return false;
            }
        }
    }
}
