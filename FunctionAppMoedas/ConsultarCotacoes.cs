using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using FunctionAppMoedas.Data;
using FunctionAppMoedas.Models;

namespace FunctionAppMoedas;

public class ConsultarCotacoes
{
    private readonly CotacoesRepository _repository;

    public ConsultarCotacoes(CotacoesRepository repository)
    {
        _repository = repository;
    }

    [FunctionName(nameof(ConsultarCotacoes))]
    [OpenApiOperation(operationId: "Cotacoes", tags: new[] { "Cotacoes" }, Summary = "Cotacoes", Description = "Consultar Cotacoes de Moedas Estrangeiras", Visibility = OpenApiVisibilityType.Important)]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(IEnumerable<DadosCotacao>), Summary = "Ultimas Cotacoes de Moedas Estrangeiras", Description = "Ultimas Cotacoes de Moedas Estrangeiras")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger _logger)
    {
        _logger.LogInformation("Consultando cotacoes ja cadastradas...");

        var dados = _repository.GetAll();
        _logger.LogInformation($"Numero de cotacoes encontradas = {dados.Count()}");

        return new OkObjectResult(dados);
    }
}