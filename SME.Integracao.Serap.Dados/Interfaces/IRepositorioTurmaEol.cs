using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioTurmaEol
    {
        Task CarregaTempTurmasEolIntegracao(string codigoEscola, int anoLetivo);
    }
}
