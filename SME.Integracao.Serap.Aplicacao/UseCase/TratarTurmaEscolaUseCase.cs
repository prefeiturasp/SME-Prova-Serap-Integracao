using MediatR;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Linq;
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

                var processoId = mensagemRabbit.Mensagem.ToString();
                var processo = await mediator.Send(new ObterProcessoPorIdQuery(Guid.Parse(processoId)));

                if (processo == null || processo.Situacao == (int)SituacaoProcesso.Finalizado || processo.Situacao == (int)SituacaoProcesso.Erro)
                    return false;

                if (processo.Situacao == (int)SituacaoProcesso.Pendente)
                {
                    processo.Situacao = (int)SituacaoProcesso.Processando;
                    await AtualizarProcesso(processo);
                }
                
                var escolasProcesso = await mediator.Send(new ObterEscolasProcessoQuery(processo.Id, 50));
                if (escolasProcesso == null || !escolasProcesso.Any())
                {
                    processo.Situacao = (int)SituacaoProcesso.Finalizado;
                    await AtualizarProcesso(processo);                    
                    return true;
                }

                var anoBase = DateTime.Now.Year;
                var codigosEscolas = escolasProcesso.Select(x => x.CodigoEscola).ToArray();
                foreach (string codigoEscola in codigosEscolas)
                {
                    await mediator.Send(new CarregarTempTurmasPorEscolaCommand(codigoEscola, anoBase));
                    await mediator.Send(new TratarTurmasPorEscolaCommand(codigoEscola, anoBase));
                }

                await mediator.Send(new ExcluirEscolasProcessoCommand(processo.Id, codigosEscolas));
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TurmaEscolaTratar, processo.Id));

                return true;
            }
            catch (Exception ex)
            {
                var mensagem = $"ERRO WORKER INTEGRACAO [TRATAR TURMAS ESCOLA] - {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}";
                await RegistrarLogErro(mensagem, ex);
                throw ex;
            }
        }

        private async Task AtualizarProcesso(ProcessoSyncTurmas processo)
        {
            await mediator.Send(new AlterarProcessoCommand(processo));
        }
    }
}
