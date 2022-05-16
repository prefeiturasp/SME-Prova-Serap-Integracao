using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class AtualizarEnderecoCommand : IRequest<bool>
    {
        public AtualizarEnderecoCommand(EndEndereco endereco)
        {
            Endereco = endereco;
        }

        public EndEndereco Endereco { get; set; }
    }
}
