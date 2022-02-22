﻿using SME.Integracao.Serap.Infra;
using System;

public class MensagemRabbit
{
    public MensagemRabbit(object mensagem, Guid codigoCorrelacao)
    {
        Mensagem = mensagem;
        CodigoCorrelacao = codigoCorrelacao;
    }
    protected MensagemRabbit()
    {

    }

    public object Mensagem { get; set; }
    public Guid CodigoCorrelacao { get; set; }

    public T ObterObjetoMensagem<T>() where T : class
    {
        return Mensagem?.ToString().ConverterObjectStringPraObjeto<T>();
    }
}