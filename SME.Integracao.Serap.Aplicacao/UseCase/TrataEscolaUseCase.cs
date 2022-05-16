using MediatR;
using SME.Integracao.Serap.Aplicacao.Commands;
using SME.Integracao.Serap.Aplicacao.Interfaces;
using SME.Integracao.Serap.Aplicacao.Queries;
using SME.Integracao.Serap.Dominio;
using SME.Integracao.Serap.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.Integracao.Serap.Aplicacao
{
    public class TrataEscolaUseCase : ITrataEscolaUseCase
    {
        private readonly IMediator mediator;

        public TrataEscolaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var escolasSerap = await mediator.Send(new BuscaEscolasQuery());
                var escolasEolCore = await mediator.Send(new BuscaEscolasEolCoreQuery());
                await TrataAlteracaoEscolas(escolasSerap, escolasEolCore);
                await TrataInclucaoEscolas(escolasSerap, escolasEolCore);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        private async Task TrataInclucaoEscolas(IEnumerable<EscEscola> escolasSerap, IEnumerable<EscolaDto> escolasEolCore)
        {
            var listaCodigoEscolasCoresso = escolasEolCore.Where(x => x.UadSituacao == 1).Select(a => a.UadCodigo).Distinct().ToList();
            var listaCodigosEscolasSerap = escolasSerap.Select(a => a.EscCodigo).Distinct().ToList();
            var listaCodigoNovasEscolas = listaCodigoEscolasCoresso.Where(x => !listaCodigosEscolasSerap.Contains(x)).ToList();
            if (listaCodigoNovasEscolas.Count > 0)
                await InsereEscolas(escolasEolCore, listaCodigoNovasEscolas);
        }

        private async Task TrataAlteracaoEscolas(IEnumerable<EscEscola> escolasSerap, IEnumerable<EscolaDto> escolasEolCore)
        {
            if (escolasSerap != null && escolasSerap.Any())
            {
                foreach (var escolaSerap in escolasSerap)
                {
                    var escolaEolCore = escolasEolCore.FirstOrDefault(x => x.UadCodigo == escolaSerap.EscCodigo);
                    if (escolaEolCore != null && DeveAtualizar(escolaSerap, escolaEolCore))
                    {
                        escolaSerap.EscNome = escolaEolCore.UadNome;
                        escolaSerap.EscSituacao = escolaEolCore.UadSituacao;
                        escolaSerap.DataAlteracao = DateTime.Now;
                        await mediator.Send(new AtualizarEscEscolaCommand(escolaSerap));
                        //atualiza aqui
                    }
                }
            }
        }

        public bool DeveAtualizar(EscEscola escolaSerap, EscolaDto escolaEolCore)
        {
            return escolaSerap.EscNome.Trim() != escolaEolCore.UadNome.Trim() ||
                   escolaSerap.EscSituacao != escolaEolCore.UadSituacao;


        }

        private async Task InsereEscolas(IEnumerable<EscolaDto> escolasEolCore, List<string> listaCodigoNovasEscolas)
        {
            var escolaId = await mediator.Send(new BuscaValorMaiorIdQuery());
            if (listaCodigoNovasEscolas != null && listaCodigoNovasEscolas.Any())
            {
                var escolaNovasParaIncluir = escolasEolCore.Where(a => listaCodigoNovasEscolas.Contains(a.UadCodigo)).ToList();


                var listaEscolasParaIncluir = new List<EscEscola>();
                escolaNovasParaIncluir.ForEach(a =>
                {

                    escolaId = escolaId + 1;
                    var esc = new EscEscola()
                    {
                        EscId = escolaId,
                        EscNome = a.UadNome,
                        UadId = a.UadId,
                        EntId = a.EntId,
                        TuaId = a.TuaId,
                        EscCodigo = a.UadCodigo,
                        DataCriacao = DateTime.Now,
                        DataAlteracao = DateTime.Now,
                        EscSituacao = 1,
                        UadIdSuperiorGestao = a.UadIdSuperior
                    };

                    listaEscolasParaIncluir.Add(esc);
                });



                foreach (var escola in listaEscolasParaIncluir)
                {
                    var escolaComUadSuperiorDre = await EncontraDresDasEscolas(escola);

                    await mediator.Send(new InserirEscEscolaCommand(escolaComUadSuperiorDre));
                }
            }
        }


        private async Task<EscEscola> EncontraDresDasEscolas(EscEscola escola)
        {
            var ehDre = false;


            var uadIdSuperiorEscola = escola.UadIdSuperiorGestao;

            for (int i = 0; i <= 5; i++)
            {
                var UadIdSuperiorDto = await mediator.Send(new BuscaUadIdSuperiorQuery(uadIdSuperiorEscola));

                if (UadIdSuperiorDto != null)
                    uadIdSuperiorEscola = UadIdSuperiorDto.UadIdSuperior;

                if (UadIdSuperiorDto.TuaId == Guid.Parse(TipoUnidadeAdministrativa.Dre))
                {
                    ehDre = true;
                    uadIdSuperiorEscola = UadIdSuperiorDto.UadId;
                    break;
                }
            }

            if (ehDre)
                escola.UadIdSuperiorGestao = uadIdSuperiorEscola;

            return escola;











































        }
    }
}
