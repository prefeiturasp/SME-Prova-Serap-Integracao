using System;

namespace SME.Integracao.Serap.Dominio
{
    public class EndEndereco
    {
        public EndEndereco()
        {

        }

        public EndEndereco(string cep, string logradouro, string bairro, string distrito,
            int? zona, Guid cidId, int situacao, DateTime dataCriacao, DateTime dataAlteracao, int integridade)
        {
            Id = Guid.NewGuid();
            Cep = cep;
            Logradouro = logradouro;
            Bairro = bairro;
            Distrito = distrito;
            Zona = zona;
            CidId = cidId;
            Situacao = situacao;
            DataCriacao = dataCriacao;
            DataAlteracao = dataAlteracao;
            Integridade = integridade;
        }

        public Guid Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Distrito { get; set; }
        public int? Zona { get; set; }
        public Guid CidId { get; set; }
        public int Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public int Integridade { get; set; }
    }
}
