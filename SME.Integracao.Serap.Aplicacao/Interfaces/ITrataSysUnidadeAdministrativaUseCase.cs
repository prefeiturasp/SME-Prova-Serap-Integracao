using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao.Interfaces
{
    public interface ITrataSysUnidadeAdministrativaUseCase 
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
