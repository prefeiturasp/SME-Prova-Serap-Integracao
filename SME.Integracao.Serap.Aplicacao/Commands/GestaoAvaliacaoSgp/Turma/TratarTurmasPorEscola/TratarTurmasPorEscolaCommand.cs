using MediatR;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TratarTurmasPorEscolaCommand : IRequest<bool>
    {
        public TratarTurmasPorEscolaCommand(string codigoEscola, int anoBase)
        {
            CodigoEscola = codigoEscola;
            AnoBase = anoBase;
        }

        public string CodigoEscola { get; set; }
        public int AnoBase { get; set; }
    }
}
