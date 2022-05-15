using SME.Integracao.Serap.Infra;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioGeralEol : RepositorioEOL, IRepositorioGeralEol
    {
        public RepositorioGeralEol(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }        
    }
}
