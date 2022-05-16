using MediatR;
using SME.Integracao.Serap.Dominio;

namespace SME.Integracao.Serap.Aplicacao
{
    public class InserirEnderecoCommand : IRequest<EndEndereco>
    {
        public InserirEnderecoCommand(EndEndereco endereco)
        {
            Endereco = endereco;
        }

        public EndEndereco Endereco { get; set; }
    }
}
