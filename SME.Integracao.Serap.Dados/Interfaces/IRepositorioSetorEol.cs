﻿using SME.Integracao.Serap.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Dados
{
    public interface IRepositorioSetorEol
    {
        Task<IEnumerable<DadosSetorDto>> ObterDadosSetores();
    }
}