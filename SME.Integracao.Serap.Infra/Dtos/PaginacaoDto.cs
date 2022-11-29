namespace SME.Integracao.Serap.Infra
{
    public class PaginacaoDto
    {
        public PaginacaoDto()
        {

        }

        public PaginacaoDto(int numeroPagina, long numeroRegistros)
        {
            NumeroPagina = numeroPagina;
            NumeroRegistros = numeroRegistros;
        }

        public int NumeroPagina { get; set; }
        public long NumeroRegistros { get; set; }
    }
}
