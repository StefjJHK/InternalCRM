using BIP.InternalCRM.Application;
using BIP.InternalCRM.Infrastructure;
using BIP.InternalCRM.Persistence;
using BIP.InternalCRM.Shopify;
using BIP.InternalCRM.WebApi;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var config = builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json")
    .AddJsonFile("appsettings.user.json", optional: true, reloadOnChange: true)
    .Build();

builder.Services
    .AddStronglyTypedIdTypeConverters()
    .AddControllers()
    .AddProblemDetailsConventions()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.Converters.AddStronglyTypedIdJsonConverters();
    });

builder.Services
    .AddProblemDetails(opts =>
    {
        opts.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    });

builder.Services
    .AddSwaggerGen()
    .AddSwaggerGenNewtonsoftSupport();

builder.Services
    .RegisterApplicationServices(config)
    .RegisterInfrastructureServices()
    .RegisterPersistenceServices(config)
    .RegisterShopifyServices(config);

builder.Services
    .AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(ApplicationProject.AssemblyRef);
    })
    .AddAutoMapper(typeof(Program), typeof(ApplicationProject));

builder.Services
    .AddDefaultCorrelationId();

builder.Services.AddAppCors(config);

var app = builder.Build();

app.UseProblemDetails();
app.UseCorrelationId();

app.UseSwagger();
app.UseSwaggerUI();

if (env.IsProduction())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}


app.UsePathBase("/api");
app.UseRouting();

if (config.GetValue<bool>("CorsOptions:Enabled"))
{
    app.UseCors();
}

app.MapControllers();

await app.RunAsync();