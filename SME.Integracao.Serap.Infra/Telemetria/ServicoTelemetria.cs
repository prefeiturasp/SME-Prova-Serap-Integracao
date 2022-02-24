﻿using Elastic.Apm;
using Microsoft.ApplicationInsights;
using SME.Integracao.Serap.Infra.VariaveisDeAmbiente;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Infra.Telemetria
{
  public   class ServicoTelemetria
    {
        private readonly TelemetryClient insightsClient;
        private readonly TelemetriaOptions telemetriaOptions;
        public ServicoTelemetria(TelemetryClient insightsClient, TelemetriaOptions telemetriaOptions)
        {
            this.insightsClient = insightsClient;
            this.telemetriaOptions = telemetriaOptions ?? throw new ArgumentNullException(nameof(telemetriaOptions));
        }

        public async Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            dynamic result = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                var transactionElk = Agent.Tracer.CurrentTransaction;

                await transactionElk.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    result = (await acao()) as dynamic;
                });
            }
            else
            {
                result = await acao() as dynamic;
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();

                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao, temporizador.Elapsed, true);
            }

            return result;
        }


    }
}
