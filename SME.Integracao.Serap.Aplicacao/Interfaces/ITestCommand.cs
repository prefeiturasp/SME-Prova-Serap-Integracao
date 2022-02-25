using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITestCommandUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
