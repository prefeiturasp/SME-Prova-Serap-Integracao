using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Infra;
using System;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TurmaEscolaSyncUseCase : AbstractUseCase, ITurmaEscolaSyncUseCase
    {
        public TurmaEscolaSyncUseCase(IMediator mediator) : base(mediator){}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var codigosEscolas = await mediator.Send(new ObterCodigoEscolasAtivasQuery());
                foreach (string codigoEscola in codigosEscolas)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TurmaEscolaTratar, codigoEscola));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [SYNC TURMA ESCOLA] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                return false;
            }
        }

    }
}
