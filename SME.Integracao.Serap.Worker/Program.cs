using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.Integracao.Serap.IoC;
using System.Reflection;
using SME.Integracao.Serap.Infra;
using SME.Integracao.Serap.Infra.VariaveisDeAmbiente;
using RabbitMQ.Client;
using Microsoft.ApplicationInsights;

namespace SME.Integracao.Serap.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
          Host.CreateDefaultBuilder(args)
              .ConfigureAppConfiguration(a => a.AddUserSecrets(Assembly.GetExecutingAssembly()))
              .ConfigureServices((hostContext, services) =>
              {
                  RegistraDependencias.Registrar(services);
                  services.AddHostedService<WorkerRabbit>();
                  ConfigEnvoiromentVariables(hostContext, services);

              });

        private static void ConfigEnvoiromentVariables(HostBuilderContext hostContext, IServiceCollection services)
        {
            var conexaoDadosVariaveis = new ConnectionStringOptions();
            hostContext.Configuration.GetSection("ConnectionStrings").Bind(conexaoDadosVariaveis, c => c.BindNonPublicProperties = true);
            services.AddSingleton(conexaoDadosVariaveis);

            var rabbitOptions = new RabbitOptions();
            hostContext.Configuration.GetSection("Rabbit").Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitOptions);

            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };

            services.AddSingleton(factory);

            var conexaoRabbit = factory.CreateConnection();
            IModel channel = conexaoRabbit.CreateModel();

            services.AddSingleton(channel);
            services.AddSingleton(conexaoRabbit);

            var telemetriaOptions = new TelemetriaOptions();
            hostContext.Configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);

            var serviceProvider = services.BuildServiceProvider();

            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();

            var servicoTelemetria = new ServicoTelemetria(clientTelemetry, telemetriaOptions);

            services.AddSingleton(telemetriaOptions);

        }
    }
}
