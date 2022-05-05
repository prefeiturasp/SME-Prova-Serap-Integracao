using Microsoft.Extensions.DependencyInjection;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao.UseCase;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dados.Mapeamentos;

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
            services.AddScoped<IRepositorioEndEndereco, RepositorioEndEndereco>();
            services.AddScoped<IRepositorioGeralCoreSso, RepositorioGeralCoreSso>();
            services.AddScoped<IRepositorioSysUnidadeAdministrativaEndereco, RepositorioSysUnidadeAdministrativaEndereco>();

            services.AddScoped<ITestCommandUseCase, TestCommandUseCase>();
            services.AddScoped<ITrataSysUnidadeAdministrativaUseCase, TrataSysUnidadeAdministrativaUseCase>();

            RegistrarMapeamentos.Registrar();

        }
    }
}
