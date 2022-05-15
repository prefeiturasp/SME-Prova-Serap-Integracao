using Microsoft.Extensions.DependencyInjection;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dados.Mapeamentos;
using SME.Integracao.Serap.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.IoC
{
  public  class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            services.AddScoped<IRepositorioSysUnidadeAdministrativa, RepositorioSysUnidadeAdministrativa>();
            services.AddScoped<IRepositorioUnidadeEducacao, RepositorioUnidadeEducacao>();
            services.AddScoped<IRepositorioEscEscola, RepositorioEscEscola>();


            services.AddScoped<ITestCommandUseCase, TestCommandUseCase>();
            services.AddScoped<ITrataSysUnidadeAdministrativaUseCase, TrataSysUnidadeAdministrativaUseCase>();
            services.AddScoped<ITrataEscolaUseCase, TrataEscolaUseCase>();

            RegistrarMapeamentos.Registrar();

        }
    }
}
