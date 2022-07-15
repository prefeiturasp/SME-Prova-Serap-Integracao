using SME.Integracao.Serap.Infra;
using System;

public class MensagemRabbit
{
    public MensagemRabbit(object mensagem, Guid codigoCorrelacao, bool continuar = true)
    {
        Mensagem = mensagem;
        CodigoCorrelacao = codigoCorrelacao;
        Continuar = continuar;
    }
    protected MensagemRabbit()
    {

    }

    public object Mensagem { get; set; }
    public Guid CodigoCorrelacao { get; set; }
    public bool Continuar { get; set; }

    public T ObterObjetoMensagem<T>() where T : class
    {
        return Mensagem?.ToString().ConverterObjectStringPraObjeto<T>();
    }
}