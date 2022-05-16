using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarDistritoSetorCommand : IRequest<bool>
    {
        public AtualizarDistritoSetorCommand(SysUnidadeAdministrativa distritoSetor)
        {
            DistritoSetor = distritoSetor;
        }

        public SysUnidadeAdministrativa DistritoSetor { get; set; }
    }
}
