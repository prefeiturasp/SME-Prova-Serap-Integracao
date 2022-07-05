using SME.Integracao.Serap.Infra;
using System;

public class MensagemRabbit
{
    public MensagemRabbit(object mensagem, Guid codigoCorrelacao, bool _continue = true)
    {
        Mensagem = mensagem;
        CodigoCorrelacao = codigoCorrelacao;
        Continue = _continue;
    }
    protected MensagemRabbit()
    {

    }

    public object Mensagem { get; set; }
    public Guid CodigoCorrelacao { get; set; }
    public bool Continue { get; set; }

    public T ObterObjetoMensagem<T>() where T : class
    {
        return Mensagem?.ToString().ConverterObjectStringPraObjeto<T>();
    }
}