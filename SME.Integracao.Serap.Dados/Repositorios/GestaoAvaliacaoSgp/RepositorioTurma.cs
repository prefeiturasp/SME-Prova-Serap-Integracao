using Dapper;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public class RepositorioTurma : RepositorioGestaoAvaliacaoSgp, IRepositorioTurma
    {
        public RepositorioTurma(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
    }
}
