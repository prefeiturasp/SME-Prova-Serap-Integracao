using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarTurmaEscolaUseCase : AbstractUseCase, ITratarTurmaEscolaUseCase
    {
        public TratarTurmaEscolaUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var codigoEscola = mensagemRabbit.Mensagem.ToString();
                var anoBase = DateTime.Now.Year;

                await mediator.Send(new CarregarTempTurmasPorEscolaCommand(codigoEscola, anoBase));
                await mediator.Send(new TratarTurmasPorEscolaCommand(codigoEscola, anoBase));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR TURMAS ESCOLA] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                return false;
            }
        }
    }
}
