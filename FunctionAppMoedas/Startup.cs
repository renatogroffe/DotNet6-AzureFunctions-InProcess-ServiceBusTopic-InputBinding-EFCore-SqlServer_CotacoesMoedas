using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FunctionAppMoedas.Data;

[assembly: FunctionsStartup(typeof(FunctionAppMoedas.Startup))]
namespace FunctionAppMoedas;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddEntityFrameworkSqlServer()
            .AddDbContext<MoedasContext>(
                options => options.UseSqlServer(
                    Environment.GetEnvironmentVariable("BaseMoedas")));
        builder.Services.AddScoped<CotacoesRepository>();
    }
}