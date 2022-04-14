using System;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using FunctionAppMoedas.Data;
using FunctionAppMoedas.Models;
using FunctionAppMoedas.Validations;

namespace FunctionAppMoedas
{
    public class ProcessarCotacoes
    {
        private readonly CotacoesRepository _repository;

        public ProcessarCotacoes(CotacoesRepository repository)
        {
            _repository = repository;
        }

        [FunctionName("ProcessarCotacoes")]
        public void Run([ServiceBusTrigger("topic-dolar", "processamento",
            Connection = "AzureServiceBusConnection")]DadosCotacao dadosCotacao, ILogger logger)
        {
            logger.LogInformation(
                $"Dados recebidos: {JsonSerializer.Serialize(dadosCotacao)}");

            var validationResult = new DadosCotacaoValidator().Validate(dadosCotacao);
            if (validationResult.IsValid)
            {
                _repository.Save(dadosCotacao);
                logger.LogInformation("Cotacao registrada com sucesso!");
            }
            else
            {
                logger.LogError("Dados invalidos para a Cotacao");
                foreach (var error in validationResult.Errors)
                    logger.LogError($" ## {error.ErrorMessage}");
            }
        }
    }
}