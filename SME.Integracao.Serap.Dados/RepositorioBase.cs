using SME.SERAp.Prova.Infra;
using System;

namespace SME.Integracao.Serap.Dados
{
    public abstract class RepositorioBase
    {
        private readonly ConnectionStringOptions connectionStrings;

        public RepositorioBase(ConnectionStringOptions connectionStrings)
        {
            this.connectionStrings = connectionStrings ?? throw new ArgumentNullException(nameof(connectionStrings));
        }
    }
}
