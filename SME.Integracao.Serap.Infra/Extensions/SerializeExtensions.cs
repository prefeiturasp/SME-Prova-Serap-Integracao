using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Infra

    {
        public static class JsonSerializerExtensions
        {
            public static T ConverterObjectStringPraObjeto<T>(this string objectString)
            {
                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<T>(objectString, jsonSerializerOptions);
            }
        }
    }

