﻿using System.Collections.Generic;
using System.Linq;
using FunctionAppMoedas.Models;

namespace FunctionAppMoedas.Data;

public class CotacoesRepository
{
    private readonly MoedasContext _context;

    public CotacoesRepository(MoedasContext context)
    {
        _context = context;
    }

    public void Save(DadosCotacao dadosCotacao)
    {
        _context.Add<Cotacao>(new()
        {
            Sigla = dadosCotacao.Sigla,
            Origem = dadosCotacao.Origem,
            Horario = dadosCotacao.Horario!.Value,
            Valor = dadosCotacao.Valor!.Value
        });
        _context.SaveChanges();
    }

    public IEnumerable<DadosCotacao> GetAll()
    {
        return _context.Cotacoes!.OrderByDescending(c => c.Id)
            .Select(c =>
                new DadosCotacao()
                {
                    Sigla = c.Sigla,
                    Origem = c.Origem,
                    Horario = c.Horario,
                    Valor= c.Valor
                }).ToArray();
    }
}