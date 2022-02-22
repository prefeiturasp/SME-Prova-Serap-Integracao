
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.Integracao.Serap.Aplicacao.Pipelines;
using System;

namespace SME.SERAp.Prova.IoC
{
    public static class RegistraMediatr
    {
        public static void AdicionarMediatr(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("SME.Integracao.Serap.Aplicacao");
            services.AddMediatR(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidacoesPipeline<,>));
        }
    }
}

