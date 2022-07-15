using MediatR;

namespace SME.Integracao.Serap.Aplicacao
{
    public class CarregarTempTurmasPorEscolaCommand : IRequest<bool>
    {
        public CarregarTempTurmasPorEscolaCommand(string codigoEscola, int anoBase)
        {
            CodigoEscola = codigoEscola;
            AnoBase = anoBase;
        }

        public string CodigoEscola { get; set; }
        public int AnoBase { get; set; }
    }
}
