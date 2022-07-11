using Microsoft.Extensions.DependencyInjection;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao;
using SME.Integracao.Serap.Dados;
using SME.Integracao.Serap.Dados.Mapeamentos;

namespace SME.Integracao.Serap.IoC
{
    public class RegistraDependencias
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
            services.AddScoped<IRepositorioUnidadeAdministrativaContatoEol, RepositorioUnidadeAdministrativaContatoEol>();
            services.AddScoped<IRepositorioSysUnidadeAdministrativaContato, RepositorioSysUnidadeAdministrativaContato>();
            services.AddScoped<IRepositorioGeralEol, RepositorioGeralEol>();
            services.AddScoped<IRepositorioDistritoEol, RepositorioDistritoEol>();
            services.AddScoped<IRepositorioSetorEol, RepositorioSetorEol>();
            services.AddScoped<IRepositorioTipoTurno, RepositorioTipoTurno>();
            services.AddScoped<IRepositorioEscola, RepositorioEscola>();
            services.AddScoped<IRepositorioTurma, RepositorioTurma>();
            services.AddScoped<IRepositorioTurmaEol, RepositorioTurmaEol>();

            services.AddScoped<ITestCommandUseCase, TestCommandUseCase>();
            services.AddScoped<ITrataSysUnidadeAdministrativaUseCase, TrataSysUnidadeAdministrativaUseCase>();
            services.AddScoped<ITratarDistritoUseCase, TratarDistritoUseCase>();
            services.AddScoped<ITratarSetorUseCase, TratarSetorUseCase>();
            services.AddScoped<ITratarEnderecoUseCase, TratarEnderecoUseCase>();
            services.AddScoped<ITratarUnidadeAdministrativaContatoUseCase, TratarUnidadeAdministrativaContatoUseCase>();
            services.AddScoped<ITratarTipoTurnoUseCase, TratarTipoTurnoUseCase>();
            services.AddScoped<ITurmaEscolaSyncUseCase, TurmaEscolaSyncUseCase>();
            services.AddScoped<ITratarTurmaEscolaUseCase, TratarTurmaEscolaUseCase>();

            RegistrarMapeamentos.Registrar();

        }
    }
}
